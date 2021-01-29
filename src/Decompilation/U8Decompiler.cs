using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Disassembler;
using U8Disasm.Structs;
using U8Disasm.Analyser;

namespace U8Disasm.Decompilation
{
    // NOTE: this compiler is based on the following paper: https://core.ac.uk/download/pdf/145692172.pdf
    public class U8Decompiler
    {
        private U8Stack U8Stack;
        private U8Graph U8Graph;
        private U8Output U8Output;

        public List<U8CodeBlock> Subroutine; // the thing we about to analyse

        public U8Decompiler(ref List<U8CodeBlock> sub)
        {
            this.U8Stack = new U8Stack(ref sub);
            this.U8Graph = new U8Graph(ref sub);
            this.U8Output = new U8Output();
        }

        public void Decompile()
        {

        }
    }
}
