using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Disassembler;
using U8Disasm.Structs;

namespace U8Disasm.Core
{
    public class U8Emulator
    {
        public U8Registers Registers;
        public byte[] Memory;
        public byte[] ROM;

        public U8Emulator()
        {
            // mini emulating
            this.Registers = new U8Registers();
        }

        public U8Emulator(byte[] ROM)
        {
            // full emulator (for mah bucket list ;))
            this.Registers = new U8Registers();
            byte segCount = 0x0F ; // 4 bit counter?
            this.Memory = new byte[segCount * 0x10000];
            this.ROM = ROM;
        }

        private void AddERegEReg(U8Cmd cmd)
        {
            // This inst adds the contents of the second word register to the those of the 
            // first and stores the result in the first
            var sum = this.Registers.GetERegisterByIndex((byte)cmd.Op1) + this.Registers.GetERegisterByIndex((byte)cmd.Op2);
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, (ushort)sum);
            this.Registers.PC += 2;

            // TODO: this.Registers.PSW.C
            this.Registers.PSW.Z = sum == 0;
            // TODO: this.Registers.PSW.S = TRACK TOP BIT OF RESULT
            this.Registers.PSW.OV = sum > ushort.MaxValue;
            // TODO: this.Registers.PSW.HC = idk ;/
        }
        private void AddERegO(U8Cmd cmd)
        {
            // This inst adds the sign-extended immediate vlaue to the contents of the specified
            // word-sized register and stores the result in the register.
            var sum = 0;
            if (U8Decoder.isNegative7Bit((byte)cmd.Op2) == 1)
                sum = this.Registers.GetERegisterByIndex((byte)cmd.Op1) - U8Decoder.ABS7Bit((byte)cmd.Op2);
            else
                sum = this.Registers.GetERegisterByIndex((byte)cmd.Op1) + U8Decoder.ABS7Bit((byte)cmd.Op2);
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, (ushort)sum);
            this.Registers.PC += 2;

