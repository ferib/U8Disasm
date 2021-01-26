using System;
using U8Disasm.Structs;

namespace U8Disasm.Disassembler
{
    public static class U8Decoder
    {
        // TODO: Make Enum?
        #region ASMDefinitions
        // define u8 instructions;
        public const byte U8_INS_NUM = 159;

        // Arithmetic instructions;
        public const byte U8_ADD_R = 0;
        public const byte U8_ADD_O = 1;
        public const byte U8_ADD_ER = 2;
        public const byte U8_ADD_ER_O = 3;
        public const byte U8_ADDC_R = 4;
        public const byte U8_ADDC_O = 5;
        public const byte U8_AND_R = 6;
        public const byte U8_AND_O = 7;
        public const byte U8_CMP_R = 8;
        public const byte U8_CMP_O = 9;
        public const byte U8_CMPC_R = 10;
        public const byte U8_CMPC_O = 11;
        public const byte U8_MOV_ER = 12;
        public const byte U8_MOV_ER_O = 13;
        public const byte U8_MOV_R = 14;
        public const byte U8_MOV_O = 15;
        public const byte U8_OR_R = 16;
        public const byte U8_OR_O = 17;
        public const byte U8_XOR_R = 18;
        public const byte U8_XOR_O = 19;
        public const byte U8_CMP_ER = 20;
        public const byte U8_SUB_R = 21;
        public const byte U8_SUBC_R = 22;

        // Shift instructions;
        public const byte U8_SLL_R = 23;
        public const byte U8_SLL_O = 24;
        public const byte U8_SLLC_R = 25;
        public const byte U8_SLLC_O = 26;
        public const byte U8_SRA_R = 27;
        public const byte U8_SRA_O = 28;
        public const byte U8_SRL_R = 29;
        public const byte U8_SRL_O = 30;
        public const byte U8_SRLC_R = 31;
        public const byte U8_SRLC_O = 32;

        // Load/store instructions;
        public const byte U8_L_ER_EA = 33;
        public const byte U8_L_ER_EAP = 34;
        public const byte U8_L_ER_ER = 35;
        public const byte U8_L_ER_D16_ER = 36;
        public const byte U8_L_ER_D6_BP = 37;
        public const byte U8_L_ER_D6_FP = 38;
        public const byte U8_L_ER_DA = 39;
        public const byte U8_L_R_EA = 40;
        public const byte U8_L_R_EAP = 41;
        public const byte U8_L_R_ER = 42;
        public const byte U8_L_R_D16_ER = 43;
        public const byte U8_L_R_D6_BP = 44;
        public const byte U8_L_R_D6_FP = 45;
        public const byte U8_L_R_DA = 46;
        public const byte U8_L_XR_EA = 47;
        public const byte U8_L_XR_EAP = 48;
        public const byte U8_L_QR_EA = 49;
        public const byte U8_L_QR_EAP = 50;
        public const byte U8_ST_ER_EA = 51;
        public const byte U8_ST_ER_EAP = 52;
        public const byte U8_ST_ER_ER = 53;
        public const byte U8_ST_ER_D16_ER = 54;
        public const byte U8_ST_ER_D6_BP = 55;
        public const byte U8_ST_ER_D6_FP = 56;
        public const byte U8_ST_ER_DA = 57;
        public const byte U8_ST_R_EA = 58;
        public const byte U8_ST_R_EAP = 59;
        public const byte U8_ST_R_ER = 60;
        public const byte U8_ST_R_D16_ER = 61;
        public const byte U8_ST_R_D6_BP = 62;
        public const byte U8_ST_R_D6_FP = 63;
        public const byte U8_ST_R_DA = 64;
        public const byte U8_ST_XR_EA = 65;
        public const byte U8_ST_XR_EAP = 66;
        public const byte U8_ST_QR_EA = 67;
        public const byte U8_ST_QR_EAP = 68;

        // Control register access instructions;
        public const byte U8_ADD_SP_O = 69;
        public const byte U8_MOV_ECSR_R = 70;
        public const byte U8_MOV_ELR_ER = 71;
        public const byte U8_MOV_EPSW_R = 72;
        public const byte U8_MOV_ER_ELR = 73;
        public const byte U8_MOV_ER_SP = 74;
        public const byte U8_MOV_PSW_R = 75;
        public const byte U8_MOV_PSW_O = 76;
        public const byte U8_MOV_R_ECSR = 77;
        public const byte U8_MOV_R_EPSW = 78;
        public const byte U8_MOV_R_PSW = 79;
        public const byte U8_MOV_SP_ER = 80;

