using System;
using U8Disasm.Structs;

namespace U8Disasm.Disassembler
{
    // TODO: Make Enum?
    #region ASMDefinitions
    public enum U8_OP
    {
        // Arithmetic instructions;
        ADD_R = 0,
        ADD_O = 1,
        ADD_ER = 2,
        ADD_ER_O = 3,
        ADDC_R = 4,
        ADDC_O = 5,
        AND_R = 6,
        AND_O = 7,
        CMP_R = 8,
        CMP_O = 9,
        CMPC_R = 10,
        CMPC_O = 11,
        MOV_ER = 12,
        MOV_ER_O = 13,
        MOV_R = 14,
        MOV_O = 15,
        OR_R = 16,
        OR_O = 17,
        XOR_R = 18,
        XOR_O = 19,
        CMP_ER = 20,
        SUB_R = 21,
        SUBC_R = 22,

        // Shift instructions,
        SLL_R = 23,
        SLL_O = 24,
        SLLC_R = 25,
        SLLC_O = 26,
        SRA_R = 27,
        SRA_O = 28,
        SRL_R = 29,
        SRL_O = 30,
        SRLC_R = 31,
        SRLC_O = 32,

        // Load/store instructions,
        L_ER_EA = 33,
        L_ER_EAP = 34,
        L_ER_ER = 35,
        L_ER_D16_ER = 36,
        L_ER_D6_BP = 37,
        L_ER_D6_FP = 38,
        L_ER_DA = 39,
        L_R_EA = 40,
        L_R_EAP = 41,
        L_R_ER = 42,
        L_R_D16_ER = 43,
        L_R_D6_BP = 44,
        L_R_D6_FP = 45,
        L_R_DA = 46,
        L_XR_EA = 47,
        L_XR_EAP = 48,
        L_QR_EA = 49,
        L_QR_EAP = 50,
        ST_ER_EA = 51,
        ST_ER_EAP = 52,
        ST_ER_ER = 53,
        ST_ER_D16_ER = 54,
        ST_ER_D6_BP = 55,
        ST_ER_D6_FP = 56,
        ST_ER_DA = 57,
        ST_R_EA = 58,
        ST_R_EAP = 59,
        ST_R_ER = 60,
        ST_R_D16_ER = 61,
        ST_R_D6_BP = 62,
        ST_R_D6_FP = 63,
        ST_R_DA = 64,
        ST_XR_EA = 65,
        ST_XR_EAP = 66,
        ST_QR_EA = 67,
        ST_QR_EAP = 68,

        // Control register access instructions,
        ADD_SP_O = 69,
        MOV_ECSR_R = 70,
        MOV_ELR_ER = 71,
        MOV_EPSW_R = 72,
        MOV_ER_ELR = 73,
        MOV_ER_SP = 74,
        MOV_PSW_R = 75,
        MOV_PSW_O = 76,
        MOV_R_ECSR = 77,
        MOV_R_EPSW = 78,
        MOV_R_PSW = 79,
        MOV_SP_ER = 80,

        // Push/pop instructions,
        PUSH_ER = 81,
        PUSH_QR = 82,
        PUSH_R = 83,
        PUSH_XR = 84,
        PUSH_RL = 85,
        POP_ER = 86,
        POP_QR = 87,
        POP_R = 88,
        POP_XR = 89,
        POP_RL = 90,

        // Coprocessor data transfer instructions,
        MOV_CR_R = 91,
        MOV_CER_EA = 92,
        MOV_CER_EAP = 93,
        MOV_CR_EA = 94,
        MOV_CR_EAP = 95,
        MOV_CXR_EA = 96,
        MOV_CXR_EAP = 97,
        MOV_CQR_EA = 98,
        MOV_CQR_EAP = 99,
        MOV_R_CR = 100,
        MOV_EA_CER = 101,
        MOV_EAP_CER = 102,
        MOV_EA_CR = 103,
        MOV_EAP_CR = 104,
        MOV_EA_CXR = 105,
        MOV_EAP_CXR = 106,
        MOV_EA_CQR = 107,
        MOV_EAP_CQR = 108,

