using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Structs;
using U8Disasm.Disassembler;
using U8Disasm.Analyser;

namespace U8Disasm.Decompilation
{
    public class U8Stack
    {
        // NOTE: Each function call in the asm language should make use of some stack instructions
        //       that we can use to figure out what arguments and local variables are used

        private List<U8CodeBlock> Subroutine;

        public U8Stack(ref List<U8CodeBlock> sub)
        {
            this.Subroutine = sub;
        }
    }
}
