using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Analyser;

namespace U8Disasm.Decompilation
{
    public class U8Graph
    {
        // TODO: we generate a graph to identify the control flow of the function, this will
        //       be used to find if's, loop's, switch tables, and its conditions used inside the functions

        private List<U8CodeBlock> Subroutine;
        public U8Graph(ref List<U8CodeBlock> sub)
        {
            this.Subroutine = sub;
        }

        // Instruction Handlers
        private void PushHandler()
        {

        }
        private void PopHandler()
        {

        }

        private void Branchhandler()
        {

        }
    }
}