        // EA register data transfer instructions,
        LEA_ER = 109,
        LEA_D16_ER = 110,
        LEA_DA = 111,

        // ALU Instructions,
        DAA_R = 112,
        DAS_R = 113,
        NEG_R = 114,

        // Bit access instructions,
        SB_R = 115,
        SB_DBIT = 116,
        RB_R = 117,
        RB_DBIT = 118,
        TB_R = 119,
        TB_DBIT = 120,

        // PSW access instructions,
        EI = 121,
        DI = 122,
        SC = 123,
        RC = 124,
        CPLC = 125,

        // Conditional relative branch instructions,
        BGE_RAD = 126,
        BLT_RAD = 127,
        BGT_RAD = 128,
        BLE_RAD = 129,
        BGES_RAD = 130,
        BLTS_RAD = 131,
        BGTS_RAD = 132,
        BLES_RAD = 133,
        BNE_RAD = 134,
        BEQ_RAD = 135,
        BNV_RAD = 136,
        BOV_RAD = 137,
        BPS_RAD = 138,
        BNS_RAD = 139,
        BAL_RAD = 140,

        // Sign extension instruction,
        EXTBW_ER = 141,

        // Software interrupt instructions,
        SWI_O = 142,
        BRK = 143,

        // Branch instructions,
        B_AD = 144,
        B_ER = 145,
        BL_AD = 146,
        BL_ER = 147,

        // Multiplication and division instructions,
        MUL_ER = 148,
        DIV_ER = 149,

        // Miscellaneous,
        INC_EA = 150,
        DEC_EA = 151,
        RT = 152,
        RTI = 153,
        NOP = 154,

        // DSR prefix for load/store,
        PRE_PSEG = 155,
        PRE_DSR = 156,
        PRE_R = 157,

        // Undefined,
        ILL = 158,

        // define u8 instructions,
        INS_NUM = 159
    }
    #endregion ASMDefinitions

    public static class U8Decoder
    {
       
        public static U8_OP DecodeInstruction(UInt16 Opcode)
        {
            UInt16 t;
            int i;

            // iterate through master table of instructions, returning on match
            // FIXME: this can be done more efficiently
            for (i = 0; i < (int)U8_OP.INS_NUM; i++)
            {
                if (((UInt16)Opcode & U8Instructions.Table[i].InsMask) == U8Instructions.Table[i].Ins)
                    return (U8_OP)i;
            }

            // err
            return U8_OP.ILL;
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
                case U8_OP.PRE_PSEG:
                    PrePSEG = DecodeOperand(Inst, U8Instructions.Table[(int)Cmd.Type].Op1Mask);
                    PrefixStr = $"{PrePSEG.ToString("X2")}:";
                    Prefix = Inst;
                    break;
                case U8_OP.PRE_DSR:
                    PrefixStr = $"dsr:";
                    PreDSR = 1;
                    Prefix = Inst;
                    break;
                case U8_OP.PRE_R:
                    PreR = DecodeOperand(Inst, U8Instructions.Table[(int)Cmd.Type].Op1Mask);
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

                Cmd.Type = (U8_OP)DecodeInstruction(Inst);
            }

            if (U8Instructions.Table[(int)Cmd.Type].Length == 2)
            {
                if (Buf.Length < (i + 1) * 2)
                    return -1;

                sWord = BitConverter.ToUInt16(Buf, i * 2); // read second/third word from stream
                Cmd.sWord = sWord;
                i++;
            }

            // set mnemonic
            Cmd.Instruction = U8Instructions.Table[(int)Cmd.Type].Name;

