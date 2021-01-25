using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u8_lib.Disasm
{
    // handle control flow to blocks?

    public class u8_CodeBlock
    {
        public int Address;
        //public byte[] Bytes;
        public u8_cmd[] Ops;
        public string[] Disassembly; // cache?

        // We only need one jump, and then figure if its conditional or not?
        public int JumpsToBlock = -1;
        public int NextBlock = -1;


        public u8_CodeBlock(u8_cmd[] ops)
        {
            this.Ops = ops;
        }

        public override string ToString()
        {
            return $"{Ops.Length.ToString().PadRight(3)} @ { this.Address.ToString("X8")}";
        }

    }

    public class u8_Flow
    {
        // generate code blocks
        public List<u8_CodeBlock> Blocks; 
        public List<List<int>> Stubs; // functions (stubs with no parents)
        private u8_Disasm Disasm;

        public u8_Flow(u8_Disasm disasm)
        {
            this.Blocks = new List<u8_CodeBlock>();
            this.Stubs = new List<List<int>>();
            this.Disasm = disasm;
            //if (!Analyse())
            //    Console.WriteLine("Err, Flow Analysis failed OwO");
        }

        public bool Analyse()
        {
            if (this.Disasm.Buffer == null || this.Disasm.Buffer.Length == 0)
                return false;

            // Get blocks
            GetBlocks();

            // analyse block to get stubs
            GetStubs();

            return true;
        }

        private bool GetBlocks_OLD()
        {
            this.Blocks.Clear();

            // Split all blocks based on branch conditions and terminate by return
            // basically just check for anything thats capable of modifying the Instruction Pointer

            // ALL Conditional relative branch instructions:
            int ret = 2;
            int AddrBlockStart = 0;
            List<u8_cmd> Cmds = new List<u8_cmd>();
            //for (int i = 0; i < this.Disasm.Buffer.Length-6; i+= ret)
            for (int i = 0; i < 0x1000-6; i+= ret) // dont kill the CPU
            {
                var newBlock = new u8_CodeBlock(null);
                bool isEndOfBlock = false;

                // should fix end of array???
                int grabSize = 6;
                if ((i + grabSize) - this.Disasm.Buffer.Length > 0)
                    grabSize = (i + grabSize) - this.Disasm.Buffer.Length;
                byte[] buf = new byte[grabSize]; // FIX End of array error?

                // fill temp buff
                for (int b = 0; b < buf.Length; b++)
                    buf[b] = this.Disasm.Buffer[i + b];

                // get opcode
                u8_cmd opcode = new u8_cmd();
                ret = u8_disas.u8_decode_opcode(buf, buf.Length, ref opcode);
                opcode.address = i;// dafuq???
                Cmds.Add(opcode);

                // check branches
                switch (opcode.type)
                {
                    case u8_disas.U8_BGE_RAD: // conditional branch
                    case u8_disas.U8_BLT_RAD:
                    case u8_disas.U8_BGT_RAD:
                    case u8_disas.U8_BLE_RAD:
                    case u8_disas.U8_BGES_RAD:
                    case u8_disas.U8_BLTS_RAD:
                    case u8_disas.U8_BGTS_RAD:
                    case u8_disas.U8_BLES_RAD:
                    case u8_disas.U8_BNE_RAD:
                    case u8_disas.U8_BEQ_RAD:
                    case u8_disas.U8_BNV_RAD:
                    case u8_disas.U8_BOV_RAD:
                    case u8_disas.U8_BPS_RAD:
                    case u8_disas.U8_BNS_RAD:
                    case u8_disas.U8_BAL_RAD:
                        // if cond ? radr : PC+=2
                        // if op1 < 0 then negative else positive;
                        // conditions? dont care actually ;D
                        newBlock.JumpsToBlock = opcode.address + opcode.op1; // jumps to
                        newBlock.NextBlock = opcode.address += 2; // skips jumps
                        isEndOfBlock = true;
                        break;
                    case u8_disas.U8_B_AD: // branch
                    case u8_disas.U8_BL_AD:
                        // PC = cadr[15:0] (op1) + second word
                        newBlock.JumpsToBlock = (opcode.op1*0x10000) + opcode.s_word; // segment + word??
                        isEndOfBlock = true;
                        break;
                    case u8_disas.U8_B_ER:
                    case u8_disas.U8_BL_ER:
                        // jumps to er{op1} - eeeh, idk what that is yet >.<
                        isEndOfBlock = true;
                        break;
                    case u8_disas.U8_RT: // return from subroutine?
                        isEndOfBlock = true;
                        break;
                    default:
                        break;
                }

                if(isEndOfBlock)
                {
                    // save block
                    newBlock.Ops = Cmds.ToArray();
                    newBlock.Address = newBlock.Ops[0].address;
                    this.Blocks.Add(newBlock);
                    AddrBlockStart = i; // set for next
                    Cmds.Clear();
                }
            }

            return true;
        }

        private bool GetBlocks()
        {
            this.Blocks.Clear();

            // Split all blocks based on branch conditions and terminate by return
            // basically just check for anything thats capable of modifying the Instruction Pointer

            // ALL Conditional relative branch instructions:
            int ret = 2;
            int AddrBlockStart = 0;
            bool isEndOfBlock = false;
            List<u8_cmd> Cmds = new List<u8_cmd>();
            var newBlock = new u8_CodeBlock(null);
            // change to while;true loop?
            //for (int i = 0; i < this.Disasm.Buffer.Length-6; i+= ret)
            for (int i = 0x01000; i < this.Disasm.Buffer.Length - 6; i += ret) // dont kill the CPU
            {
                u8_cmd cmd = new u8_cmd();
                ret = GetBlock(i, ref cmd, ref newBlock, ref isEndOfBlock);
                cmd.address = i;
                Cmds.Add(cmd);

                if (isEndOfBlock)
                {
                    // save block
                    newBlock.Ops = Cmds.ToArray();
                    newBlock.Address = newBlock.Ops[0].address;
                    this.Blocks.Add(newBlock);
                    Cmds.Clear();
                    newBlock = new u8_CodeBlock(null);
                    isEndOfBlock = false;
                }
            }

            // TODO: Replace with BuildBlock()

            return true;
        }

        // disassembles with block propertys
        private int GetBlock(int Address, ref u8_cmd Cmd, ref u8_CodeBlock newBlock, ref bool isEndOfBlock)
        {
            // ALL Conditional relative branch instructions:
            int ret = -1;
            
            byte[] buf = new byte[6];

            if (Address + 6 > this.Disasm.Buffer.Length)
            {
                isEndOfBlock = true; // wont come back again
                return - 1; // Cia Adios
            }

            // fill temp buff
            for (int b = 0; b < buf.Length; b++)
                buf[b] = this.Disasm.Buffer[Address + b];

            // get opcode
            //u8_cmd opcode = new u8_cmd();
            ret = u8_disas.u8_decode_opcode(buf, buf.Length, ref Cmd);
            Cmd.address = Address;// dafuq???
            //Cmds.Add(opcode);

            // check branches
            switch (Cmd.type)
            {
                case u8_disas.U8_BGE_RAD: // conditional branch
                case u8_disas.U8_BLT_RAD:
                case u8_disas.U8_BGT_RAD:
                case u8_disas.U8_BLE_RAD:
                case u8_disas.U8_BGES_RAD:
                case u8_disas.U8_BLTS_RAD:
                case u8_disas.U8_BGTS_RAD:
                case u8_disas.U8_BLES_RAD:
                case u8_disas.U8_BNE_RAD:
                case u8_disas.U8_BEQ_RAD:
                case u8_disas.U8_BNV_RAD:
                case u8_disas.U8_BOV_RAD:
                case u8_disas.U8_BPS_RAD:
                case u8_disas.U8_BNS_RAD:
                case u8_disas.U8_BAL_RAD:
                    // if cond ? radr : PC+=2
                    // if op1 < 0 then negative else positive;
                    // conditions? dont care actually ;D
                    newBlock.JumpsToBlock = Cmd.address + Cmd.op1; // takes jump
                    newBlock.NextBlock = Cmd.address += 2; // continue
                    isEndOfBlock = true;
                    break;
                case u8_disas.U8_B_AD: // branch
                case u8_disas.U8_BL_AD:
                    // PC = cadr[15:0] (op1) + second word
                    newBlock.JumpsToBlock = (Cmd.op1 * 0x10000) + Cmd.s_word; // takes jump
                    newBlock.NextBlock = -1; // newBlock.JumpsToBlock; // dont set so we know its forced?
                    isEndOfBlock = true;
                    break;
                case u8_disas.U8_B_ER:
                case u8_disas.U8_BL_ER:
                    // jumps to er{op1} - eeeh, idk what that is yet >.<
                    newBlock.NextBlock = Cmd.address += 2; // continue
                    isEndOfBlock = true;
                    break;
                case u8_disas.U8_RT: // return from subroutine?
                    isEndOfBlock = true; //  there is no next ;D
                    break;
                default:
                    break;
            }

            return ret;
        }

        private u8_CodeBlock BuildBlock(int Address)
        {
            bool complete = false;
            int offset = 0;
            List<u8_cmd> Cmds = new List<u8_cmd>();
            var newBlock = new u8_CodeBlock(null);
            while (!complete)
            {
                u8_cmd cmd = new u8_cmd();
                offset += GetBlock(Address+offset, ref cmd, ref newBlock, ref complete);
                Cmds.Add(cmd);
            }

            // save block 
            newBlock.Ops = Cmds.ToArray();
            newBlock.Address = newBlock.Ops[0].address;
            this.Blocks.Add(newBlock);
            return newBlock;
        }

        private bool GetStubs()
        {
            this.Stubs.Clear();

            if (this.Blocks == null)
                return false;

            // TODO: do afterwards after main analysis is done??
            // loop blocks - check if this is end of sub


            //List<u8_CodeBlock> Splits = new List<u8_CodeBlock>(); // keep track of when we split so we can come back later
            //for (int i = 0; i < this.Blocks.Count; i++)

            bool JobsDone = false;
            int Count = 0;
            while(Count < this.Blocks.Count) // give the chance to count every block?
            {
                int depth = 0;
                List<u8_CodeBlock> CurrentSub = new List<u8_CodeBlock>();
                if (!IsAlreadyDiscovered(this.Blocks[Count].Address))
                {
                    // TODO: Split blocks based on branch destination addresses (once we get branch fully working hehe)
                    GetFlowBlocks(Count, ref CurrentSub, ref depth);   // get flow for undiscovered blocks
                    lock (this.Stubs)
                    {
                        List<int> bs = new List<int>();
                        foreach(var b in CurrentSub)
                            bs.Add(this.Blocks.IndexOf(b));
                        this.Stubs.Add(bs);
                    }
                        
                }
                Count++;
            }

            return true;
        }

        // get all blocks based on its flow
        private void GetFlowBlocks(int BlockIndex, ref List<u8_CodeBlock> CurrentSub, ref int depth)
        {
            // TOOD: Cleanup
            // check if already in CurrentSub (meaning we merging back to normal flow)
            bool isDuplicate = false;
            foreach(var b in CurrentSub)
            {
                if(b.Address == this.Blocks[BlockIndex].Address)
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
                CurrentSub.Add(this.Blocks[BlockIndex]);

            depth++; // keep track of how deep we are
            
            // check & prevent memory corrupting stuff
            if (CurrentSub[CurrentSub.Count-1].Ops.Length > 2000)
            {
                // abort and remove in case of over 2000 instructions in one block?
                CurrentSub.RemoveAt(CurrentSub.Count - 1); // dont kill the RAM ;)
                //depth--;
                return; 
            }

            // do jump block
            if (this.Blocks[BlockIndex].NextBlock != -1) // do normal blocks
            {
                // checkout the next block below
                int bIndex = FindBlockIndex(this.Blocks[BlockIndex].NextBlock);
                if (bIndex != -1 && depth < 60)
                {
                    // existing block found - do not visit again?
                    GetFlowBlocks(bIndex, ref CurrentSub, ref depth);
                }
            }
            if (this.Blocks[BlockIndex].JumpsToBlock != -1) // then do jumps
            {
                // checkout the jumpeto block
                int bIndex = FindBlockIndex(this.Blocks[BlockIndex].JumpsToBlock);
                if (bIndex != -1 && depth < 60)
                {
                    // existing block found - do not visit again?
                    GetFlowBlocks(bIndex, ref CurrentSub, ref depth);
                }
            }
            

            depth--;
            return;
        }

        private bool IsAlreadyDiscovered(int address)
        {
            lock (this.Stubs)
            {
                if (this.Stubs == null)
                    return false;

                // TODO: optimize this?
                foreach (var s in this.Stubs)
                {
                    foreach (var b in s)
                    {
                        //if (b.Address <= address && b.Ops[b.Ops.Length - 1].address >= address)
                        //if (b.Ops[0].address <= address && b.Ops[b.Ops.Length - 1].address >= address)

                       
                        if (address >= this.Blocks[b].Ops[0].address && address <= this.Blocks[b].Ops[this.Blocks[b].Ops.Length - 1].address)
                            return true;
                    }
                }
            }
            

            return false;
        }

        private u8_CodeBlock FindBlock(int address)
        {
            if (this.Blocks == null)
                return null;

            foreach(var b in this.Blocks)
            {
                //if (address >= b.Ops[0].address && address <= b.Ops[b.Ops.Length - 1].address)
                //    return b;

                // SLICEBLOCK: its okaye, we can make new blocks if this aint the right start point!
                if (address == b.Ops[0].address)
                    return b;
            }

            return null;
        }

        private int FindBlockIndex(int address)
        {
            if (address == -1)
                return -1;

            var bb = FindBlock(address);
            if(bb == null)
            {
                // SLICEBLOCK: create new block
                var block = BuildBlock(address); // this sneaky!

                // update
                bb = FindBlock(address);
            }
            return this.Blocks.IndexOf(bb);
        }
    }
}