        // Push/pop instructions;
        public const byte U8_PUSH_ER = 81;
        public const byte U8_PUSH_QR = 82;
        public const byte U8_PUSH_R = 83;
        public const byte U8_PUSH_XR = 84;
        public const byte U8_PUSH_RL = 85;
        public const byte U8_POP_ER = 86;
        public const byte U8_POP_QR = 87;
        public const byte U8_POP_R = 88;
        public const byte U8_POP_XR = 89;
        public const byte U8_POP_RL = 90;

        // Coprocessor data transfer instructions;
        public const byte U8_MOV_CR_R = 91;
        public const byte U8_MOV_CER_EA = 92;
        public const byte U8_MOV_CER_EAP = 93;
        public const byte U8_MOV_CR_EA = 94;
        public const byte U8_MOV_CR_EAP = 95;
        public const byte U8_MOV_CXR_EA = 96;
        public const byte U8_MOV_CXR_EAP = 97;
        public const byte U8_MOV_CQR_EA = 98;
        public const byte U8_MOV_CQR_EAP = 99;
        public const byte U8_MOV_R_CR = 100;
        public const byte U8_MOV_EA_CER = 101;
        public const byte U8_MOV_EAP_CER = 102;
        public const byte U8_MOV_EA_CR = 103;
        public const byte U8_MOV_EAP_CR = 104;
        public const byte U8_MOV_EA_CXR = 105;
        public const byte U8_MOV_EAP_CXR = 106;
        public const byte U8_MOV_EA_CQR = 107;
        public const byte U8_MOV_EAP_CQR = 108;

        // EA register data transfer instructions;
        public const byte U8_LEA_ER = 109;
        public const byte U8_LEA_D16_ER = 110;
        public const byte U8_LEA_DA = 111;

        // ALU Instructions;
        public const byte U8_DAA_R = 112;
        public const byte U8_DAS_R = 113;
        public const byte U8_NEG_R = 114;

        // Bit access instructions;
        public const byte U8_SB_R = 115;
        public const byte U8_SB_DBIT = 116;
        public const byte U8_RB_R = 117;
        public const byte U8_RB_DBIT = 118;
        public const byte U8_TB_R = 119;
        public const byte U8_TB_DBIT = 120;

        // PSW access instructions;
        public const byte U8_EI = 121;
        public const byte U8_DI = 122;
        public const byte U8_SC = 123;
        public const byte U8_RC = 124;
        public const byte U8_CPLC = 125;

        // Conditional relative branch instructions;
        public const byte U8_BGE_RAD = 126;
        public const byte U8_BLT_RAD = 127;
        public const byte U8_BGT_RAD = 128;
        public const byte U8_BLE_RAD = 129;
        public const byte U8_BGES_RAD = 130;
        public const byte U8_BLTS_RAD = 131;
        public const byte U8_BGTS_RAD = 132;
        public const byte U8_BLES_RAD = 133;
        public const byte U8_BNE_RAD = 134;
        public const byte U8_BEQ_RAD = 135;
        public const byte U8_BNV_RAD = 136;
        public const byte U8_BOV_RAD = 137;
        public const byte U8_BPS_RAD = 138;
        public const byte U8_BNS_RAD = 139;
        public const byte U8_BAL_RAD = 140;

        // Sign extension instruction;
        public const byte U8_EXTBW_ER = 141;

        // Software interrupt instructions;
        public const byte U8_SWI_O = 142;
        public const byte U8_BRK = 143;

        // Branch instructions;
        public const byte U8_B_AD = 144;
        public const byte U8_B_ER = 145;
        public const byte U8_BL_AD = 146;
        public const byte U8_BL_ER = 147;

        // Multiplication and division instructions;
        public const byte U8_MUL_ER = 148;
        public const byte U8_DIV_ER = 149;

        // Miscellaneous;
        public const byte U8_INC_EA = 150;
        public const byte U8_DEC_EA = 151;
        public const byte U8_RT = 152;
        public const byte U8_RTI = 153;
        public const byte U8_NOP = 154;

        // DSR prefix for load/store;
        public const byte U8_PRE_PSEG = 155;
        public const byte U8_PRE_DSR = 156;
        public const byte U8_PRE_R = 157;

        // Undefined;
        public const byte U8_ILL = 158;
        #endregion ASMDefinitions