            // get op1
            if (U8Instructions.Table[(int)Cmd.Type].Operands == 1)
            {
                Op1 = DecodeOperand(Inst, U8Instructions.Table[(int)Cmd.Type].Op1Mask);
                Cmd.Op1 = Op1;
            }
            if (U8Instructions.Table[(int)Cmd.Type].Operands == 2) // or get op1 and op2
            {
                Op1 = DecodeOperand(Inst, U8Instructions.Table[(int)Cmd.Type].Op1Mask);
                Op2 = DecodeOperand(Inst, U8Instructions.Table[(int)Cmd.Type].Op2Mask);
                Cmd.Op1 = Op1;
                Cmd.Op2 = Op2;
            }

            // format operands TODO: These may be buggy, I dont have any resource to confirm the opcodes
            switch (Cmd.Type)
            {
                // 8-bit register instructions
                case U8_OP.ADD_R:
                case U8_OP.AND_R:
                case U8_OP.ADDC_R:
                case U8_OP.CMP_R:
                case U8_OP.CMPC_R:
                case U8_OP.MOV_R:
                case U8_OP.OR_R:
                case U8_OP.XOR_R:
                case U8_OP.SUB_R:
                case U8_OP.SUBC_R:
                case U8_OP.SLL_R:
                case U8_OP.SLLC_R:
                case U8_OP.SRA_R:
                case U8_OP.SRL_R:
                case U8_OP.SRLC_R:
                    Cmd.Operands = $"r{Op1} r{Op2}";
                    //fmt_op_str("r%d, r%d", op1, op2);
                    break;

                // 8-bit register/object instructions
                case U8_OP.ADD_O:
                case U8_OP.AND_O:
                case U8_OP.ADDC_O:
                case U8_OP.CMP_O:
                case U8_OP.CMPC_O:
                case U8_OP.MOV_O:
                case U8_OP.OR_O:
                case U8_OP.XOR_O:
                case U8_OP.SLL_O:
                case U8_OP.SLLC_O:
                case U8_OP.SRA_O:
                case U8_OP.SRL_O:
                case U8_OP.SRLC_O:
                    Cmd.Operands = $"r{Op1} #{Op2.ToString("X")}";
                    //fmt_op_str("r%d, #%xh", op1, op2);
                    break;

                // 16-bit extended register instructions
                case U8_OP.ADD_ER:
                case U8_OP.MOV_ER:
                case U8_OP.CMP_ER:
                    Cmd.Operands = $"er{Op1} er{Op2}"; // op1[11:8], op2[7:4] bitwise?
                                                       //fmt_op_str("er%d, er%d", op1, op2);
                    break;

                // Extended register/object instructions #imm7
                case U8_OP.ADD_ER_O:
                case U8_OP.MOV_ER_O:
                    if (isNegative7Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, #-{ABS7Bit((byte)Op2)}h";
                    //fmt_op_str("er%d, #-%xh", op1, abs_7bit((byte)op2));
                    else
                        Cmd.Operands = $"er{Op1}, #{ABS7Bit((byte)Op2)}h"; // TODO: look innto 0's
                                                                           //fmt_op_str("er%d, #%xh", op1, abs_7bit((byte)op2));
                    break;

                // Extended register load/store instructions
                case U8_OP.L_ER_EA:
                case U8_OP.ST_ER_EA:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea]";
                    //fmt_op_str("er%d, %s[ea]", op1, prefix_str);
                    break;
                case U8_OP.L_ER_EAP:
                case U8_OP.ST_ER_EAP:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea+]";
                    //fmt_op_str("er%d, %s[ea+]", op1, prefix_str);
                    break;
                case U8_OP.L_ER_ER:
                case U8_OP.ST_ER_ER:
                    Cmd.Operands = $"er{Op1} {PrefixStr}[ea{Op2}]";
                    //fmt_op_str("er%d, %s[er%d]", op1, prefix_str, op2);
                    break;
                case U8_OP.L_ER_D16_ER:
                case U8_OP.ST_ER_D16_ER:
                    Cmd.Operands = $"er{Op1} {Op1.ToString("X4")}h{sWord}[ea{Op2}]";
                    //fmt_op_str("er%d, %04xh[er%d]", op1, s_word, op2);
                    break;
                case U8_OP.L_ER_D6_BP:
                case U8_OP.ST_ER_D6_BP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("er%d, -%xh[bp]", op1, abs_6bit(op2));
                    else
                        Cmd.Operands = $"er{Op1}, {ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("er%d, %xh[bp]", op1, abs_6bit(op2));
                    break;
                case U8_OP.L_ER_D6_FP:
                case U8_OP.ST_ER_D6_FP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("er%d, -%xh[fp]", op1, abs_6bit(op2));
                    else
                        Cmd.Operands = $"er{Op1}, -{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("er%d, %xh[fp]", op1, abs_6bit(op2));
                    break;
                case U8_OP.L_ER_DA:
                case U8_OP.ST_ER_DA:
                    //fmt_op_str("er%d, %04xh", op1, s_word);
                    Cmd.Operands = $"er{Op1}, {sWord.ToString("X4")}h";
                    break;

