using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u8_disasm.Disasm
{
    // handle control flow to blocks?

    public class u8_CodeBlock
    {
        public int Address;
        //public byte[] Bytes;
        private u8_cmd[] Ops;
        public string[] Disassembly; // cache?

        // neighbour block? - no clue how to name em otherwise
        public u8_CodeBlock ParentBlock; // parent
        public u8_CodeBlock BlueBlock; // always true
        public u8_CodeBlock RedBlock; // condition not met
        public u8_CodeBlock GreenBlock; // condition met


        public u8_CodeBlock(u8_cmd[] ops)
        {
            this.Ops = ops;
        }

    }

    public class u8_Flow
    {
        // generate code blocks
        public List<u8_CodeBlock> Stubs; // functions
        private u8_Disasm Disasm;

        public u8_Flow(u8_Disasm disasm)
        {
            this.Stubs = new List<u8_CodeBlock>();
            this.Disasm = disasm;
            if (!Analyse())
                Console.WriteLine("Err, Flow Analysis failed OwO");
        }

        public bool Analyse()
        {
            if (this.Disasm.Buffer == null || this.Disasm.Buffer.Length == 0)
                return false;

            // fill Stubs list ;D
            GetStubs();

            return true;
        }

        private bool GetStubs()
        {
            this.Stubs.Clear();

            // Split all blocks based on branch conditions and terminate by return
            // basically just check for anything thats capable of modifying the Instruction Pointer

            // ALL Conditional relative branch instructions:
            int ret = 2;
            int BlockStart = 0;
            List<u8_cmd> Cmds = new List<u8_cmd>();
            for (int i = 0; i < this.Disasm.Buffer.Length-6; i+= ret)
            {
                bool isEnd = false;
                int inst = u8_disas.u8_decode_inst(BitConverter.ToUInt16(this.Disasm.Buffer, i));

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
                Cmds.Add(opcode);

                switch (inst)
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
                        // end of block, may end of sub
                        isEnd = true; 
                        break;
                    case u8_disas.U8_B_AD: // branch
                    case u8_disas.U8_B_ER:
                    case u8_disas.U8_BL_AD:
                    case u8_disas.U8_BL_ER:
                        // end of block, may end of sub
                        isEnd = true;
                        break;
                    case u8_disas.U8_RT: // return from subroutine? why cant i find caller instruction??
                        // end of sub
                        isEnd = true;
                        break;
                    default:
                        break;
                }

                if(isEnd)
                {
                    // save block
                    var newSub = new u8_CodeBlock(Cmds.ToArray());
                    newSub.Address = BlockStart;
                    this.Stubs.Add(newSub);
                    BlockStart = i + 1; // set for next
                    Cmds.Clear();
                }
            }

            return true;
        }
    }
}