        public static int DecodeInstruction(UInt16 Opcode)
        {
            UInt16 t;
            int i;

            // iterate through master table of instructions, returning on match
            // FIXME: this can be done more efficiently
            for (i = 0; i < U8_INS_NUM; i++)
            {
                if ((Opcode & U8Instructions.Table[i].InsMask) == U8Instructions.Table[i].Ins)
                    return i;
            }

            // err
            return U8_ILL;
        }

        public static UInt16 DecodeOperand(UInt16 Instruction, UInt16 Mask)
        {
            UInt16 n;

            switch (Mask)
            {
                case 0x0800:
                    n = 11;
                    break;
                case 0x0c00:
                    n = 10;
                    break;
                case 0x0e00:
                    n = 9;
                    break;
                case 0x0f00:
                case 0x0700:
                    n = 8;
                    break;
                case 0x00e0:
                    n = 5;
                    break;
                case 0x00f0:
                case 0x0070:
                    n = 4;
                    break;
                case 0x00ff:
                case 0x007f:
                case 0x003f:
                default:
                    n = 0;
                    break;
            }

            // bitwise magic to extract value
            return (UInt16)((Instruction & Mask) >> n);
        }

        // get opcode string
        public static int DecodeOpcode(byte[] Buf, ref U8Cmd Cmd)
        {
            int i = 0, Addr = 0;

            UInt16 Inst, sWord = 0, Prefix = 0;
            UInt16 Op1 = 0, Op2 = 0;

            UInt16 PrePSEG = 0, PreDSR = 0, PreR = 0;
            string PrefixStr = "";

            if (Buf.Length < 2)    // words are always 2 bytes bro
                return -1;

            Inst = BitConverter.ToUInt16(Buf, 0);
            i++;    // word read counter

            Cmd.Type = DecodeInstruction(Inst);

            // check for prefix
            switch (Cmd.Type)
            {
                case U8_PRE_PSEG:
                    PrePSEG = DecodeOperand(Inst, U8Instructions.Table[Cmd.Type].Op1Mask);
                    PrefixStr = $"{PrePSEG.ToString("X2")}:";
                    Prefix = Inst;
                    break;
                case U8_PRE_DSR:
                    PrefixStr = $"dsr:";
                    PreDSR = 1;
                    Prefix = Inst;
                    break;
                case U8_PRE_R:
                    PreR = DecodeOperand(Inst, U8Instructions.Table[Cmd.Type].Op1Mask);
                    PrefixStr = $"r{PreR.ToString()}:";
                    Prefix = Inst;
                    break;
            }

            if (Prefix != 0)
            {
                if (Buf.Length < (i + 1) * 2)
                    return -1;

                Inst = BitConverter.ToUInt16(Buf, i * 2); // read the first real world after prefix
                i++;

                Cmd.Type = DecodeInstruction(Inst);
            }

            if (U8Instructions.Table[Cmd.Type].Length == 2)
            {
                if (Buf.Length < (i + 1) * 2)
                    return -1;

                sWord = BitConverter.ToUInt16(Buf, i * 2); // read second/third word from stream
                Cmd.sWord = sWord;
                i++;
            }

            // set mnemonic
            Cmd.Instruction = U8Instructions.Table[Cmd.Type].Name;

            // get op1
            if (U8Instructions.Table[Cmd.Type].Operands == 1)
            {
                Op1 = DecodeOperand(Inst, U8Instructions.Table[Cmd.Type].Op1Mask);
                Cmd.Op1 = Op1;
            }
            if (U8Instructions.Table[Cmd.Type].Operands == 2) // or get op1 and op2
            {
                Op1 = DecodeOperand(Inst, U8Instructions.Table[Cmd.Type].Op1Mask);
                Op2 = DecodeOperand(Inst, U8Instructions.Table[Cmd.Type].Op2Mask);
                Cmd.Op1 = Op1;
                Cmd.Op2 = Op2;
            }

            // format operands TODO: These may be buggy, I dont have any resource to confirm the opcodes
            switch (Cmd.Type)
            {
                // 8-bit register instructions
                case U8_ADD_R:
                case U8_AND_R:
                case U8_ADDC_R:
                case U8_CMP_R:
                case U8_CMPC_R:
                case U8_MOV_R:
                case U8_OR_R:
                case U8_XOR_R:
                case U8_SUB_R:
                case U8_SUBC_R:
                case U8_SLL_R:
                case U8_SLLC_R:
                case U8_SRA_R:
                case U8_SRL_R:
                case U8_SRLC_R:
                    Cmd.Operands = $"r{Op1} r{Op2}";
                    //fmt_op_str("r%d, r%d", op1, op2);
                    break;

                // 8-bit register/object instructions
                case U8_ADD_O:
                case U8_AND_O:
                case U8_ADDC_O:
                case U8_CMP_O:
                case U8_CMPC_O:
                case U8_MOV_O:
                case U8_OR_O:
                case U8_XOR_O:
                case U8_SLL_O:
                case U8_SLLC_O:
                case U8_SRA_O:
                case U8_SRL_O:
                case U8_SRLC_O:
                    Cmd.Operands = $"r{Op1} #{Op2.ToString("X")}";
                    //fmt_op_str("r%d, #%xh", op1, op2);
                    break;

                // 16-bit extended register instructions
                case U8_ADD_ER:
                case U8_MOV_ER:
                case U8_CMP_ER:
                    Cmd.Operands = $"er{Op1} er{Op2}"; // op1[11:8], op2[7:4] bitwise?
                                                       //fmt_op_str("er%d, er%d", op1, op2);
                    break;

                // Extended register/object instructions #imm7
                case U8_ADD_ER_O:
                case U8_MOV_ER_O:
                    if (isNegative7Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, #-{ABS7Bit((byte)Op2)}h";
                    //fmt_op_str("er%d, #-%xh", op1, abs_7bit((byte)op2));
                    else
                        Cmd.Operands = $"er{Op1}, #{ABS7Bit((byte)Op2)}h"; // TODO: look innto 0's
                                                                           //fmt_op_str("er%d, #%xh", op1, abs_7bit((byte)op2));
                    break;

                // Extended register load/store instructions
                case U8_L_ER_EA:
                case U8_ST_ER_EA:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea]";
                    //fmt_op_str("er%d, %s[ea]", op1, prefix_str);
                    break;
                case U8_L_ER_EAP:
                case U8_ST_ER_EAP:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea+]";
                    //fmt_op_str("er%d, %s[ea+]", op1, prefix_str);
                    break;
                case U8_L_ER_ER:
                case U8_ST_ER_ER:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea{Op2}]";
                    //fmt_op_str("er%d, %s[er%d]", op1, prefix_str, op2);
                    break;
                case U8_L_ER_D16_ER:
                case U8_ST_ER_D16_ER:
                    Cmd.Operands = $"er{Op1} {Op1.ToString("X4")}h{sWord}[ea{Op2}]";
                    //fmt_op_str("er%d, %04xh[er%d]", op1, s_word, op2);
                    break;
                case U8_L_ER_D6_BP:
                case U8_ST_ER_D6_BP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("er%d, -%xh[bp]", op1, abs_6bit(op2));
                    else
                        Cmd.Operands = $"er{Op1}, {ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("er%d, %xh[bp]", op1, abs_6bit(op2));
                    break;
                case U8_L_ER_D6_FP:
                case U8_ST_ER_D6_FP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("er%d, -%xh[fp]", op1, abs_6bit(op2));
                    else
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("er%d, %xh[fp]", op1, abs_6bit(op2));
                    break;
                case U8_L_ER_DA:
                case U8_ST_ER_DA:
                    //fmt_op_str("er%d, %04xh", op1, s_word);
                    Cmd.Operands = $"er{Op1}, {sWord.ToString("X4")}h";
                    break;