                // Register load/store instructions
                case U8_OP.L_R_EA:
                case U8_OP.ST_R_EA:
                    //fmt_op_str("r%d, [ea]", op1);
                    Cmd.Operands = $"r{Op1}, [ea]";
                    break;
                case U8_OP.L_R_EAP:
                case U8_OP.ST_R_EAP:
                    //fmt_op_str("r%d, [ea+]", op1);
                    Cmd.Operands = $"r{Op1}, [ea+]";
                    break;
                case U8_OP.L_R_ER:
                case U8_OP.ST_R_ER:
                    //fmt_op_str("r%d, [er%d]", op1, op2);
                    Cmd.Operands = $"r{Op1}, [er{Op2}]";
                    break;
                case U8_OP.L_R_D16_ER:
                case U8_OP.ST_R_D16_ER:
                    //fmt_op_str("r%d, %s%04xh[er%d]", op1, prefix_str, s_word, op2);
                    Cmd.Operands = $"r{Op1}, {PrefixStr}{sWord.ToString("X4")}h[er{Op2}]";
                    break;
                case U8_OP.L_R_D6_BP:
                case U8_OP.ST_R_D6_BP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"r{Op1}, {PrefixStr}-{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s-%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    else
                        Cmd.Operands = $"r{Op1}, {PrefixStr}{ABS6Bit((byte)Op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    break;
                case U8_OP.L_R_D6_FP:
                case U8_OP.ST_R_D6_FP:
                    if (IsNegative6Bit((byte)Op2) == 1)
                        Cmd.Operands = $"r{Op1}, {PrefixStr}-{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s-%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    else
                        Cmd.Operands = $"r{Op1}, {PrefixStr}{ABS6Bit((byte)Op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    break;
                case U8_OP.L_R_DA:
                case U8_OP.ST_R_DA:
                    //fmt_op_str("r%d, %s%04xh", op1, prefix_str, s_word);
                    Cmd.Operands = $"r{Op1}, {PrefixStr}{sWord.ToString("X4")}h";
                    break;

                // Double/quad word register load/store instructions
                case U8_OP.L_XR_EA:
                case U8_OP.ST_XR_EA:
                    //fmt_op_str("xr%d, %s[ea]", op1, prefix_str);
                    Cmd.Operands = $"xr{Op1}, {PrefixStr}[ea]";
                    break;
                case U8_OP.L_XR_EAP:
                case U8_OP.ST_XR_EAP:
                    //fmt_op_str("xr%d, %s[ea+]", op1, prefix_str);
                    Cmd.Operands = $"xr{Op1}, {PrefixStr}[ea+]";
                    break;
                case U8_OP.L_QR_EA:
                case U8_OP.ST_QR_EA:
                    //fmt_op_str("qr%d, %s[ea]", op1, prefix_str);
                    Cmd.Operands = $"qr{Op1}, {PrefixStr}[ea]";
                    break;
                case U8_OP.L_QR_EAP:
                case U8_OP.ST_QR_EAP:
                    //fmt_op_str("qr%d, %s[ea+]", op1, prefix_str);
                    Cmd.Operands = $"qr{Op1}, {PrefixStr}[ea+]";
                    break;

