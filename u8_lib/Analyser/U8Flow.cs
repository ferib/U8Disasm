using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U8Disasm.Disassembler;
using U8Disasm.Structs;

namespace U8Disasm.Analyser
{
    public class U8Flow
    {
        // generate code blocks
        public List<U8CodeBlock> Blocks; 
        public List<List<int>> Stubs; // functions (stubs with no parents)
        public byte[] Memory;

        public U8Flow(byte[] memory)
        {
            this.Blocks = new List<U8CodeBlock>();
            this.Stubs = new List<List<int>>();
            this.Memory = memory;
            //if (!Analyse())
            //    Console.WriteLine("Err, Flow Analysis failed OwO");
        }

        public bool Analyse()
        {
            if (this.Memory == null || this.Memory.Length == 0)
                return false;

            // Get blocks
            GetBlocks();

            // analyse block to get stubs
            GetStubs();

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
            List<U8Cmd> Cmds = new List<U8Cmd>();
            var newBlock = new U8CodeBlock(null);
            // change to while;true loop?
            //for (int i = 0; i < this.Disasm.Buffer.Length-6; i+= ret)
            for (int i = 0x01000; i < this.Memory.Length - 6; i += ret) // dont kill the CPU
            {
                U8Cmd cmd = new U8Cmd();
                ret = GetBlock(i, ref cmd, ref newBlock, ref isEndOfBlock);
                cmd.Address = i;
                Cmds.Add(cmd);

                if (isEndOfBlock)
                {
                    // save block
                    newBlock.Ops = Cmds.ToArray();
                    newBlock.Address = newBlock.Ops[0].Address;
                    this.Blocks.Add(newBlock);
                    Cmds.Clear();
                    newBlock = new U8CodeBlock(null);
                    isEndOfBlock = false;
                }
            }

            // TODO: Replace with BuildBlock()

            return true;
        }

        // disassembles with block propertys
        private int GetBlock(int Address, ref U8Cmd Cmd, ref U8CodeBlock newBlock, ref bool isEndOfBlock)
        {
            // ALL Conditional relative branch instructions:
            int ret = -1;
            
            byte[] buf = new byte[6];

            if (Address + 6 > this.Memory.Length)
            {
                isEndOfBlock = true; // wont come back again
                return - 1; // Cia Adios
            }

            // fill temp buff
            for (int b = 0; b < buf.Length; b++)
                buf[b] = this.Memory[Address + b];

            // get opcode
            //u8_cmd opcode = new u8_cmd();
            ret = U8Decoder.DecodeOpcode(buf, ref Cmd);
            Cmd.Address = Address;// dafuq???
            //Cmds.Add(opcode);

            // check branches
            switch (Cmd.Type)
            {
                case U8Decoder.U8_BGE_RAD: // conditional branch
                case U8Decoder.U8_BLT_RAD:
                case U8Decoder.U8_BGT_RAD:
                case U8Decoder.U8_BLE_RAD:
                case U8Decoder.U8_BGES_RAD:
                case U8Decoder.U8_BLTS_RAD:
                case U8Decoder.U8_BGTS_RAD:
                case U8Decoder.U8_BLES_RAD:
                case U8Decoder.U8_BNE_RAD:
                case U8Decoder.U8_BEQ_RAD:
                case U8Decoder.U8_BNV_RAD:
                case U8Decoder.U8_BOV_RAD:
                case U8Decoder.U8_BPS_RAD:
                case U8Decoder.U8_BNS_RAD:
                case U8Decoder.U8_BAL_RAD:
                    // if cond ? radr : PC+=2
                    // if op1 < 0 then negative else positive;
                    // conditions? dont care actually ;D
                    newBlock.JumpsToBlock = Cmd.Address + Cmd.Op1; // takes jump
                    newBlock.NextBlock = Cmd.Address += 2; // continue
                    isEndOfBlock = true;
                    break;
                case U8Decoder.U8_B_AD: // branch
                case U8Decoder.U8_BL_AD:
                    // PC = cadr[15:0] (op1) + second word
                    newBlock.JumpsToBlock = (Cmd.Op1 * 0x10000) + Cmd.sWord; // takes jump
                    newBlock.NextBlock = -1; // newBlock.JumpsToBlock; // dont set so we know its forced?
                    isEndOfBlock = true;
                    break;
                case U8Decoder.U8_B_ER:
                case U8Decoder.U8_BL_ER:
                    // jumps to er{op1} - eeeh, idk what that is yet >.<
                    newBlock.NextBlock = Cmd.Address += 2; // continue
                    isEndOfBlock = true;
                    break;
                case U8Decoder.U8_RT: // return from subroutine?
                    isEndOfBlock = true; //  there is no next ;D
                    break;
                default:
                    break;
            }

            return ret;
        }

        private U8CodeBlock BuildBlock(int Address)
        {
            bool complete = false;
            int offset = 0;
            List<U8Cmd> Cmds = new List<U8Cmd>();
            var newBlock = new U8CodeBlock(null);
            while (!complete)
            {
                U8Cmd cmd = new U8Cmd();
                offset += GetBlock(Address+offset, ref cmd, ref newBlock, ref complete);
                Cmds.Add(cmd);
            }

            // save block 
            newBlock.Ops = Cmds.ToArray();
            newBlock.Address = newBlock.Ops[0].Address;
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

            int Count = 0;
            while(Count < this.Blocks.Count) // give the chance to count every block?
            {
                int depth = 0;
                List<U8CodeBlock> CurrentSub = new List<U8CodeBlock>();
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
        private void GetFlowBlocks(int BlockIndex, ref List<U8CodeBlock> CurrentSub, ref int Depth)
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

            Depth++; // keep track of how deep we are
            
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
                if (bIndex != -1 && Depth < 60)
                {
                    // existing block found - do not visit again?
                    GetFlowBlocks(bIndex, ref CurrentSub, ref Depth);
                }
            }
            if (this.Blocks[BlockIndex].JumpsToBlock != -1) // then do jumps
            {
                // checkout the jumpeto block
                int bIndex = FindBlockIndex(this.Blocks[BlockIndex].JumpsToBlock);
                if (bIndex != -1 && Depth < 60)
                {
                    // existing block found - do not visit again?
                    GetFlowBlocks(bIndex, ref CurrentSub, ref Depth);
                }
            }
            

            Depth--;
            return;
        }

        private bool IsAlreadyDiscovered(int Address)
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

                       
                        if (Address >= this.Blocks[b].Ops[0].Address && Address <= this.Blocks[b].Ops[this.Blocks[b].Ops.Length - 1].Address)
                            return true;
                    }
                }
            }
            

            return false;
        }

        private U8CodeBlock FindBlock(int Address)
        {
            if (this.Blocks == null)
                return null;

            foreach(var b in this.Blocks)
            {
                //if (address >= b.Ops[0].address && address <= b.Ops[b.Ops.Length - 1].address)
                //    return b;

                // SLICEBLOCK: its okaye, we can make new blocks if this aint the right start point!
                if (Address == b.Ops[0].Address)
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
