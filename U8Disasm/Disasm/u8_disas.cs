using System;

namespace u8_lib.Disasm
{
    // TODO: Make Enum?
    public static class u8_disas
    {
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


        public static int u8_decode_inst(UInt16 opcode)
        {
            UInt16 t;
            int i;

            // iterate through master table of instructions, returning on match
            // FIXME: this can be done more efficiently
            for (i = 0; i < U8_INS_NUM; i++)
            {
                if ((opcode & u8_inst.u8inst[i].ins_mask) == u8_inst.u8inst[i].ins)
                    return i;
            }
            // or error
            return U8_ILL;
        }

        public static UInt16 u8_decode_operand(UInt16 inst, UInt16 mask)
        {
            UInt16 n;

            switch(mask)
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
            return (UInt16)((inst & mask) >> n);
        }

        // get opcode string
        public static int u8_decode_opcode(byte[] buf, int len, ref u8_cmd cmd)
        {
            int i = 0, addr = 0;

            UInt16 inst, s_word = 0, prefix = 0;
			UInt16 op1 = 0, op2 = 0;

            UInt16 pre_pseg = 0, pre_dsr = 0, pre_r = 0;
            string prefix_str = "";

            if (len < 2)    // words are always 2 bytes bro
                return -1;

            inst = BitConverter.ToUInt16(buf, 0);
            i++;    // word read counter

            cmd.type = u8_decode_inst(inst);

            // check for prefix
            switch(cmd.type)
            {
                case U8_PRE_PSEG:
                    pre_pseg = u8_decode_operand(inst, u8_inst.u8inst[cmd.type].op1_mask);
                    prefix_str = $"{pre_pseg.ToString("X2")}:";
                    prefix = inst;
                    break;
                case U8_PRE_DSR:
                    prefix_str = $"dsr:";
                    pre_dsr = 1;
                    prefix = inst;
                    break;
                case U8_PRE_R:
                    pre_r = u8_decode_operand(inst, u8_inst.u8inst[cmd.type].op1_mask);
                    prefix_str = $"r{pre_r.ToString()}:";
                    prefix = inst;
                    break;
            }

            if(prefix != 0)
            {
                if (len < (i + 1) * 2)
                    return -1;

                inst = BitConverter.ToUInt16(buf, i*2); // read the first real world after prefix
                i++;

                cmd.type = u8_decode_inst(inst);
            }

            if(u8_inst.u8inst[cmd.type].len == 2)
            {
                if (len < (i + 1) * 2)
                    return -1;

                s_word = BitConverter.ToUInt16(buf, i*2); // read second/third word from stream
                cmd.s_word = s_word;
                i++;
            }

            // set mnemonic
            cmd.instr = u8_inst.u8inst[cmd.type].name;

            // get op1
            if(u8_inst.u8inst[cmd.type].ops == 1)
            {
                op1 = u8_decode_operand(inst, u8_inst.u8inst[cmd.type].op1_mask);
                cmd.op1 = op1;
            }
            if(u8_inst.u8inst[cmd.type].ops == 2) // or get op1 and op2
            {
                op1 = u8_decode_operand(inst, u8_inst.u8inst[cmd.type].op1_mask);
                op2 = u8_decode_operand(inst, u8_inst.u8inst[cmd.type].op2_mask);
                cmd.op1 = op1;
                cmd.op2 = op2;
            }

            // format operands TODO: These may be buggy, I dont have any resource to confirm the opcodes
            switch(cmd.type)
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
					cmd.operands = $"r{op1} r{op2}";
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
					cmd.operands = $"r{op1} #{op2.ToString("X")}";
					//fmt_op_str("r%d, #%xh", op1, op2);
					break;

				// 16-bit extended register instructions
				case U8_ADD_ER:
				case U8_MOV_ER:
				case U8_CMP_ER:
					cmd.operands = $"er{op1} er{op2}"; // op1[11:8], op2[7:4] bitwise?
					//fmt_op_str("er%d, er%d", op1, op2);
					break;

                // Extended register/object instructions #imm7
                case U8_ADD_ER_O:
                case U8_MOV_ER_O:
					if (isneg_7bit((byte)op2) == 1)
						cmd.operands = $"er{op1}, #-{abs_7bit((byte)op2)}h";
					//fmt_op_str("er%d, #-%xh", op1, abs_7bit((byte)op2));
					else
						cmd.operands = $"er{op1}, #{abs_7bit((byte)op2)}h"; // TODO: look innto 0's
					//fmt_op_str("er%d, #%xh", op1, abs_7bit((byte)op2));
                    break;

                // Extended register load/store instructions
                case U8_L_ER_EA:
				case U8_ST_ER_EA:
					cmd.operands = $"er{op1} {prefix_str}[ea]";
					//fmt_op_str("er%d, %s[ea]", op1, prefix_str);
					break;
				case U8_L_ER_EAP:
				case U8_ST_ER_EAP:
					cmd.operands = $"er{op1} {prefix_str}[ea+]";
					//fmt_op_str("er%d, %s[ea+]", op1, prefix_str);
					break;
				case U8_L_ER_ER:
				case U8_ST_ER_ER:
					cmd.operands = $"er{op1} {prefix_str}[ea{op2}]";
					//fmt_op_str("er%d, %s[er%d]", op1, prefix_str, op2);
					break;
				case U8_L_ER_D16_ER:
				case U8_ST_ER_D16_ER:
					cmd.operands = $"er{op1} {op1.ToString("X4")}h{s_word}[ea{op2}]";
					//fmt_op_str("er%d, %04xh[er%d]", op1, s_word, op2);
					break;
                case U8_L_ER_D6_BP:
                case U8_ST_ER_D6_BP:
                    if (isneg_6bit((byte)op2) == 1)
                        cmd.operands = $"er{op1}, -{abs_6bit((byte)op2).ToString("X")}h[bp]";
                        //fmt_op_str("er%d, -%xh[bp]", op1, abs_6bit(op2));
                    else
                        cmd.operands = $"er{op1}, {abs_6bit((byte)op2).ToString("X")}h[bp]";
                    //fmt_op_str("er%d, %xh[bp]", op1, abs_6bit(op2));
                    break;
                case U8_L_ER_D6_FP:
                case U8_ST_ER_D6_FP:
                    if (isneg_6bit((byte)op2) == 1)
                        cmd.operands = $"er{op1}, -{abs_6bit((byte)op2).ToString("X")}h[fp]";
                        //fmt_op_str("er%d, -%xh[fp]", op1, abs_6bit(op2));
                    else
                        cmd.operands = $"er{op1}, -{abs_6bit((byte)op2).ToString("X")}h[fp]";
                    //fmt_op_str("er%d, %xh[fp]", op1, abs_6bit(op2));
                    break;
                case U8_L_ER_DA:
				case U8_ST_ER_DA:
					//fmt_op_str("er%d, %04xh", op1, s_word);
					cmd.operands = $"er{op1}, {s_word.ToString("X4")}h";
					break;

				// Register load/store instructions
				case U8_L_R_EA:
				case U8_ST_R_EA:
					//fmt_op_str("r%d, [ea]", op1);
					cmd.operands = $"r{op1}, [ea]";
					break;
				case U8_L_R_EAP:
				case U8_ST_R_EAP:
					//fmt_op_str("r%d, [ea+]", op1);
					cmd.operands = $"r{op1}, [ea+]";
					break;
				case U8_L_R_ER:
				case U8_ST_R_ER:
					//fmt_op_str("r%d, [er%d]", op1, op2);
					cmd.operands = $"r{op1}, [er{op2}]";
					break;
				case U8_L_R_D16_ER:
				case U8_ST_R_D16_ER:
					//fmt_op_str("r%d, %s%04xh[er%d]", op1, prefix_str, s_word, op2);
					cmd.operands = $"r{op1}, {prefix_str}{s_word.ToString("X4")}h[er{op2}]";
					break;
                case U8_L_R_D6_BP:
                case U8_ST_R_D6_BP:
                    if (isneg_6bit((byte)op2) == 1)
                        cmd.operands = $"r{op1}, {prefix_str}-{abs_6bit((byte)op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s-%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    else
                        cmd.operands = $"r{op1}, {prefix_str}{abs_6bit((byte)op2).ToString("X")}h[bp]";
                    //fmt_op_str("r%d, %s%xh[bp]", op1, prefix_str, abs_6bit(op2));
                    break;
                case U8_L_R_D6_FP:
                case U8_ST_R_D6_FP:
                    if (isneg_6bit((byte)op2) == 1)
                        cmd.operands = $"r{op1}, {prefix_str}-{abs_6bit((byte)op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s-%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    else
                        cmd.operands = $"r{op1}, {prefix_str}{abs_6bit((byte)op2).ToString("X")}h[fp]";
                    //fmt_op_str("r%d, %s%xh[fp]", op1, prefix_str, abs_6bit(op2));
                    break;
				case U8_L_R_DA:
				case U8_ST_R_DA:
					//fmt_op_str("r%d, %s%04xh", op1, prefix_str, s_word);
					cmd.operands = $"r{op1}, {prefix_str}{s_word.ToString("X4")}h";
					break;

				// Double/quad word register load/store instructions
				case U8_L_XR_EA:
				case U8_ST_XR_EA:
					//fmt_op_str("xr%d, %s[ea]", op1, prefix_str);
					cmd.operands = $"xr{op1}, {prefix_str}[ea]";
					break;
				case U8_L_XR_EAP:
				case U8_ST_XR_EAP:
					//fmt_op_str("xr%d, %s[ea+]", op1, prefix_str);
					cmd.operands = $"xr{op1}, {prefix_str}[ea+]";
					break;
				case U8_L_QR_EA:
				case U8_ST_QR_EA:
					//fmt_op_str("qr%d, %s[ea]", op1, prefix_str);
					cmd.operands = $"qr{op1}, {prefix_str}[ea]";
					break;
				case U8_L_QR_EAP:
				case U8_ST_QR_EAP:
					//fmt_op_str("qr%d, %s[ea+]", op1, prefix_str);
					cmd.operands = $"qr{op1}, {prefix_str}[ea+]";
					break;

				// Control register access instructions
				case U8_ADD_SP_O:
					//fmt_op_str("sp, #%xh", op1);
					cmd.operands = $"sp, #{op1.ToString("X")}h";
					break;
				case U8_MOV_ECSR_R:
					//fmt_op_str("ecsr, r%d", op1);
					cmd.operands = $"ecsr, r{op1}";
					break;
				case U8_MOV_ELR_ER:
					//fmt_op_str("elr, er%d", op1);
					cmd.operands = $"elr, er{op1}";
					break;
				case U8_MOV_EPSW_R:
					//fmt_op_str("epsw, r%d", op1);
					cmd.operands = $"epsw, r{op1}";
					break;
				case U8_MOV_ER_ELR:
					//fmt_op_str("er%d, elr", op1);
					cmd.operands = $"er{op1}, elr";
					break;
				case U8_MOV_ER_SP:
					//fmt_op_str("er%d, sp", op1);
					cmd.operands = $"er{op1}, sp";
					break;
				case U8_MOV_PSW_R:
					//fmt_op_str("psw, r%d", op1);
					cmd.operands = $"psw, r{op1}";
					break;
				case U8_MOV_PSW_O:
					//fmt_op_str("psw, #%xh", op1);
					cmd.operands = $"psw, #{op1.ToString("X")}h";
					break;
				case U8_MOV_R_ECSR:
					//fmt_op_str("r%d, ecsr", op1);
					cmd.operands = $"r{op1}, ecsr";
					break;
				case U8_MOV_R_EPSW:
					//fmt_op_str("r%d, epsw", op1);
					cmd.operands = $"r{op1}, epsw";
					break;
				case U8_MOV_R_PSW:
					//fmt_op_str("r%d, psw", op1);
					cmd.operands = $"r{op1}, psw";
					break;
				case U8_MOV_SP_ER:
					//fmt_op_str("sp, er%d", op1);
					cmd.operands = $"sp, er{op1}";
					break;

				// Push/pop instructions
				case U8_PUSH_ER:
				case U8_POP_ER:
					//fmt_op_str("er%d", op1);
					cmd.operands = $"er{op1}";
					break;
				case U8_PUSH_QR:
				case U8_POP_QR:
					//fmt_op_str("qr%d", op1);
					cmd.operands = $"qr{op1}";
					break;
				case U8_PUSH_R:
				case U8_POP_R:
					//fmt_op_str("r%d", op1);
					cmd.operands = $"r{op1}";
					break;
				case U8_PUSH_XR:
				case U8_POP_XR:
					//fmt_op_str("xr%d", op1);
					cmd.operands = $"xr{op1}";
					break;

                // Register list stack instructions
                case U8_PUSH_RL:
                    switch (cmd.op1)            // parse 4-bit list
                    {
                        case 1:
                            cmd.operands = "ea"; break;
                            //fmt_op_str("ea"); 
                        case 2:
                            cmd.operands = "elr"; break;
                            //fmt_op_str("elr");
                        case 3:
                            cmd.operands = "ea, elr"; break;
                            //fmt_op_str("ea, elr");
                        case 4:
                            cmd.operands = "epsw"; break;
                            //fmt_op_str("epsw"); 
                        case 5:
                            cmd.operands = "epsw, ea"; break;
                            //fmt_op_str("epsw, ea");
                        case 6:
                            cmd.operands = "epsw, elr"; break;
                            //fmt_op_str("epsw, elr");
                        case 7:
                            cmd.operands = "epsw, elr, ea"; break;
                            //fmt_op_str("epsw, elr, ea");
                        case 8:
                            cmd.operands = "lr"; break;
                            //fmt_op_str("lr");
                        case 9:
                            cmd.operands = "lr, ea"; break;
                            //fmt_op_str("lr, ea");
                        case 0xa:
                            cmd.operands = "lr, elr"; break;
                            //fmt_op_str("lr, elr");
                        case 0xb:
                            cmd.operands = "lr, ea, elr"; break;
                            //fmt_op_str("lr, ea, elr");
                        case 0xc:
                            cmd.operands = "lr, epsw"; break;
                            //fmt_op_str("lr, epsw");
                        case 0xd:
                            cmd.operands = "lr, epsw, ea"; break;
                            //fmt_op_str("lr, epsw, ea");
                        case 0xe:
                            cmd.operands = "lr, epsw, elr"; break;
                            //fmt_op_str("lr, epsw, elr");
                        case 0xf:
                            cmd.operands = "lr, epsw, elr, ea"; break;
                            //fmt_op_str("lr, epsw, elr, ea");
                        default:
                            cmd.operands = "??";
                            //fmt_op_str("?");
                            break;
                    }
                    break;

                case U8_POP_RL:
                    switch (op1)            // parse 4-bit list
                    {
                        case 1:
                            cmd.operands = "ea"; break;
                            //fmt_op_str("ea"); break;
                        case 2:
                            cmd.operands = "pc"; break;
                            //fmt_op_str("pc"); break;
                        case 3:
                            cmd.operands = "ea, pc"; break;
                            //fmt_op_str("ea, pc"); break;
                        case 4:
                            cmd.operands = "psw"; break;
                            //fmt_op_str("psw"); break;
                        case 5:
                            cmd.operands = "ea, psw"; break;
                            //fmt_op_str("ea, psw"); break;
                        case 6:
                            cmd.operands = "pc, psw"; break;
                            //fmt_op_str("pc, psw"); break;
                        case 7:
                            cmd.operands = "ea, pc, psw"; break;
                            //fmt_op_str("ea, pc, psw"); break;
                        case 8:
                            cmd.operands = "lr"; break;
                            //fmt_op_str("lr"); break;
                        case 9:
                            cmd.operands = "ea, lr"; break;
                            //fmt_op_str("ea, lr"); break;
                        case 0xa:
                            cmd.operands = "pc, lr"; break;
                            //fmt_op_str("pc, lr"); break;
                        case 0xb:
                            cmd.operands = "ea, pc, lr"; break;
                            //fmt_op_str("ea, pc, lr"); break;
                        case 0xc:
                            cmd.operands = "lr, psw"; break;
                            //fmt_op_str("lr, psw"); break;
                        case 0xd:
                            cmd.operands = "ea, psw, lr"; break;
                            //fmt_op_str("ea, psw, lr"); break;
                        case 0xe:
                            cmd.operands = "pc, psw, lr"; break;
                            //fmt_op_str("pc, psw, lr"); break;
                        case 0xf:
                            cmd.operands = "ea, pc, psw, lr"; break;
                            //fmt_op_str("ea, pc, psw, lr"); break;
                        default:
                            cmd.operands = "??"; break;
                            //fmt_op_str("?");
                    }
                    break;

                // Coprocessor data transfer instructions
                case U8_MOV_CR_R:
                    cmd.operands = $"cr{op1}, r{op2}";
                    //fmt_op_str("cr%d, r%d", op1, op2);
                    break;
                case U8_MOV_CER_EA:
                    cmd.operands = $"cer{op1}, [ea]";
                    //fmt_op_str("cer%d, [ea]", op1);
                    break;
                case U8_MOV_CER_EAP:
                    cmd.operands = $"cer{op1}, [ea+]";
                    //fmt_op_str("cer%d, [ea+]", op1);
                    break;
                case U8_MOV_CR_EA:
                    cmd.operands = $"cr{op1}, [ea]";
                    //fmt_op_str("cr%d, [ea]", op1);
                    break;
                case U8_MOV_CR_EAP:
                    cmd.operands = $"cr{op1}, [ea+]";
                    //fmt_op_str("cr%d, [ea+]", op1);
                    break;
                case U8_MOV_CXR_EA:
                    cmd.operands = $"cxr{op1}, [ea]";
                    //fmt_op_str("cxr%d, [ea]", op1);
                    break;
                case U8_MOV_CXR_EAP:
                    cmd.operands = $"cxr{op1}, [ea+]";
                    //fmt_op_str("cxr%d, [ea+]", op1);
                    break;
                case U8_MOV_CQR_EA:
                    cmd.operands = $"cqr{op1}, [ea]";
                    //fmt_op_str("cqr%d, [ea]", op1);
                    break;
                case U8_MOV_CQR_EAP:
                    cmd.operands = $"cqr{op1}, [ea+]";
                    //fmt_op_str("cqr%d, [ea+]", op1);
                    break;
                case U8_MOV_R_CR:
                    cmd.operands = $"r{op1}, cr{op2}";
                    //fmt_op_str("r%d, cr%d", op1, op2);
                    break;
                case U8_MOV_EA_CER:
                    cmd.operands = $"[ea], cer{op1}";
                    //fmt_op_str("[ea], cer%d", op1);
                    break;
                case U8_MOV_EAP_CER:
                    cmd.operands = $"[ea+], cer{op1}";
                    //fmt_op_str("[ea+], cer%d", op1);
                    break;
                case U8_MOV_EA_CR:
                    cmd.operands = $"[ea], cr{op1}";
                    //fmt_op_str("[ea], cr%d", op1);
                    break;
                case U8_MOV_EAP_CR:
                    cmd.operands = $"[ea+], cr{op1}";
                    //fmt_op_str("[ea+], cr%d", op1);
                    break;
                case U8_MOV_EA_CXR:
                    cmd.operands = $"[ea], cxr{op1}";
                    //fmt_op_str("[ea], cxr%d", op1);
                    break;
                case U8_MOV_EAP_CXR:
                    cmd.operands = $"[ea+], cxr{op1}";
                    //fmt_op_str("[ea+], cxr%d", op1);
                    break;
                case U8_MOV_EA_CQR:
                    cmd.operands = $"[ea], cqr{op1}";
                    //fmt_op_str("[ea], cqr%d", op1);
                    break;
                case U8_MOV_EAP_CQR:
                    cmd.operands = $"[ea+], cqr{op1}";
                    //fmt_op_str("[ea+], cqr%d", op1);
                    break;

                // EA register data transfer instructions
                case U8_LEA_ER:
                    cmd.operands = $"[er{op1}]";
                    //fmt_op_str("[er%d]", op1);
                    break;
                case U8_LEA_D16_ER:
                    cmd.operands = $"{s_word.ToString("X4")}h[er{op1}]";
                    //fmt_op_str("%04xh[er%d]", s_word, op1);
                    break;
                case U8_LEA_DA:
                    cmd.operands = $"{s_word.ToString("X4")}h";
                    //fmt_op_str("%04xh", s_word);
                    break;

                // ALU Instructions
                case U8_DAA_R:
                case U8_DAS_R:
                case U8_NEG_R:
                    cmd.operands = $"r{op1}";
                    //fmt_op_str("r%d", op1);
                    break;

                // Bit access instructions
                case U8_SB_R:
                case U8_RB_R:
                case U8_TB_R:
                    cmd.operands = $"r{op1}.{op2}";
                    //fmt_op_str("r%d.%d", op1, op2);
                    break;
                case U8_SB_DBIT:
                case U8_RB_DBIT:
                case U8_TB_DBIT:
                    cmd.operands = $"{s_word.ToString("X4")}h.{op1}";
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
                    if ((byte)op1 < 0)
                        cmd.operands = $"-{Math.Abs(0 - (byte)op1)}h";
                        //fmt_op_str("-%02xh", abs(0 - (st8)op1));
                    else
                        cmd.operands = $"+{Math.Abs(0 - (byte)op1)}h";
                    //fmt_op_str("+%02xh", (st8)op1);
                    break;

                // Sign extension instruction
                case U8_EXTBW_ER:
					//fmt_op_str("er%d", op2);
					cmd.operands = $"er{op2}h";
					break;

				// Software interrupt instructions
				case U8_SWI_O:
					//fmt_op_str("#%xh", op1);
					cmd.operands = $"#{op1.ToString("X")}h";
					break;
				case U8_BRK:
					break;

				// Branch instructions
				case U8_B_AD:
				case U8_BL_AD:
					//fmt_op_str("%xh:%04xh", op1, s_word);
					cmd.operands = $"{op1.ToString("X")}h:{s_word.ToString("X4")}h";
					break;
				case U8_B_ER:
				case U8_BL_ER:
					cmd.operands = $"er{op1}";
					//fmt_op_str("er%d", op1);
					break;

				// Multiplication and division instructions
				case U8_MUL_ER:
				case U8_DIV_ER:
					cmd.operands = $"er{op1}, r{op2}";
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
					cmd.operands = $"{inst.ToString("X4")}h";
					break;
			}

			cmd.opcode = inst;

			return i * 2;
		}

		// for signed 6-bit integers (Disp6)
		static int isneg_6bit(byte n)
		{
			return (n >> 5) & 0x01; // msb determines sign
		}

		// for signed 7-bit integers (#imm7)
		static int isneg_7bit(byte n)
		{
			return (n >> 6) & 0x01; // msb determines sign
		}

		// calculate absolute value for signed 6-bit number (Disp6)
		static byte abs_6bit(byte n)
		{
			if (isneg_6bit(n) == 1)      // -ive number
				return (byte)(~n + 1 & 0x3f); //	flip bits and add 1...
			else                // +ive number
				return (byte)(n & 0x3f);    //	...or just mask out top 2 bits;
		}

		// calculate absolute value for signed 7-bit number (#imm7)
		static byte abs_7bit(byte n)
		{
			if (isneg_7bit(n) == 1)      // -ive number
				return (byte)(~n + 1 & 0x7f); //	flip bits and add 1...
			else                // +ive number
				return (byte)(n & 0x7f);    //	...or just mask out top bit;
		}

	}
}