                // Control register access instructions
                case U8_OP.ADD_SP_O:
                    //fmt_op_str("sp, #%xh", op1);
                    Cmd.Operands = $"sp, #{Op1.ToString("X")}h";
                    break;
                case U8_OP.MOV_ECSR_R:
                    //fmt_op_str("ecsr, r%d", op1);
                    Cmd.Operands = $"ecsr, r{Op1}";
                    break;
                case U8_OP.MOV_ELR_ER:
                    //fmt_op_str("elr, er%d", op1);
                    Cmd.Operands = $"elr, er{Op1}";
                    break;
                case U8_OP.MOV_EPSW_R:
                    //fmt_op_str("epsw, r%d", op1);
                    Cmd.Operands = $"epsw, r{Op1}";
                    break;
                case U8_OP.MOV_ER_ELR:
                    //fmt_op_str("er%d, elr", op1);
                    Cmd.Operands = $"er{Op1}, elr";
                    break;
                case U8_OP.MOV_ER_SP:
                    //fmt_op_str("er%d, sp", op1);
                    Cmd.Operands = $"er{Op1}, sp";
                    break;
                case U8_OP.MOV_PSW_R:
                    //fmt_op_str("psw, r%d", op1);
                    Cmd.Operands = $"psw, r{Op1}";
                    break;
                case U8_OP.MOV_PSW_O:
                    //fmt_op_str("psw, #%xh", op1);
                    Cmd.Operands = $"psw, #{Op1.ToString("X")}h";
                    break;
                case U8_OP.MOV_R_ECSR:
                    //fmt_op_str("r%d, ecsr", op1);
                    Cmd.Operands = $"r{Op1}, ecsr";
                    break;
                case U8_OP.MOV_R_EPSW:
                    //fmt_op_str("r%d, epsw", op1);
                    Cmd.Operands = $"r{Op1}, epsw";
                    break;
                case U8_OP.MOV_R_PSW:
                    //fmt_op_str("r%d, psw", op1);
                    Cmd.Operands = $"r{Op1}, psw";
                    break;
                case U8_OP.MOV_SP_ER:
                    //fmt_op_str("sp, er%d", op1);
                    Cmd.Operands = $"sp, er{Op1}";
                    break;

                // Push/pop instructions
                case U8_OP.PUSH_ER:
                case U8_OP.POP_ER:
                    //fmt_op_str("er%d", op1);
                    Cmd.Operands = $"er{Op1}";
                    break;
                case U8_OP.PUSH_QR:
                case U8_OP.POP_QR:
                    //fmt_op_str("qr%d", op1);
                    Cmd.Operands = $"qr{Op1}";
                    break;
                case U8_OP.PUSH_R:
                case U8_OP.POP_R:
                    //fmt_op_str("r%d", op1);
                    Cmd.Operands = $"r{Op1}";
                    break;
                case U8_OP.PUSH_XR:
                case U8_OP.POP_XR:
                    //fmt_op_str("xr%d", op1);
                    Cmd.Operands = $"xr{Op1}";
                    break;

                // Register list stack instructions
                case U8_OP.PUSH_RL:
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