                // Register load/store instructions
                case U8_L_R_EA:
                case U8_ST_R_EA:
                    //fmt_op_str("r%d, [ea]", op1);
                    Cmd.Operands = $"r{Op1}, [ea]";
                    break;
                case U8_L_R_EAP:
                case U8_ST_R_EAP:
                    //fmt_op_str("r%d, [ea+]", op1);
                    Cmd.Operands = $"r{Op1}, [ea+]";
                    break;
                case U8_L_R_ER:
                case U8_ST_R_ER:
                    //fmt_op_str("r%d, [er%d]", op1, op2);
                    Cmd.Operands = $"r{Op1}, [er{Op2}]";
                    break;
                case U8_L_R_D16_ER:
                case U8_ST_R_D16_ER:
                    //fmt_op_str("r%d, %s%04xh[er%d]", op1, prefix_str, s_word, op2);
                    Cmd.Operands = $"r{Op1}, {PrefixStr}{sWord.ToString("X4")}h[er{Op2}]";
                    break;
                case U8_L_R_D6_BP:
                case U8_ST_R_D6_BP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"r{Op1}, {PrefixStr}-{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s-%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    else
                        Cmd.Operands = $"r{Op1}, {PrefixStr}{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    break;
                case U8_L_R_D6_FP:
                case U8_ST_R_D6_FP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"r{Op1}, {PrefixStr}-{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s-%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    else
                        Cmd.Operands = $"r{Op1}, {PrefixStr}{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    break;
                case U8_L_R_DA:
                case U8_ST_R_DA:
                    //fmt_op_str("r%d, %s%04xh", op1, prefix_str, s_word);
                    Cmd.Operands = $"r{Op1}, {PrefixStr}{sWord.ToString("X4")}h";
                    break;

