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

        private Dictionary<byte, Action<U8Cmd>> HandlerTable;

        public U8Emulator()
        {
            // mini emulating
            this.Registers = new U8Registers();
        }

        public U8Emulator(byte[] ROM)
        {
            // full emulator (for mah bucket list ;))
            this.Registers = new U8Registers();
            byte segCount = 0x0F; // 4 bit counter?
            this.Memory = new byte[segCount * 0x10000];
            this.ROM = ROM;
        }


        public void Initialise()
        {
            this.HandlerTable = new Dictionary<byte, Action<U8Cmd>>
            {
                #region ActionTable
                { U8Decoder.U8_ADD_R, (cmd) => {AddRegReg(cmd);} },
                // TODO: complete list
                #endregion
            };
        }

        public void Execute(U8Cmd cmd)
        {
            if (this.HandlerTable.ContainsKey((byte)cmd.Type))
                this.HandlerTable[(byte)cmd.Type].Invoke(cmd);
            else
                throw new Exception("opcode not yet supported");
        }

        // #===============#
        // #    Handlers   #
        // #===============#

        #region Handlers
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

            if (this.Registers.LR == 0)
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
            if (this.Registers.PSW.ELEVEL > 1)
            {
                // CPU system reset
                this.Registers.Initialise();
                if (this.Memory != null)
                {
                    this.Registers.SP = BitConverter.ToUInt16(this.Memory, 0);
                    this.Registers.SP = BitConverter.ToUInt16(this.Memory, 2);
                }
            }
            else if (this.Registers.PSW.ELEVEL < 2)
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
            int res = this.Registers.EA - 1;
            this.Registers.EA = (byte)res;
            this.Registers.PSW.Z = this.Registers.EA == 0;
            // S
            this.Registers.PSW.OV = (byte)(res + 1) == 0;
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
        private void EI(U8Cmd cmd)
        {
            // this inst sets the Master Interrupt Enable to 1 to enable maskable interrupts
            this.Registers.PSW.MIE = true;
            this.Registers.PC += 2;
        }
        private void EXTBWEreg(U8Cmd cmd)
        {
            // this instruction extends the content of the Rn register to signed 16-bit format and stores
            // it in the ERn register
            // The contents of the Rn+1 are filled with bit 7 of the Rn register, as the result

            // TODO: look into again

            this.Registers.PC += 2;
        }
        private void INCea(U8Cmd cmd)
        {
            // this inst adds one to the byte at the address in the EA register
            int res = this.Registers.EA + 1; // byte?
            this.Registers.EA = (ushort)res;
            this.Registers.PSW.Z = this.Registers.EA == 0;
            // S
            this.Registers.PSW.OV = res > ushort.MaxValue;
            // HC
            this.Registers.PC += 2;
        }
        private void LERegEA(U8Cmd cmd)
        {
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.EA);
            this.Registers.PSW.Z = this.Registers.GetERegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LERegEAP(U8Cmd cmd)
        {
            LERegEA(cmd);
            this.Registers.EA += 1;
        }
        private void LERegEReg(U8Cmd cmd)
        {
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.GetERegisterByIndex((byte)cmd.Op2));
            this.Registers.PSW.Z = this.Registers.GetERegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LERegD16EReg(U8Cmd cmd)
        {
            // TODO
            // S
            this.Registers.PC += 4;
        }
        private void LERegD6BP(U8Cmd cmd)
        {
            //TODO
            // S
            this.Registers.PC += 2;
        }
        private void LERegD6FP(U8Cmd cmd)
        {
            //TODO
            // S
            this.Registers.PC += 2;
        }
        private void LERegDA(U8Cmd cmd)
        {
            //TODO
            // S
            this.Registers.PC += 4;
        }
        private void LRegEA(U8Cmd cmd)
        {
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)this.Registers.EA);
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LRegEAP(U8Cmd cmd)
        {
            LRegEA(cmd);
            this.Registers.EA++;
        }
        private void LRegER(U8Cmd cmd)
        {
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.GetRegisterByIndex((byte)cmd.Op2));
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LRegD16EReg(U8Cmd cmd)
        {
            // TODO
            this.Registers.PC += 4;
        }
        private void LRegD6BP(U8Cmd cmd)
        {
            // TODO
            this.Registers.PC += 2;
        }
        private void LRegD6FP(U8Cmd cmd)
        {
            // TODO
            this.Registers.PC += 2;
        }
        private void LRegDA(U8Cmd cmd)
        {
            // TODO
            this.Registers.PC += 4;
        }
        private void LXRegEA(U8Cmd cmd)
        {
            this.Registers.SetXRegisterByIndex((byte)cmd.Op1, this.Registers.EA);
            this.Registers.PSW.Z = this.Registers.GetXRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LXRegEAP(U8Cmd cmd)
        {
            LXRegEA(cmd);
            this.Registers.EA++;
        }
        private void LQRegEA(U8Cmd cmd)
        {
            this.Registers.SetQRegisterByIndex((byte)cmd.Op1, this.Registers.EA);
            this.Registers.PSW.Z = this.Registers.GetQRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        private void LQRegEAP(U8Cmd cmd)
        {
            LQRegEA(cmd);
            this.Registers.EA++;
        }
        private void LEAEReg(U8Cmd cmd)
        {
            // this inst loads the EA register with a specified word value
            this.Registers.EA = this.Registers.GetERegisterByIndex((byte)cmd.Op1);
            this.Registers.PC += 2;
        }
        private void LEAD16(U8Cmd cmd)
        {
            // this inst loads the EA register with a specified word value
            this.Registers.EA = cmd.sWord;
            this.Registers.PC += 4;
        }
        private void LEAAddr(U8Cmd cmd)
        {
            // this inst loads the EA register with a specified word value
            // TODO: figure out Disp16[ERm]
            //this.Registers.EA = (0x10000) + cmd.sWord;
            this.Registers.PC += 4;
        }
        private void MOVCRReg(U8Cmd cmd)
        {
            //TODO: CR?
            this.Registers.PC += 2;
        }
        private void MOVCREA(U8Cmd cmd)
        {
            //TODO: CR?
            this.Registers.PC += 2;
        }
        private void MOVCREAP(U8Cmd cmd)
        {
            //TODO: CR?
            MOVCREA(cmd);
            this.Registers.EA++;
        }
        private void MOVCEREA(U8Cmd cmd)
        {
            //TODO: CER?
            // this instr loads t he specified coprocessor word-sized register from
            // the specified word address
            //this.Registers.
            this.Registers.PC += 2;
        }
        private void MOVCEREAP(U8Cmd cmd)
        {
            //TODO: CER?
            MOVCEREA(cmd);
            this.Registers.EA++;
        }
        private void MOVCXREA(U8Cmd cmd)
        {

        }
        private void MOVCXREAP(U8Cmd cmd)
        {

        }
        private void MOVCQREA(U8Cmd cmd)
        {

        }
        private void MOVCQREAP(U8Cmd cmd)
        {

        }
        private void MOVRegCR(U8Cmd cmd)
        {

        }
        private void MOVEACER(U8Cmd cmd)
        {

        }
        private void MOVEAPCER(U8Cmd cmd)
        {

        }
        private void MOVEACR(U8Cmd cmd)
        {

        }
        private void MOVEAPCR(U8Cmd cmd)
        {

        }
        private void MOVEACXR(U8Cmd cmd)
        {

        }
        private void MOVEAPCXR(U8Cmd cmd)
        {

        }
        private void MOVEACQR(U8Cmd cmd)
        {

        }
        private void MOVEAPCQR(U8Cmd cmd)
        {

        }
        private void MOVECECSRReg(U8Cmd cmd)
        {
            // this inst loads the contents of the specified register int othe local code segment
            // register LCSR if ELEVEL is zero and into the EXSR register (ECSR1 to ECSR3) for
            // the current exception level (ELEVEL) setting otherwise
            if (this.Registers.PSW.ELEVEL == 0)
                this.Registers.LCSR = this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            else
                this.Registers.SetECSRByIndex(this.Registers.PSW.ELEVEL, this.Registers.GetRegisterByIndex((byte)cmd.Op2)); // NOTE: 1 or 2?
            this.Registers.PC += 2;
        }
        private void MOVECECSREReg(U8Cmd cmd)
        {
            // this inst loads the contents of the specified word-sized register int othe local code segment
            // register LR if ELEVEL is zero and into the exception link register (ELR1 to ELR3) for
            // the current exception level (ELEVEL) setting otherwise
            if (this.Registers.PSW.ELEVEL == 0)
                this.Registers.LCSR = this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            else
                this.Registers.SetECSRByIndex(this.Registers.PSW.ELEVEL, this.Registers.GetRegisterByIndex((byte)cmd.Op2)); // NOTE: 1 or 2?

            this.Registers.PC += 2;
        }
        private void MOVELREReg(U8Cmd cmd)
        {
            // this inst loads the contents of the specified word-sized register into the link
            // register LR if ELEVEL is zero and into the exception link register (ELR1~ELR3) for
            // the current exception level (ELEBEL) settings otherwise
            if (this.Registers.PSW.ELEVEL == 0)
                this.Registers.LR = this.Registers.GetERegisterByIndex((byte)cmd.Op2);
            else
                this.Registers.SetELRByIndex(this.Registers.PSW.ELEVEL, this.Registers.GetERegisterByIndex((byte)cmd.Op2)); // NOTE: 1 or 2?

            this.Registers.PC += 2;
        }

        private void MOVEPSWReg(U8Cmd cmd)
        {
            // this inst loads the content of the specified register into the exception program
            // status word (EPSW1~EPSW3) register for the current exception level (ELEVEL)
            // setting if ELBEL is nonzero
            // if ELEVEL is zero, the inst does nothing
            if (this.Registers.PSW.ELEVEL != 0)
                this.Registers.SetEPSWByIndex(this.Registers.PSW.ELEVEL, this.Registers.GetRegisterByIndex((byte)cmd.Op2));

            this.Registers.PC += 2;
        }

        private void MOVERegELR(U8Cmd cmd)
        {
            // this inst loads the specified word-sized register from the link register LR if
            // ELEVEL is zero and from the excpetion link register (ELR1 to ELR3) for the current
            // exception level (ELEBEL) setting otherwise
            if (this.Registers.PSW.ELEVEL == 0)
                this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.LR);
            else
                this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.GetELRByIndex((byte)cmd.Op2));

            this.Registers.PC += 2;
        }

        private void MOVEregEreg(U8Cmd cmd)
        {
            // this inst loads the first word-sized register from the second
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.GetERegisterByIndex((byte)cmd.Op2));
            this.Registers.PSW.Z = this.Registers.GetERegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }

        private void MOVERegO(U8Cmd cmd)
        {
            // this inst loads the sign-extended imm7 into the specified word-sized register
            // More precisely, it loads the immediate value into Rn, the lower half of the register, and
            // copies bit 6 from the immediate value in Rn bit 7 amd all bits of Rn+1

            // TODO
        }

        private void MOVERegSP(U8Cmd cmd)
        {
            // this instr saves the contents of the stack pointer (SP) in the specified word-sized register
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, this.Registers.SP);
            this.Registers.PC += 2;
        }

        private void MOVOCER(U8Cmd cmd)
        {
            // this inst saves the contents of the specified coprocessor word-sized register at
            // the specified word address in the EA register

            // TODO
        }

        private void MOVOCQR(U8Cmd cmd)
        {
            // this instr saves the contents of the specified coprocessor squad word-sized register
            // at the specified word address in the EA register

            // TODO
        }

        private void MOVOCR(U8Cmd cmd)
        {
            // this inst saves the contents of the specified coprocessor byte-sized register at the
            // specified byte address in the EA register

            // TODO
        }

        private void MOVOCXR(U8Cmd cmd)
        {
            // this inst saves the contents of the specified coprocessor double word-sized
            // register at the specified word address in the EA register

            // TODO
        }

        private void MOVPSWO(U8Cmd cmd)
        {
            // this inst loads the program status word (PSW) from the specified byte-sized obj
            // NOTE: place NOP right after to fix cycle delay
            this.Registers.PSW.Set((byte)cmd.Op2);// unsigned8
            this.Registers.PC += 2;
        }

        private void MOVPSWReg(U8Cmd cmd)
        {
            // this inst loads the program status word (PSW) from the specified byte-sized obj
            // NOTE: place NOP right after to fix cycle delay
            this.Registers.PSW.Set(this.Registers.GetRegisterByIndex((byte)cmd.Op2));
            this.Registers.PC += 2;
        }

        //private void MOVRegCR(U8Cmd cmd)
        //{
        //    // this inst loads the specified byte-sized register from the specified coprocessor
        //    // byte-size register
        //    // TODO: this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.)
        //    this.Registers.PC += 2;
        //}

        private void MOVRegECSR(U8Cmd cmd)
        {
            // this inst loads the specified byte-sized register from the local code segment
            // register (LCSR) if ELEVEL is zero and from the ECSR register (ECSR1 to ECSR3) for
            // the current exception level (ELEVEL) setting otherwise
            if (this.Registers.PSW.ELEVEL == 0)
                this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.LCSR);
            else
                this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.GetECSRByIndex((byte)cmd.Op2));
            this.Registers.PC += 2;
        }

        private void MOVRegEPSW(U8Cmd cmd)
        {
            // this instr loads the specified byte-sized register from the exception program
            // status word (EPSW1 to EPSW3) register for the current exception level (ELEVEL)
            // setting if ELEVEL is nonzero
            if (this.Registers.PSW.ELEVEL != 0)
            {
                this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)this.Registers.GetEPSWByIndex(this.Registers.PSW.ELEVEL));
            }
            this.Registers.PC += 2;
        }

        private void MOVRegPSW(U8Cmd cmd)
        {
            // this instr loads the specified byte-sized register from the program status word (PSW)
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.PSW.Get());
            this.Registers.PC += 2;
        }

        private void MOVRegReg(U8Cmd cmd)
        {
            // this inst loads the specified byte-sized register from the specified byte-sized register
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, this.Registers.GetRegisterByIndex((byte)cmd.Op2));
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }

        private void MOVRegO(U8Cmd cmd)
        {
            // this inst loads the specified byte-sized register from the specified byte-sized imm8
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)cmd.Op2); // #imm8?
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }

        private void MOVSPEReg(U8Cmd cmd)
        {
            // this instr loads the stack pointer (SP_) from the specified word-sized register
            this.Registers.SP = this.Registers.GetERegisterByIndex((byte)cmd.Op2);
            this.Registers.PC += 2;
        }

        private void MULERegReg(U8Cmd cmd)
        {
            // this instr multiplies the contents of the two specified byte-size registers and stores
            // the 16-bit product in the word-zied register corresponding to the first register
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) * this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            this.Registers.SetERegisterByIndex((byte)cmd.Op1, (ushort)res);
            this.Registers.PSW.Z = res == 0;
            this.Registers.PC += 2;
        }

        private void NEGReg(U8Cmd cmd)
        {
            // this inst calculate the two complement of the contents of the specified
            // byte-size register and stores the result in the register
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)(0 - this.Registers.GetRegisterByIndex((byte)cmd.Op1)));
            // C
            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PSW.OV = false; // can this even overflow?
            // HC
            this.Registers.PC += 2;
        }

        private void NOP(U8Cmd cmd)
        {
            // No operation
            this.Registers.PC += 2;
        }

        private void ORRegReg(U8Cmd cmd)
        {
            // this instr ORs the contents of the specified byte-sized register and object and stores the result
            // in the register
            var res = this.Registers.GetRegisterByIndex((byte)cmd.Op1) | this.Registers.GetRegisterByIndex((byte)cmd.Op2);
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)res);
            this.Registers.PSW.Z = res == 0;
            // S
            this.Registers.PC += 2;
        }
        private void ORRegO(U8Cmd cmd)
        {
            // this instr ORs the contents of the specified byte-sized register and object and stores the result
            // in the register
            this.Registers.SetRegisterByIndex((byte)cmd.Op1, (byte)cmd.Op2);
            this.Registers.PSW.Z = (byte)cmd.Op2 == 0;
            // S
            this.Registers.PC += 2;
        }

        // TODO: POP & PUSH

        private void RBDBitaddr(U8Cmd cmd)
        {
            // TODO
        }

        private void RBRegBit(U8Cmd cmd)
        {
            // TODO
        }

        private void RC(U8Cmd cmd)
        {
            // this instr resets the carry flag to 0
            this.Registers.PSW.C = false;
            this.Registers.PC += 2;
        }

        private void RT(U8Cmd cmd)
        {
            // this instr is for returning from a subroutine called with a BL instr.
            // it restores the address of the instruction the BL instruction by loading the
            // code segment register from the local code segment (LCSR) and the program
            // counter (PC) from the link register (LR)
            this.Registers.CSR = this.Registers.LCSR;
            this.Registers.PC = this.Registers.LR;
            this.Registers.PC += 2;
        }

        private void RTI(U8Cmd cmd)
        {
            // this inst is for returning from an interrupt handler
            // it restores the program status word (PSW) and the program counter (PC) from
            // the exception program status word (EPSW1 to EPSW3) register and exception link
            // register (ELR1 ~ ELR3), respectively. For the current exception level (ELEVEL) setting
            // 1 for maskable interrupt and 2 for non-maskable ones
            this.Registers.CSR = this.Registers.GetECSRByIndex(this.Registers.PSW.ELEVEL);
            this.Registers.PC = this.Registers.GetELRByIndex(this.Registers.PSW.ELEVEL);
            this.Registers.PSW.Set((byte)this.Registers.GetEPSWByIndex(this.Registers.PSW.ELEVEL)); // TODO: verify this?
            this.Registers.PC += 2;
        }

        private void SBDbit(U8Cmd cmd)
        {
            // this instr test the specified bit by reading it form memory, invesing it, and
            // storing the result in the Z flag, it then sets the original bit tp 1
            // this bit address DBitadr has the format Badr16bit where bit is an integer between 0 and
            // 7 specifying the bit position within the memory byte

            // TODO
        }

        private void SBRegBit(U8Cmd cmd)
        {
            // this instr reads the specified bbit from the specified byte-sized register, inverts it,
            // and stores it in the Z flag, it then sets the original bit to 1
            // bit_ffset is an integer between 0 and 7 specifying the bit position within the register
        }

        private void SC(U8Cmd cmd)
        {
            // this instr sets the carry flag to 1
            this.Registers.PSW.C = true;
            this.Registers.PC += 2;
        }

        private void SLLRegReg(U8Cmd cmd)
        {
            // todo
        }
        private void SLLRegO(U8Cmd cmd)
        {
            // todo
        }

        private void SLLCRegReg(U8Cmd cmd)
        {
            // TODO
        }
        private void SLLCRegO(U8Cmd cmd)
        {
            // TODO
        }

        private void SRARegReg(U8Cmd cmd)
        {
            // TODO
        }
        private void SRARegO(U8Cmd cmd)
        {
            // TODO
        }

        private void SRLRegReg(U8Cmd cmd)
        {
            // TODO
        }

        private void SRLRegI(U8Cmd cmd)
        {
            // TODO
        }

        private void SRLCRegReg(U8Cmd cmd)
        {
            // TODO
        }
        private void SRLCRegO(U8Cmd cmd)
        {
            // TODO
        }

        // TODO ST's

        private void SUBRegReg(U8Cmd cmd)
        {
            // this instr subtracts the contents of the second byte-sized register from those of the
            // first and stores the result in the first
        }

        private void SUBCRegReg(U8Cmd cmd)
        {
            // this instr subtracts the contents of the second byte-sized register and the carry
            // flag from the contents of the first register and stores the result in the first register

        }

        private void SWI(U8Cmd cmd)
        {
            // this instr loads the specified vector table entry into the program counter PC the
            // operand is an integer between 0 and 63, during the interrupt cycle, the instruction also
            // saves the addres of the next instr in the ELR1 register
            this.Registers.EPSW1 = this.Registers.PSW.Get();
            this.Registers.PSW.ELEVEL = 1;
            this.Registers.ELR1 = this.Registers.PC += 2;
            this.Registers.ECSR1 = this.Registers.CSR;
            this.Registers.PSW.MIE = false;
            // TODO: this.Registers.PC = TABLE[(byte)cmd.Op1<<1]
        }

        private void TBDbit(U8Cmd cmd)
        {
            // this instr test the specified bit by reading the from memory, inverting it, and
            // storing the result in the Z flag
            // the bit address Dbitaddr has the format Dadr16.bit, where bit is an integer betweem 0 and
            // 7 specifying the bit position within the memory byte

            // TODO: Z ~[Dbit]
        }

        private void TBReg(U8Cmd cmd)
        {
            // this instr test the specified bit by reading it from memory, inverting it, and
            // storing the result in the Z flag
            // bit_offset us an integer between 0 and 7 specifying the bit position within the register
            // TODO: Z = ~Rn[bit_offset]
        }

        private void XORReg(U8Cmd cmd)
        {
            // this insts XORs the contents of the specified byte-sized register and object and
            // stores the result in the register
            this.Registers.SetRegisterByIndex((byte)cmd.Op1,
                (byte)(this.Registers.GetRegisterByIndex((byte)cmd.Op1) ^ this.Registers.GetRegisterByIndex((byte)cmd.Op2)));

            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }

        private void XORO(U8Cmd cmd)
        {
            // this insts XORs the contents of the specified byte-sized register and object and
            // stores the result in the register
            this.Registers.SetRegisterByIndex((byte)cmd.Op1,
                (byte)(this.Registers.GetRegisterByIndex((byte)cmd.Op1) ^ (byte)cmd.Op2));

            this.Registers.PSW.Z = this.Registers.GetRegisterByIndex((byte)cmd.Op1) == 0;
            // S
            this.Registers.PC += 2;
        }
        #endregion Handlers

    }
}
