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
        public List<List<u8_CodeBlock>> Stubs; // functions (stubs with no parents)
        private u8_Disasm Disasm;

        public u8_Flow(u8_Disasm disasm)
        {
            this.Blocks = new List<u8_CodeBlock>();
            this.Stubs = new List<List<u8_CodeBlock>>();
            this.Disasm = disasm;
            if (!Analyse())
                Console.WriteLine("Err, Flow Analysis failed OwO");
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

        private bool GetBlocks()
        {
            this.Blocks.Clear();

            // Split all blocks based on branch conditions and terminate by return
            // basically just check for anything thats capable of modifying the Instruction Pointer

            // ALL Conditional relative branch instructions:
            int ret = 2;
            int BlockStart = 0;
            List<u8_cmd> Cmds = new List<u8_cmd>();
            for (int i = 0; i < this.Disasm.Buffer.Length-6; i+= ret)
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
                    newBlock.Address = BlockStart;
                    this.Blocks.Add(newBlock);
                    BlockStart = i + 1; // set for next
                    Cmds.Clear();
                }
            }

            return true;
        }

        private bool GetStubs()
        {
            if (this.Blocks == null)
                return false;

            // TODO: do afterwards after main analysis is done??
            // loop blocks - check if this is end of sub

            List<u8_CodeBlock> currentSub = new List<u8_CodeBlock>();
            for (int i = 0; i < this.Blocks.Count; i++)
            {
                bool isEndOfSub = false;

                // check if block jumps outside of the collection
                int[] jumps = new int[] { this.Blocks[i].BlueBlock, this.Blocks[i].GreenBlock, this.Blocks[i].RedBlock };
                foreach(var j in jumps)
                {
                    if (j == -1)
                        continue; //skip

                    // check if existing block
                    var block = GetBlock(j);
                    bool isNew = true;
                    foreach(var eb in currentSub)
                    {
                        if(eb == block)
                        {
                            isNew = false;
                            break;
                        }
                    }
                    if (isNew)
                        currentSub.Add(block);
                    else
                    {
                        isEndOfSub = true; // hmm idk?
                        this.Stubs.Add(currentSub);
                        currentSub = new List<u8_CodeBlock>();
                    } 
                }

                currentSub.Add(this.Blocks[i]);
            }

            return true;
        }

        private u8_CodeBlock GetBlock(int address)
        {
            if (this.Blocks == null)
                return null;

            // TODO: optimize this?
            foreach(var b in this.Blocks)
            {
                if (b.Address == address)
                    return b;

                foreach(var o in b.Ops)
                {
                    if (o.address == address)
                        return b;
                }
            }

            return null;
        }
    }
}
