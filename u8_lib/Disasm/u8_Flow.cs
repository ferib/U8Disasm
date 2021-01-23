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

        // neighbour block? - no clue how to name em otherwise
        public int ParentBlock = -1; // parent
        public int BlueBlock = -1; // always true
        public int RedBlock = -1; // condition not met
        public int GreenBlock = -1; // condition met
        //public u8_CodeBlock ParentBlock; // parent
        //public u8_CodeBlock BlueBlock; // always true
        //public u8_CodeBlock RedBlock; // condition not met
        //public u8_CodeBlock GreenBlock; // condition met


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
                        newBlock.GreenBlock = opcode.address + opcode.op1; // jumps to
                        newBlock.RedBlock = opcode.address += 2; // skips jumps
                        isEndOfBlock = true;
                        break;
                    case u8_disas.U8_B_AD: // branch
                    case u8_disas.U8_BL_AD:
                        // PC = cadr[15:0] (op1) + second word
                        newBlock.BlueBlock = (opcode.op1*0x10000) + opcode.s_word; // segment + word??
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
            for (int i = 0; i < this.Disasm.Buffer.Length - 6; i += ret) // dont kill the CPU
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
                    newBlock.GreenBlock = Cmd.address + Cmd.op1; // jumps to
                    newBlock.RedBlock = Cmd.address += 2; // skips jumps
                    isEndOfBlock = true;
                    break;
                case u8_disas.U8_B_AD: // branch
                case u8_disas.U8_BL_AD:
                    // PC = cadr[15:0] (op1) + second word
                    newBlock.BlueBlock = (Cmd.op1 * 0x10000) + Cmd.s_word; // segment + word??
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
            //else
            //    depth--; // calc the depth los and subtract?

            depth++; // keep track of how deep we are
            
            // check if block jumps outside of the collection
            int[] jumps = new int[] { this.Blocks[BlockIndex].BlueBlock, this.Blocks[BlockIndex].GreenBlock, this.Blocks[BlockIndex].RedBlock };

            //// check if we need to split
            //int jumpcount = 0;
            //foreach (var j in jumps)
            //    if (j != -1)
            //        jumpcount++;

            //if (jumpcount > 1)
            //    Splits.Add(this.Blocks[BlockIndex]); // save this block so we can come back later?
            //                                // NOTE: go recursive?

            // check all 'possible 3' jumps, its either 1 or 2 destinations that need to be looked into
            foreach (var j in jumps)
            {
                if (j == -1)
                    continue; //skip
                // recursive ;D
                int bIndex = FindBlockIndex(j);
                if(bIndex != -1 && depth < 60)
                {
                    // check if already visited in this sub
                    bool isVisited = false;
                    foreach(var b in CurrentSub)
                    {
                        if (j == b.Address) // if jump_address == existing block address
                        {
                            isVisited = true;
                            break;
                        }
                    }
                    if(!isVisited)
                        GetFlowBlocks(bIndex, ref CurrentSub, ref depth); // How to stackoverflow
                }  
            }
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
                if (address >= b.Ops[0].address && address <= b.Ops[b.Ops.Length - 1].address)
                    return b;

                // its okaye, we can make new blocks if this aint the right start point!
                //if(address >= b.Ops[0].address && address <= b.Ops[0].address+2)
                //    return b;
            }

            return null;
        }

        private int FindBlockIndex(int address)
        {
            var bb = FindBlock(address);
            if(bb == null)
            {
                // create new block
                //var block = BuildBlock(address); // this sneaky!

                // update
                bb = FindBlock(address);
            }
            return this.Blocks.IndexOf(bb);
        }
    }
}
