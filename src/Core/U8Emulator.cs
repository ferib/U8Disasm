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
        private void AndRegReg(U8Cmd cmd)
        {
            // this inst ANDs the content of the specified byte-sized register
            // and object and stores the result in the register
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) & this.Registers.GetRegisterByIndex((byte)cmd.Op2);

            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)res);
            this.Registers.PC += 2;

            this.Registers.PSW.Z = res == 0;
            // TODO: S 
        }
        private void AndRegO(U8Cmd cmd)
        {
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) & (byte)cmd.Op2;

            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)res);
            this.Registers.PC += 2;

            this.Registers.PSW.Z = res == 0;
            // TODO: S 
        }
        private void BAddr(U8Cmd cmd)
        {
            // this inst jumps to the specified address anywhere in the progream/code memory
            this.Registers.CSR = (byte)cmd.Op1;
            this.Registers.PC = cmd.sWord;
        }
        private void BEReg(U8Cmd cmd)
        {
            // this inst jumps within the same physical segment to the offset in the specified word-sized register
            // the program must load the target offset into the register before executing this instruction
            this.Registers.PC = this.Registers.GetERegisterByIndex((byte)cmd.Op1);
        }
        private void BLEReg(U8Cmd cmd)
        {
            // this inst is for calling routine
            // save addr of next instruction in LR and the CSR into LCSR and then jump within same physical seg
            
            // if this sub calls another sub, it must use PUSH inst to save LR and LCSR reg to the stack
            // before the first such call and POP inst to restore the LR and LCSR registers after the last one
            
            if(this.Registers.LR == 0)
            {
                // TODO:
                // push(LR)
                // push(LCSR)
            }

            this.Registers.LR = (ushort)(this.Registers.PC + 2);
            this.Registers.LCSR = this.Registers.CSR;

            this.Registers.PC = this.Registers.GetERegisterByIndex((byte)cmd.Op1);
        }
        private void BLAddr(U8Cmd cmd)
        {
            // this inst is for calling routine
            // save addr of next instruction in LR and the CSR into LCSR and then jump within same physical seg

            // if this sub calls another sub, it must use PUSH inst to save LR and LCSR reg to the stack
            // before the first such call and POP inst to restore the LR and LCSR registers after the last one

            if (this.Registers.LR == 0)
            {
                // TODO:
                // push(LR)
                // push(LCSR)
            }

            this.Registers.LR = (ushort)(this.Registers.PC + 4);
            this.Registers.LCSR = this.Registers.CSR;

            this.Registers.PC = cmd.sWord;
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
                case U8Decoder.U8_AND_O:
                    AndRegO(cmd);
                    break;
                case U8Decoder.U8_AND_R:
                    AndRegReg(cmd);
                    break;
                case U8Decoder.U8_B_AD:
                    BAddr(cmd);
                    break;
                case U8Decoder.U8_B_ER:
                    BEReg(cmd);
                    break;
                // NOTE: BC missing?
                case U8Decoder.U8_BL_AD:
                    BLAddr(cmd);
                    break;
                case U8Decoder.U8_BL_ER:
                    BLEReg(cmd);
                    break;
                default:
                    break;
            }
        }

        // #===============#
        // #    Handlers   #
        // #===============#

    }
}
