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
        private int EA = 0; // keep track of register EA

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
            // NOTE: [EA+] means it gets incremented after execution
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
                        case U8Decoder.U8_ADD_SP_O: // SP += value
                            AddHandler(cmd); // value ranged between -128 and +127
                            break;
                        case U8Decoder.U8_MOV_SP_ER:
                        case U8Decoder.U8_MOV_ER_SP:
                            MovHandler(cmd); // I asume ONLY this will be used to save/restore the SP
                            break;
                        case U8Decoder.U8_BL_AD:
                        case U8Decoder.U8_BL_ER:
                            Callhandler(cmd);
                            break;
                        case U8Decoder.U8_DEC_EA:
                            EA--;
                            break;
                        case U8Decoder.U8_INC_EA:
                            EA++;
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
            // another switch to keep it clean?
            switch (cmd.Type)
            {
                case U8Decoder.U8_PUSH_R: // 8bit
                    StackPtr -= 2;
                    break;
                case U8Decoder.U8_PUSH_ER: // 16 bit
                    StackPtr -= 4;
                    break;
                case U8Decoder.U8_PUSH_XR: // 32 bit
                    StackPtr -= 8;
                    break;
                case U8Decoder.U8_PUSH_QR: // 64 bit
                    StackPtr -= 16;
                    break;
                case U8Decoder.U8_PUSH_RL: // wtf? - assume thats a 8bit?
                    StackPtr -= 2;
                    break;
                default:
                    break;
            }
        }

        private void PopHandler(U8Cmd cmd)
        {
            // keep track of stack address
            // another switch to keep it clean?
            switch(cmd.Type)
            {
                case U8Decoder.U8_POP_R: // 8bit
                    StackPtr += 2;
                    break;
                case U8Decoder.U8_POP_ER: // 16 bit
                    StackPtr += 4;
                    break;
                case U8Decoder.U8_POP_XR: // 32 bit
                    StackPtr += 8;
                    break;
                case U8Decoder.U8_POP_QR: // 64 bit
                    StackPtr += 16;
                    break;
                case U8Decoder.U8_POP_RL: // wtf? - assume thats a 8bit?
                    StackPtr += 2;
                    break;
                default:
                    break;
            }
        }

        private void SubHandler(U8Cmd cmd)
        {
            // check subtraction from stack
            if(cmd.Type == U8Decoder.U8_MOV_SP_ER)
            {
                StackPtr = EA;
            }
        }

        private void AddHandler(U8Cmd cmd)
        {
            //  This instruction adds the sign-extended signed8 to the contetnts of the stack 
            //  poionter and stores the result in the stack pointer.

            //  bit 7 in signed8 is interpreted as the sign bit, so signed8 is an integer
            //  quantity between -128 and +127.
            if(U8Decoder.isNegative7Bit((byte)cmd.Op2) == 1)
                StackPtr -= U8Decoder.ABS7Bit((byte)cmd.Op2);
            else
                StackPtr += U8Decoder.ABS7Bit((byte)cmd.Op2);
        }

        private void MovHandler(U8Cmd cmd)
        {
            // saves the contents of the SP int the specified word-sized register
        }

        private void Callhandler(U8Cmd cmd)
        {
            int destination = -1;
            if(cmd.Type == U8Decoder.U8_BL_AD)
            {
                // calls CSR:addr
                // return LCSR:LR
            }else if(cmd.Type == U8Decoder.U8_BL_ER)
            {
                // calls ER register?
            }
            
            // check which arguments are used where
        }
    }
}