            // TODO: this.Registers.PSW.C
            this.Registers.PSW.Z = sum == 0;
            // TODO: this.Registers.PSW.S = TRACK TOP BIT OF RESULT
            this.Registers.PSW.OV = sum > ushort.MaxValue;
            // TODO: this.Registers.PSW.HC = idk ;/
        }
        private void AddRegReg(U8Cmd cmd)
        {
            // This inst adds the content of the specified byte-sized object to
            // those of the specified byte-sized register and stores the result in that register
            var sum = this.Registers.GetRegisterByIndex((byte)cmd.Op1) + this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)sum);
            this.Registers.PC += 2;

            // TODO: this.Registers.PSW.C
            this.Registers.PSW.Z = sum == 0;
            // TODO: this.Registers.PSW.S = TRACK TOP BIT OF RESULT
            this.Registers.PSW.OV = sum > byte.MaxValue;
            // TODO: this.Registers.PSW.HC = idk ;/
        }
        private void AddRegO(U8Cmd cmd)
        {
            // This inst adds the content of the specified byte-sized object to
            // those of the specified byte-sized register and stores the result in that register
            var sum = 0;
                sum = this.Registers.GetRegisterByIndex((byte)cmd.Op1) + (byte)cmd.Op2;
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)sum);
            this.Registers.PC += 2;

            // TODO: this.Registers.PSW.C
            this.Registers.PSW.Z = sum == 0;
            // TODO: this.Registers.PSW.S = TRACK TOP BIT OF RESULT
            this.Registers.PSW.OV = sum > byte.MaxValue;
            // TODO: this.Registers.PSW.HC = idk ;/
        }
        private void AddSpO(U8Cmd cmd)
        {
            // This inst adds the sign-extended signed8 to the contents of the stack pointer
            // and stores the result in the stack pointer
            if (U8Decoder.isNegative7Bit((byte)cmd.Op1) == 1)
                this.Registers.SP -= U8Decoder.ABS7Bit((byte)cmd.Op1); // TODO: verify, may need Op2!!!
            else
                this.Registers.SP += U8Decoder.ABS7Bit((byte)cmd.Op1);
             
            this.Registers.PC += 2;
        }
        private void AddcRegReg(U8Cmd cmd)
        {
            // This inst adds the contents of the specified byte-sized register,
            // the specified byte-sized objectm and the carry flag C and stores the result in the register
            var sum = this.Registers.GetRegisterByIndex((byte)cmd.Op1) +
                this.Registers.GetRegisterByIndex((byte)cmd.Op2);

            if (this.Registers.PSW.C)
                sum++;

            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)sum);
            this.Registers.PC += 2;

            // TODO: C
            this.Registers.PSW.Z = this.Registers.PSW.Z == true && sum == 0;
            // TODO: S
            this.Registers.PSW.OV = sum > byte.MaxValue;
            // TODO: HC
        }
        private void AddcRegO(U8Cmd cmd)
        {
            // This inst adds the contents of the specified byte-sized register,
            // the specified byte-sized objectm and the carry flag C and stores the result in the register
            var sum = this.Registers.GetRegisterByIndex((byte)cmd.Op1) + (byte)cmd.Op2;

            if (this.Registers.PSW.C)
                sum++;

            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)sum);
            this.Registers.PC += 2;

            // TODO: C
            this.Registers.PSW.Z = this.Registers.PSW.Z == true && sum == 0;
            // TODO: S
            this.Registers.PSW.OV = sum > byte.MaxValue;
            // TODO: HC
        }
        public void Execute(U8Cmd cmd)
        {
            switch (cmd.Type)
            {
                case U8Decoder.U8_ADD_ER:
                    AddERegEReg(cmd);
                    break;
                case U8Decoder.U8_ADD_ER_O:
                    AddERegO(cmd);
                    break;
                case U8Decoder.U8_ADD_R:
                    AddRegReg(cmd);
                    break;
                case U8Decoder.U8_ADD_O:
                    AddRegO(cmd);
                    break;
                case U8Decoder.U8_ADD_SP_O:
                    AddSpO(cmd);
                    break;
                case U8Decoder.U8_ADDC_R:
                    AddcRegReg(cmd);
                    break;
                case U8Decoder.U8_ADDC_O:
                    AddcRegO(cmd);
                    break;
                //case U8Decoder.U8_AND_O:
                //case U8Decoder.U8_AND_R:
                // OLD ones
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
                    this.Registers.EA--;
                    this.Registers.PC += 2;
                    break;
                case U8Decoder.U8_MOV_CER_EAP: // all increase EA after execution
                case U8Decoder.U8_MOV_CR_EAP:
                case U8Decoder.U8_MOV_CXR_EAP:
                case U8Decoder.U8_MOV_CQR_EAP:
                case U8Decoder.U8_L_ER_EAP:
                case U8Decoder.U8_L_R_EAP:
                case U8Decoder.U8_L_XR_EAP:
                case U8Decoder.U8_L_QR_EAP:
                case U8Decoder.U8_ST_ER_EAP:
                case U8Decoder.U8_ST_R_EAP:
                case U8Decoder.U8_ST_XR_EAP:
                case U8Decoder.U8_ST_QR_EAP:
                case U8Decoder.U8_INC_EA:
                    this.Registers.EA++;
                    this.Registers.PC += 2;
                    break;
                case U8Decoder.U8_MOV_EAP_CER: // inc EA
                case U8Decoder.U8_MOV_EAP_CR:
                case U8Decoder.U8_MOV_EAP_CXR:
                case U8Decoder.U8_MOV_EAP_CQR:
                    MovHandler(cmd);
                    this.Registers.EA++;
                    break;
                case U8Decoder.U8_MOV_EA_CER:
                case U8Decoder.U8_MOV_EA_CR:
                case U8Decoder.U8_MOV_EA_CXR:
                case U8Decoder.U8_MOV_EA_CQR:
                    MovHandler(cmd);
                    break;
                case U8Decoder.U8_LEA_ER:
                    // loads EA with ERm
                    // TODO: EA = [erX]
                    break;
                case U8Decoder.U8_LEA_DA:
                    // loads EA with sword
                    this.Registers.EA = cmd.sWord;
                    this.Registers.PC += 4;
                    break;
                case U8Decoder.U8_LEA_D16_ER:
                    // loads EA with Disp16[ERm]
                    this.Registers.EA = (ushort)((cmd.Op1 * 0x10000) + cmd.sWord); // TODO: verify (= 0000h[erX])
                    this.Registers.PC += 4;
                    break;
                default:
                    break;
            }
        }

        // #===============#
        // #    Handlers   #
        // #===============#

        private void PushHandler(U8Cmd cmd)
        {
            // keep track of stack address
            // another switch to keep it clean?
            switch (cmd.Type)
            {
                case U8Decoder.U8_PUSH_R: // 8bit
                    this.Registers.SP -= 2;
                    break;
                case U8Decoder.U8_PUSH_ER: // 16 bit
                    this.Registers.SP -= 4;
                    break;
                case U8Decoder.U8_PUSH_XR: // 32 bit
                    this.Registers.SP -= 8;
                    break;
                case U8Decoder.U8_PUSH_QR: // 64 bit
                    this.Registers.SP -= 16;
                    break;
                case U8Decoder.U8_PUSH_RL: // wtf? - assume thats a 8bit?
                    this.Registers.SP -= 2;
                    break;
                default:
                    break;
            }
            this.Registers.PC += 2;
        }

        private void PopHandler(U8Cmd cmd)
        {
            // keep track of stack address
            // another switch to keep it clean?
            switch (cmd.Type)
            {
                case U8Decoder.U8_POP_R: // 8bit
                    this.Registers.SP += 2;
                    break;
                case U8Decoder.U8_POP_ER: // 16 bit
                    this.Registers.SP += 4;
                    break;
                case U8Decoder.U8_POP_XR: // 32 bit
                    this.Registers.SP += 8;
                    break;
                case U8Decoder.U8_POP_QR: // 64 bit
                    this.Registers.SP += 16;
                    break;
                case U8Decoder.U8_POP_RL: // wtf? - assume thats a 8bit?
                    this.Registers.SP += 2;
                    break;
                default:
                    break;
            }
            this.Registers.PC += 2;
        }

        private void SubHandler(U8Cmd cmd)
        {
            // check subtraction from stack
            if (cmd.Type == U8Decoder.U8_MOV_SP_ER)
            {
                if (this.Memory != null)
                    this.Registers.SP = this.Memory[this.Registers.EA];
                this.Registers.PC += 2;
            }
        }

        private void AddHandler(U8Cmd cmd)
        {
            //  This instruction adds the sign-extended signed8 to the contetnts of the stack 
            //  poionter and stores the result in the stack pointer.

            //  bit 7 in signed8 is interpreted as the sign bit, so signed8 is an integer
            //  quantity between -128 and +127.
            if (U8Decoder.isNegative7Bit((byte)cmd.Op2) == 1)
                this.Registers.SP -= U8Decoder.ABS7Bit((byte)cmd.Op2);
            else
                this.Registers.SP += U8Decoder.ABS7Bit((byte)cmd.Op2);
        }

        private void MovHandler(U8Cmd cmd)
        {
            // saves the contents of the SP int the specified word-sized register

            if (cmd.Type == U8Decoder.U8_MOV_SP_ER)
            {
                // restore stack ptr
                this.Registers.SP = this.Registers.GetERegisterByIndex((byte)cmd.Op1);
            }
            else if (cmd.Type == U8Decoder.U8_MOV_ER_SP)
            {
                // save stack ptr
                this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.SP);
            }
            else
            {
                // EA gets overwritten

            }
            this.Registers.PC += 2;
        }

        private void Callhandler(U8Cmd cmd)
        {
            if (cmd.Type == U8Decoder.U8_BL_AD)
            {
                // return LCSR:LR
                this.Registers.LR = (ushort)(this.Registers.PC+4);

                // calls CSR:addr
                this.Registers.PC = (ushort)((this.Registers.CSR*0x10000) + cmd.sWord);
            }
            else if (cmd.Type == U8Decoder.U8_BL_ER)
            {
                // save return
                this.Registers.LR = (ushort)(this.Registers.PC + 2);

                // calls ER register?
                if (this.Memory != null)
                    this.Registers.PC = this.Memory[this.Registers.GetERegisterByIndex((byte)cmd.Op1)];
            }
            
        }

    }
}