                // Double/quad word register load/store instructions
                case U8_L_XR_EA:
                case U8_ST_XR_EA:
                    //fmt_op_str("xr%d, %s[ea]", op1, prefix_str);
                    Cmd.Operands = $"xr{Op1}, {PrefixStr}[ea]";
                    break;
                case U8_L_XR_EAP:
                case U8_ST_XR_EAP:
                    //fmt_op_str("xr%d, %s[ea+]", op1, prefix_str);
                    Cmd.Operands = $"xr{Op1}, {PrefixStr}[ea+]";
                    break;
                case U8_L_QR_EA:
                case U8_ST_QR_EA:
                    //fmt_op_str("qr%d, %s[ea]", op1, prefix_str);
                    Cmd.Operands = $"qr{Op1}, {PrefixStr}[ea]";
                    break;
                case U8_L_QR_EAP:
                case U8_ST_QR_EAP:
                    //fmt_op_str("qr%d, %s[ea+]", op1, prefix_str);
                    Cmd.Operands = $"qr{Op1}, {PrefixStr}[ea+]";
                    break;

                // Control register access instructions
                case U8_ADD_SP_O:
                    //fmt_op_str("sp, #%xh", op1);
                    Cmd.Operands = $"sp, #{Op1.ToString("X")}h";
                    break;
                case U8_MOV_ECSR_R:
                    //fmt_op_str("ecsr, r%d", op1);
                    Cmd.Operands = $"ecsr, r{Op1}";
                    break;
                case U8_MOV_ELR_ER:
                    //fmt_op_str("elr, er%d", op1);
                    Cmd.Operands = $"elr, er{Op1}";
                    break;
                case U8_MOV_EPSW_R:
                    //fmt_op_str("epsw, r%d", op1);
                    Cmd.Operands = $"epsw, r{Op1}";
                    break;
                case U8_MOV_ER_ELR:
                    //fmt_op_str("er%d, elr", op1);
                    Cmd.Operands = $"er{Op1}, elr";
                    break;
                case U8_MOV_ER_SP:
                    //fmt_op_str("er%d, sp", op1);
                    Cmd.Operands = $"er{Op1}, sp";
                    break;
                case U8_MOV_PSW_R:
                    //fmt_op_str("psw, r%d", op1);
                    Cmd.Operands = $"psw, r{Op1}";
                    break;
                case U8_MOV_PSW_O:
                    //fmt_op_str("psw, #%xh", op1);
                    Cmd.Operands = $"psw, #{Op1.ToString("X")}h";
                    break;
                case U8_MOV_R_ECSR:
                    //fmt_op_str("r%d, ecsr", op1);
                    Cmd.Operands = $"r{Op1}, ecsr";
                    break;
                case U8_MOV_R_EPSW:
                    //fmt_op_str("r%d, epsw", op1);
                    Cmd.Operands = $"r{Op1}, epsw";
                    break;
                case U8_MOV_R_PSW:
                    //fmt_op_str("r%d, psw", op1);
                    Cmd.Operands = $"r{Op1}, psw";
                    break;
                case U8_MOV_SP_ER:
                    //fmt_op_str("sp, er%d", op1);
                    Cmd.Operands = $"sp, er{Op1}";
                    break;

                // Push/pop instructions
                case U8_PUSH_ER:
                case U8_POP_ER:
                    //fmt_op_str("er%d", op1);
                    Cmd.Operands = $"er{Op1}";
                    break;
                case U8_PUSH_QR:
                case U8_POP_QR:
                    //fmt_op_str("qr%d", op1);
                    Cmd.Operands = $"qr{Op1}";
                    break;
                case U8_PUSH_R:
                case U8_POP_R:
                    //fmt_op_str("r%d", op1);
                    Cmd.Operands = $"r{Op1}";
                    break;
                case U8_PUSH_XR:
                case U8_POP_XR:
                    //fmt_op_str("xr%d", op1);
                    Cmd.Operands = $"xr{Op1}";
                    break;

