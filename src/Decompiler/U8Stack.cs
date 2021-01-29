using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Structs;
using U8Disasm.Disassembler;
using U8Disasm.Analyser;

namespace U8Disasm.Decompiler
{
    public class U8Stack
    {
        // NOTE: Each function call in the asm language should make use of some stack instructions
        //       that we can use to figure out what arguments and local variables are used


        private List<U8CodeBlock> Subroutine;

        private List<short> VirtualStack; // emulate stack to guess afterwards?

        private int StackMax = 0;
        private int StackMin = 0;
        private int StackPtr = 0;

        public U8Stack(ref List<U8CodeBlock> sub)
        {
            this.Subroutine = sub;
            Analyse();
        }

        public void Analyse()
        {
            // NOTE: RT restores LR (subroutine save addr) back into PC
            // NOTE: subroutines can also end with POP
            // NOTE: BL is basicly call, saves PC to LCSR:LR
            foreach(var block in this.Subroutine)
            {
                foreach(var cmd in block.Ops)
                {
                    switch(cmd.Type)
                    {
                        case U8Decoder.U8_PUSH_ER:
                        case U8Decoder.U8_PUSH_QR:
                        case U8Decoder.U8_PUSH_R:
                        case U8Decoder.U8_PUSH_RL:
                        case U8Decoder.U8_PUSH_XR:
                            PushHandler(cmd);
                            break;
                        case U8Decoder.U8_POP_ER:
                        case U8Decoder.U8_POP_QR:
                        case U8Decoder.U8_POP_R:
                        case U8Decoder.U8_POP_RL:
                        case U8Decoder.U8_POP_XR:
                            PopHandler(cmd);
                            break;
                        case U8Decoder.U8_SUBC_R:
                        case U8Decoder.U8_SUB_R:
                            SubHandler(cmd);
                            break;
                        case U8Decoder.U8_ADD_SP_O:
                            AddHandler(cmd); //case U8Decoder.U8_ADD_ER: // add ER,ER
                            break;
                        case U8Decoder.U8_MOV_SP_ER: // saves the contents of the SP int the specified word-sized register
                            MovHandler(cmd); // I asume ONLY this will be used to alter the SP
                            break;
                        case U8Decoder.U8_BL_AD:
                        case U8Decoder.U8_BL_ER:
                            Callhandler(cmd);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void PushHandler(U8Cmd cmd)
        {
            // keep track of stack address
        }

        private void PopHandler(U8Cmd cmd)
        {
            // keep track of stack address
        }

        private void SubHandler(U8Cmd cmd)
        {
            // check subtraction from stack
            if(cmd.Type == U8Decoder.U8_MOV_SP_ER)
            {
                //StackPtr = // whats in ER?
            }
        }

        private void AddHandler(U8Cmd cmd)
        {
            // check add to stack
        }

        private void MovHandler(U8Cmd cmd)
        {
            // check add to stack
        }

        private void Callhandler(U8Cmd cmd)
        {
            int destination = -1;
            if(cmd.Type == U8Decoder.U8_BL_AD)
            {
                // calls CSR:addr
            }else if(cmd.Type == U8Decoder.U8_BL_ER)
            {
                // calls ER register?
            }
            
            // check which arguments are used where
        }
    }
}