                case U8_OP.POP_RL:
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
                case U8_OP.MOV_CR_R:
                    Cmd.Operands = $"cr{Op1}, r{Op2}";
                    //fmt_op_str("cr%d, r%d", op1, op2);
                    break;
                case U8_OP.MOV_CER_EA:
                    Cmd.Operands = $"cer{Op1}, [ea]";
                    //fmt_op_str("cer%d, [ea]", op1);
                    break;
                case U8_OP.MOV_CER_EAP:
                    Cmd.Operands = $"cer{Op1}, [ea+]";
                    //fmt_op_str("cer%d, [ea+]", op1);
                    break;
                case U8_OP.MOV_CR_EA:
                    Cmd.Operands = $"cr{Op1}, [ea]";
                    //fmt_op_str("cr%d, [ea]", op1);
                    break;
                case U8_OP.MOV_CR_EAP:
                    Cmd.Operands = $"cr{Op1}, [ea+]";
                    //fmt_op_str("cr%d, [ea+]", op1);
                    break;
                case U8_OP.MOV_CXR_EA:
                    Cmd.Operands = $"cxr{Op1}, [ea]";
                    //fmt_op_str("cxr%d, [ea]", op1);
                    break;
                case U8_OP.MOV_CXR_EAP:
                    Cmd.Operands = $"cxr{Op1}, [ea+]";
                    //fmt_op_str("cxr%d, [ea+]", op1);
                    break;
                case U8_OP.MOV_CQR_EA:
                    Cmd.Operands = $"cqr{Op1}, [ea]";
                    //fmt_op_str("cqr%d, [ea]", op1);
                    break;
                case U8_OP.MOV_CQR_EAP:
                    Cmd.Operands = $"cqr{Op1}, [ea+]";
                    //fmt_op_str("cqr%d, [ea+]", op1);
                    break;
                case U8_OP.MOV_R_CR:
                    Cmd.Operands = $"r{Op1}, cr{Op2}";
                    //fmt_op_str("r%d, cr%d", op1, op2);
                    break;
                case U8_OP.MOV_EA_CER:
                    Cmd.Operands = $"[ea], cer{Op1}";
                    //fmt_op_str("[ea], cer%d", op1);
                    break;
                case U8_OP.MOV_EAP_CER:
                    Cmd.Operands = $"[ea+], cer{Op1}";
                    //fmt_op_str("[ea+], cer%d", op1);
                    break;
                case U8_OP.MOV_EA_CR:
                    Cmd.Operands = $"[ea], cr{Op1}";
                    //fmt_op_str("[ea], cr%d", op1);
                    break;
                case U8_OP.MOV_EAP_CR:
                    Cmd.Operands = $"[ea+], cr{Op1}";
                    //fmt_op_str("[ea+], cr%d", op1);
                    break;
                case U8_OP.MOV_EA_CXR:
                    Cmd.Operands = $"[ea], cxr{Op1}";
                    //fmt_op_str("[ea], cxr%d", op1);
                    break;
                case U8_OP.MOV_EAP_CXR:
                    Cmd.Operands = $"[ea+], cxr{Op1}";
                    //fmt_op_str("[ea+], cxr%d", op1);
                    break;
                case U8_OP.MOV_EA_CQR:
                    Cmd.Operands = $"[ea], cqr{Op1}";
                    //fmt_op_str("[ea], cqr%d", op1);
                    break;
                case U8_OP.MOV_EAP_CQR:
                    Cmd.Operands = $"[ea+], cqr{Op1}";
                    //fmt_op_str("[ea+], cqr%d", op1);
                    break;

                // EA register data transfer instructions
                case U8_OP.LEA_ER:
                    Cmd.Operands = $"[er{Op1}]";
                    //fmt_op_str("[er%d]", op1);
                    break;
                case U8_OP.LEA_D16_ER:
                    Cmd.Operands = $"{sWord.ToString("X4")}h[er{Op1}]";
                    //fmt_op_str("%04xh[er%d]", s_word, op1);
                    break;
                case U8_OP.LEA_DA:
                    Cmd.Operands = $"{sWord.ToString("X4")}h";
                    //fmt_op_str("%04xh", s_word);
                    break;

                // ALU Instructions
                case U8_OP.DAA_R:
                case U8_OP.DAS_R:
                case U8_OP.NEG_R:
                    Cmd.Operands = $"r{Op1}";
                    //fmt_op_str("r%d", op1);
                    break;

                // Bit access instructions
                case U8_OP.SB_R:
                case U8_OP.RB_R:
                case U8_OP.TB_R:
                    Cmd.Operands = $"r{Op1}.{Op2}";
                    //fmt_op_str("r%d.%d", op1, op2);
                    break;
                case U8_OP.SB_DBIT:
                case U8_OP.RB_DBIT:
                case U8_OP.TB_DBIT:
                    Cmd.Operands = $"{sWord.ToString("X4")}h.{Op1}";
                    //fmt_op_str("%04xh.%d", s_word, op1);
                    break;