                // Register list stack instructions
                case U8_PUSH_RL:
                    switch (Cmd.Op1)            // parse 4-bit list
                    {
                        case 1:
                            Cmd.Operands = "ea"; break;
                        //fmt_op_str("ea"); 
                        case 2:
                            Cmd.Operands = "elr"; break;
                        //fmt_op_str("elr");
                        case 3:
                            Cmd.Operands = "ea, elr"; break;
                        //fmt_op_str("ea, elr");
                        case 4:
                            Cmd.Operands = "epsw"; break;
                        //fmt_op_str("epsw"); 
                        case 5:
                            Cmd.Operands = "epsw, ea"; break;
                        //fmt_op_str("epsw, ea");
                        case 6:
                            Cmd.Operands = "epsw, elr"; break;
                        //fmt_op_str("epsw, elr");
                        case 7:
                            Cmd.Operands = "epsw, elr, ea"; break;
                        //fmt_op_str("epsw, elr, ea");
                        case 8:
                            Cmd.Operands = "lr"; break;
                        //fmt_op_str("lr");
                        case 9:
                            Cmd.Operands = "lr, ea"; break;
                        //fmt_op_str("lr, ea");
                        case 0xa:
                            Cmd.Operands = "lr, elr"; break;
                        //fmt_op_str("lr, elr");
                        case 0xb:
                            Cmd.Operands = "lr, ea, elr"; break;
                        //fmt_op_str("lr, ea, elr");
                        case 0xc:
                            Cmd.Operands = "lr, epsw"; break;
                        //fmt_op_str("lr, epsw");
                        case 0xd:
                            Cmd.Operands = "lr, epsw, ea"; break;
                        //fmt_op_str("lr, epsw, ea");
                        case 0xe:
                            Cmd.Operands = "lr, epsw, elr"; break;
                        //fmt_op_str("lr, epsw, elr");
                        case 0xf:
                            Cmd.Operands = "lr, epsw, elr, ea"; break;
                        //fmt_op_str("lr, epsw, elr, ea");
                        default:
                            Cmd.Operands = "??";
                            //fmt_op_str("?");
                            break;
                    }
                    break;

                case U8_POP_RL:
                    switch (Op1)            // parse 4-bit list
                    {
                        case 1:
                            Cmd.Operands = "ea"; break;
                        //fmt_op_str("ea"); break;
                        case 2:
                            Cmd.Operands = "pc"; break;
                        //fmt_op_str("pc"); break;
                        case 3:
                            Cmd.Operands = "ea, pc"; break;
                        //fmt_op_str("ea, pc"); break;
                        case 4:
                            Cmd.Operands = "psw"; break;
                        //fmt_op_str("psw"); break;
                        case 5:
                            Cmd.Operands = "ea, psw"; break;
                        //fmt_op_str("ea, psw"); break;
                        case 6:
                            Cmd.Operands = "pc, psw"; break;
                        //fmt_op_str("pc, psw"); break;
                        case 7:
                            Cmd.Operands = "ea, pc, psw"; break;
                        //fmt_op_str("ea, pc, psw"); break;
                        case 8:
                            Cmd.Operands = "lr"; break;
                        //fmt_op_str("lr"); break;
                        case 9:
                            Cmd.Operands = "ea, lr"; break;
                        //fmt_op_str("ea, lr"); break;
                        case 0xa:
                            Cmd.Operands = "pc, lr"; break;
                        //fmt_op_str("pc, lr"); break;
                        case 0xb:
                            Cmd.Operands = "ea, pc, lr"; break;
                        //fmt_op_str("ea, pc, lr"); break;
                        case 0xc:
                            Cmd.Operands = "lr, psw"; break;
                        //fmt_op_str("lr, psw"); break;
                        case 0xd:
                            Cmd.Operands = "ea, psw, lr"; break;
                        //fmt_op_str("ea, psw, lr"); break;
                        case 0xe:
                            Cmd.Operands = "pc, psw, lr"; break;
                        //fmt_op_str("pc, psw, lr"); break;
                        case 0xf:
                            Cmd.Operands = "ea, pc, psw, lr"; break;
                        //fmt_op_str("ea, pc, psw, lr"); break;
                        default:
                            Cmd.Operands = "??"; break;
                            //fmt_op_str("?");
                    }
                    break;

