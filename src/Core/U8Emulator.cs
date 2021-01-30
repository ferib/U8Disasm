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
        private void BRK(U8Cmd cmd)
        {
            if(this.Registers.PSW.ELEVEL > 1)
            {
                // CPU system reset
                this.Registers.Initialise();
                if (this.Memory != null)
                {
                    this.Registers.SP = BitConverter.ToUInt16(this.Memory, 0);
                    this.Registers.SP = BitConverter.ToUInt16(this.Memory, 2);
                }
            }
            else if(this.Registers.PSW.ELEVEL < 2)
            {
                // equivalent of nonmaskable interrupt. CPU then loads the PC with the word data from vector
                // tabvle address 4 at the beginning of the code/program memory space
                // TODO: add vector table!
            }
            
        }
        private void CMPERegEReg(U8Cmd cmd)
        {
            // this inst compars the contents of the two word-sized reg by
            // subtracting the latter form the format and setting the PSW flags
            // (register contenst dont change)

            var res = this.Registers.GetERegisterByIndex((byte)cmd.Op1) - this.Registers.GetERegisterByIndex((byte)cmd.Op2);

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PSW.OV = res > ushort.MaxValue;
            //HC

            this.Registers.PC += 2;
        }
        private void CMPRegReg(U8Cmd cmd)
        {
            // this inst compares the contents of the specified byte-sized reg and object by
            // subtracting the latter form the format and setting the PSW flags
            // (register contenst dont change)

            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) - this.Registers.GetRegisterByIndex((byte)cmd.Op2);

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PSW.OV = res > byte.MaxValue;
            //HC

            this.Registers.PC += 2;
        }
        private void CMPRegO(U8Cmd cmd)
        {
            // this inst compares the contents of the specified byte-sized reg and object by
            // subtracting the latter form the format and setting the PSW flags
            // (register contenst dont change)

            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) - (byte)cmd.Op2;

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PSW.OV = res > byte.MaxValue;
            //HC

            this.Registers.PC += 2;
        }
        private void CMPCRegO(U8Cmd cmd)
        {
            // this inst comapres the contents of the reg and obj by subtracting the latter and the
            // carry flag from the former and setting the PSW flags
            // (register content doesnt change)
            // can be used after a CMP to compate multibyte sequences
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) - (byte)cmd.Op2;
            if (this.Registers.PSW.C)
                res--;

            this.Registers.PC += 2;

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PSW.OV = res > byte.MaxValue;
            // HC
        }
        private void CMPCRegReg(U8Cmd cmd)
        {
            // this inst comapres the contents of the reg and obj by subtracting the latter and the
            // carry flag from the former and setting the PSW flags
            // (register content doesnt change)
            // can be used after a CMP to compate multibyte sequences
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) - this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            if (this.Registers.PSW.C)
                res--;

            this.Registers.PC += 2;

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PSW.OV = res > byte.MaxValue;
            // HC
        }
        private void CPLC(U8Cmd cmd)
        {
            // this instr inverst the contents of the carry flag
            this.Registers.PSW.C = !this.Registers.PSW.C;
            this.Registers.PC += 2;
        }
        private void DAAReg(U8Cmd cmd)
        {
            // this inst convert the contents of the specified byte-sized register into a binary
            // coded decimal value by adding the appropriate value, based on the content of the register
            // as well as the C and HC flags

            byte res = this.Registers.GetRegisterByIndex((byte)cmd.Op1);
            // TODO: implement this weird adjustment table?

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            // HC
            this.Registers.PC += 2;
        }
        private void DASReg(U8Cmd cmd)
        {
            // this inst convert the contents of the specified byte-sized register into a binary
            // coded decimal value by subtracting the approperiate value, based on the
            // contents of the register as well as the C and HC flags

            byte res = this.Registers.GetRegisterByIndex((byte)cmd.Op1);
            // TODO: implement this weird adjustment table?

            // C
            this.Registers.PSW.Z = res == 0;
            // S
            // HC
            this.Registers.PC += 2;
        }
        private void DECea(U8Cmd cmd)
        {
            // this inst subtracts one from the EA register
            this.Registers.EA--;
            this.Registers.PSW.Z = this.Registers.EA == 0;
            // S
            this.Registers.PSW.OV = this.Registers.EA > ushort.MaxValue;
            // HC
            this.Registers.PC += 2;
        }
        private void DI(U8Cmd cmd)
        {
            // this inst sets the Master Interrupt Enable to 0 to disable maskable interrupts
            this.Registers.PSW.MIE = false;
            this.Registers.PC += 2;
        }
        private void DIVEReg(U8Cmd cmd)
        {
            // this inst divides the contents of the specified word-sized register by those of the
            // specified byte-sized register, stores the 16-bit divided in the former, and stores the
            // 8bit reminder in the latter
            this.Registers.PSW.C = this.Registers.GetRegisterByIndex((byte)cmd.Op2) == 0;
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            if (!this.Registers.PSW.C)
            {
                var div = (ushort)(this.Registers.GetERegisterByIndex((byte)cmd.Op1) / this.Registers.GetRegisterByIndex((byte)cmd.Op2)); //ERn = ERn / Rm
                var mod = (byte)(this.Registers.GetERegisterByIndex((byte)cmd.Op1) % this.Registers.GetRegisterByIndex((byte)cmd.Op2)); // Rm = ERn % Rm
                this.Registers.SetERegisterByIndex((byte)cmd.Op1, div);
                this.Registers.SetRegisterByIndex((byte)cmd.Op2, mod);
            }
            this.Registers.PC += 2;
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
                case U8Decoder.U8_BRK:
                    BRK(cmd);
                    break;
                case U8Decoder.U8_CMP_ER:
                    CMPERegEReg(cmd);
                    break;
                case U8Decoder.U8_CMP_R:
                    CMPRegReg(cmd);
                    break;
                case U8Decoder.U8_CMP_O:
                    CMPRegO(cmd);
                    break;
                case U8Decoder.U8_CMPC_O:
                    CMPCRegO(cmd);
                    break;
                case U8Decoder.U8_CMPC_R:
                    CMPCRegReg(cmd);
                    break;
                case U8Decoder.U8_CPLC:
                    CPLC(cmd);
                    break;
                // case U8Decoder.U8_DAA
                // case U8Decoder.U8_DAS
                case U8Decoder.U8_DEC_EA:
                    DECea(cmd);
                    break;
                case U8Decoder.U8_DI:
                    DI(cmd);
                    break;
                case U8Decoder.U8_DIV_ER:
                    DIVEReg(cmd);
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
