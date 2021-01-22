using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u8_disasm
{
    public static class asm_u8
    {
        public struct AsmOp
        {
            public string instr;
            public string operands;
        }
        public static int dissaseble(ref byte[] buf, int len, ref u8_cmd cmd)
        {
            //u8_cmd cmd = new u8_cmd();
            if (len < 2) return -1;

            int ret = u8_disas.u8_decode_opcode(buf, len, ref cmd);

            //if (ret > 0)
            //    return cmd;

            return ret;
        }
    }
}