                // Coprocessor data transfer instructions
                case U8_MOV_CR_R:
                    Cmd.Operands = $"cr{Op1}, r{Op2}";
                    //fmt_op_str("cr%d, r%d", op1, op2);
                    break;
                case U8_MOV_CER_EA:
                    Cmd.Operands = $"cer{Op1}, [ea]";
                    //fmt_op_str("cer%d, [ea]", op1);
                    break;
                case U8_MOV_CER_EAP:
                    Cmd.Operands = $"cer{Op1}, [ea+]";
                    //fmt_op_str("cer%d, [ea+]", op1);
                    break;
                case U8_MOV_CR_EA:
                    Cmd.Operands = $"cr{Op1}, [ea]";
                    //fmt_op_str("cr%d, [ea]", op1);
                    break;
                case U8_MOV_CR_EAP:
                    Cmd.Operands = $"cr{Op1}, [ea+]";
                    //fmt_op_str("cr%d, [ea+]", op1);
                    break;
                case U8_MOV_CXR_EA:
                    Cmd.Operands = $"cxr{Op1}, [ea]";
                    //fmt_op_str("cxr%d, [ea]", op1);
                    break;
                case U8_MOV_CXR_EAP:
                    Cmd.Operands = $"cxr{Op1}, [ea+]";
                    //fmt_op_str("cxr%d, [ea+]", op1);
                    break;
                case U8_MOV_CQR_EA:
                    Cmd.Operands = $"cqr{Op1}, [ea]";
                    //fmt_op_str("cqr%d, [ea]", op1);
                    break;
                case U8_MOV_CQR_EAP:
                    Cmd.Operands = $"cqr{Op1}, [ea+]";
                    //fmt_op_str("cqr%d, [ea+]", op1);
                    break;
                case U8_MOV_R_CR:
                    Cmd.Operands = $"r{Op1}, cr{Op2}";
                    //fmt_op_str("r%d, cr%d", op1, op2);
                    break;
                case U8_MOV_EA_CER:
                    Cmd.Operands = $"[ea], cer{Op1}";
                    //fmt_op_str("[ea], cer%d", op1);
                    break;
                case U8_MOV_EAP_CER:
                    Cmd.Operands = $"[ea+], cer{Op1}";
                    //fmt_op_str("[ea+], cer%d", op1);
                    break;
                case U8_MOV_EA_CR:
                    Cmd.Operands = $"[ea], cr{Op1}";
                    //fmt_op_str("[ea], cr%d", op1);
                    break;
                case U8_MOV_EAP_CR:
                    Cmd.Operands = $"[ea+], cr{Op1}";
                    //fmt_op_str("[ea+], cr%d", op1);
                    break;
                case U8_MOV_EA_CXR:
                    Cmd.Operands = $"[ea], cxr{Op1}";
                    //fmt_op_str("[ea], cxr%d", op1);
                    break;
                case U8_MOV_EAP_CXR:
                    Cmd.Operands = $"[ea+], cxr{Op1}";
                    //fmt_op_str("[ea+], cxr%d", op1);
                    break;
                case U8_MOV_EA_CQR:
                    Cmd.Operands = $"[ea], cqr{Op1}";
                    //fmt_op_str("[ea], cqr%d", op1);
                    break;
                case U8_MOV_EAP_CQR:
                    Cmd.Operands = $"[ea+], cqr{Op1}";
                    //fmt_op_str("[ea+], cqr%d", op1);
                    break;

                // EA register data transfer instructions
                case U8_LEA_ER:
                    Cmd.Operands = $"[er{Op1}]";
                    //fmt_op_str("[er%d]", op1);
                    break;
                case U8_LEA_D16_ER:
                    Cmd.Operands = $"{sWord.ToString("X4")}h[er{Op1}]";
                    //fmt_op_str("%04xh[er%d]", s_word, op1);
                    break;
                case U8_LEA_DA:
                    Cmd.Operands = $"{sWord.ToString("X4")}h";
                    //fmt_op_str("%04xh", s_word);
                    break;

                // ALU Instructions
                case U8_DAA_R:
                case U8_DAS_R:
                case U8_NEG_R:
                    Cmd.Operands = $"r{Op1}";
                    //fmt_op_str("r%d", op1);
                    break;