                // PSW access instructions (no operands)
                case U8_OP.EI:
                case U8_OP.DI:
                case U8_OP.SC:
                case U8_OP.RC:
                case U8_OP.CPLC:
                    break;

                // Conditional relative branch instructions
                case U8_OP.BGE_RAD:
                case U8_OP.BLT_RAD:
                case U8_OP.BGT_RAD:
                case U8_OP.BLE_RAD:
                case U8_OP.BGES_RAD:
                case U8_OP.BLTS_RAD:
                case U8_OP.BGTS_RAD:
                case U8_OP.BLES_RAD:
                case U8_OP.BNE_RAD:
                case U8_OP.BEQ_RAD:
                case U8_OP.BNV_RAD:
                case U8_OP.BOV_RAD:
                case U8_OP.BPS_RAD:
                case U8_OP.BNS_RAD:
                case U8_OP.BAL_RAD:
                    // handle +ive or -ive address jump cases
                    if ((byte)Op1 < 0)
                        Cmd.Operands = $"-{Math.Abs(0 - (byte)Op1).ToString("X")}h";
                    //fmt_op_str("-%02xh", abs(0 - (st8)op1));
                    else
                        Cmd.Operands = $"+{Math.Abs(0 - (byte)Op1).ToString("X")}h";
                    //fmt_op_str("+%02xh", (st8)op1);
                    break;

                // Sign extension instruction
                case U8_OP.EXTBW_ER:
                    //fmt_op_str("er%d", op2);
                    Cmd.Operands = $"er{Op2}h";
                    break;

                // Software interrupt instructions
                case U8_OP.SWI_O:
                    //fmt_op_str("#%xh", op1);
                    Cmd.Operands = $"#{Op1.ToString("X")}h";
                    break;
                case U8_OP.BRK:
                    break;

                // Branch instructions
                case U8_OP.B_AD:
                case U8_OP.BL_AD:
                    //fmt_op_str("%xh:%04xh", op1, s_word);
                    Cmd.Operands = $"{Op1.ToString("X")}h:{sWord.ToString("X4")}h";
                    break;
                case U8_OP.B_ER:
                case U8_OP.BL_ER:
                    Cmd.Operands = $"er{Op1}";
                    //fmt_op_str("er%d", op1);
                    break;

                // Multiplication and division instructions
                case U8_OP.MUL_ER:
                case U8_OP.DIV_ER:
                    Cmd.Operands = $"er{Op1}, r{Op2}";
                    //fmt_op_str("er%d, r%d", op1, op2);
                    break;

                // Miscellaneous (no operands)
                case U8_OP.INC_EA:
                case U8_OP.DEC_EA:
                case U8_OP.RT:
                case U8_OP.RTI:
                case U8_OP.NOP:
                    break;

                case U8_OP.ILL:
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
        public static int IsNegative6Bit(byte n)
        {
            return (n >> 5) & 0x01; // msb determines sign
        }

        // for signed 7-bit integers (#imm7)
        public static int isNegative7Bit(byte n)
        {
            return (n >> 6) & 0x01; // msb determines sign
        }

        // calculate absolute value for signed 6-bit number (Disp6)
        public static byte ABS6Bit(byte n)
        {
            if (IsNegative6Bit(n) == 1)      // -ive number
                return (byte)(~n + 1 & 0x3f); //	flip bits and add 1...
            else                // +ive number
                return (byte)(n & 0x3f);    //	...or just mask out top 2 bits;
        }

        // calculate absolute value for signed 7-bit number (#imm7)
        public static byte ABS7Bit(byte n)
        {
            if (isNegative7Bit(n) == 1)      // -ive number
                return (byte)(~n + 1 & 0x7f); //	flip bits and add 1...
            else                // +ive number
                return (byte)(n & 0x7f);    //	...or just mask out top bit;
        }
    }
}