                // Bit access instructions
                case U8_SB_R:
                case U8_RB_R:
                case U8_TB_R:
                    Cmd.Operands = $"r{Op1}.{Op2}";
                    //fmt_op_str("r%d.%d", op1, op2);
                    break;
                case U8_SB_DBIT:
                case U8_RB_DBIT:
                case U8_TB_DBIT:
                    Cmd.Operands = $"{sWord.ToString("X4")}h.{Op1}";
                    //fmt_op_str("%04xh.%d", s_word, op1);
                    break;

                // PSW access instructions (no operands)
                case U8_EI:
                case U8_DI:
                case U8_SC:
                case U8_RC:
                case U8_CPLC:
                    break;

                // Conditional relative branch instructions
                case U8_BGE_RAD:
                case U8_BLT_RAD:
                case U8_BGT_RAD:
                case U8_BLE_RAD:
                case U8_BGES_RAD:
                case U8_BLTS_RAD:
                case U8_BGTS_RAD:
                case U8_BLES_RAD:
                case U8_BNE_RAD:
                case U8_BEQ_RAD:
                case U8_BNV_RAD:
                case U8_BOV_RAD:
                case U8_BPS_RAD:
                case U8_BNS_RAD:
                case U8_BAL_RAD:
                    // handle +ive or -ive address jump cases
                    if ((byte)Op1 < 0)
                        Cmd.Operands = $"-{Math.Abs(0 - (byte)Op1).ToString("X")}h";
                    //fmt_op_str("-%02xh", abs(0 - (st8)op1));
                    else
                        Cmd.Operands = $"+{Math.Abs(0 - (byte)Op1).ToString("X")}h";
                    //fmt_op_str("+%02xh", (st8)op1);
                    break;

                // Sign extension instruction
                case U8_EXTBW_ER:
                    //fmt_op_str("er%d", op2);
                    Cmd.Operands = $"er{Op2}h";
                    break;

                // Software interrupt instructions
                case U8_SWI_O:
                    //fmt_op_str("#%xh", op1);
                    Cmd.Operands = $"#{Op1.ToString("X")}h";
                    break;
                case U8_BRK:
                    break;

                // Branch instructions
                case U8_B_AD:
                case U8_BL_AD:
                    //fmt_op_str("%xh:%04xh", op1, s_word);
                    Cmd.Operands = $"{Op1.ToString("X")}h:{sWord.ToString("X4")}h";
                    break;
                case U8_B_ER:
                case U8_BL_ER:
                    Cmd.Operands = $"er{Op1}";
                    //fmt_op_str("er%d", op1);
                    break;

                // Multiplication and division instructions
                case U8_MUL_ER:
                case U8_DIV_ER:
                    Cmd.Operands = $"er{Op1}, r{Op2}";
                    //fmt_op_str("er%d, r%d", op1, op2);
                    break;

                // Miscellaneous (no operands)
                case U8_INC_EA:
                case U8_DEC_EA:
                case U8_RT:
                case U8_RTI:
                case U8_NOP:
                    break;

                case U8_ILL:
                default:
                    // will display with 'dw' mnemonic to indicate 'data'
                    //fmt_op_str("%4xh", inst);
                    Cmd.Operands = $"{Inst.ToString("X4")}h";
                    break;
            }

            Cmd.Opcode = Inst;

            return i * 2;
        }

        // for signed 6-bit integers (Disp6)
        static int IsNegative6Bit(byte n)
        {
            return (n >> 5) & 0x01; // msb determines sign
        }

        // for signed 7-bit integers (#imm7)
        static int isNegative7Bit(byte n)
        {
            return (n >> 6) & 0x01; // msb determines sign
        }

        // calculate absolute value for signed 6-bit number (Disp6)
        static byte ABS6Bit(byte n)
        {
            if (IsNegative6Bit(n) == 1)      // -ive number
                return (byte)(~n + 1 & 0x3f); //	flip bits and add 1...
            else                // +ive number
                return (byte)(n & 0x3f);    //	...or just mask out top 2 bits;
        }

        // calculate absolute value for signed 7-bit number (#imm7)
        static byte ABS7Bit(byte n)
        {
            if (isNegative7Bit(n) == 1)      // -ive number
                return (byte)(~n + 1 & 0x7f); //	flip bits and add 1...
            else                // +ive number
                return (byte)(n & 0x7f);    //	...or just mask out top bit;
        }
    }
}
