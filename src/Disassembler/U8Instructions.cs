using U8Disasm.Structs;

namespace U8Disasm.Disassembler
{


    public static class U8Instructions
    {
        // Thanks : https://github.com/ferib/u8_r2_plugin/blob/master/u8_inst.c
        public static U8Instruction[] Table =
        {
            // Arithmetic instructions
		    new U8Instruction() { Id=U8Decoder.U8_ADD_R, Name="add", Length=1, Operands=2, Flags=0b111101, Ins=0x8001, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ADD_O, Name="add", Length=1, Operands=2, Flags=0b111101, Ins=0x1000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_ADD_ER, Name="add", Length=1, Operands=2, Flags=0b111101, Ins=0xf006, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ADD_ER_O, Name="add", Length=1, Operands=2, Flags=0b111101, Ins=0xe080, InsMask=0xf180, Op1Mask=0x0f00, Op2Mask=0x007f},
            new U8Instruction() { Id=U8Decoder.U8_ADDC_R, Name="addc", Length=1, Operands=2, Flags=0b111101, Ins=0x8006, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ADDC_O, Name="addc", Length=1, Operands=2, Flags=0b111101, Ins=0x6000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_AND_R, Name="and", Length=1, Operands=2, Flags=0b011000, Ins=0x8002, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_AND_O, Name="and", Length=1, Operands=2, Flags=0b011000, Ins=0x2000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_CMP_R, Name="cmp", Length=1, Operands=2, Flags=0b111101, Ins=0x8007, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_CMP_O, Name="cmp", Length=1, Operands=2, Flags=0b111101, Ins=0x7000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_CMPC_R, Name="cmpc", Length=1, Operands=2, Flags=0b111101, Ins=0x8005, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_CMPC_O, Name="cmpc", Length=1, Operands=2, Flags=0b111101, Ins=0x5000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ER, Name="mov", Length=1, Operands=2, Flags=0b011000, Ins=0xf005, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ER_O, Name="mov", Length=1, Operands=2, Flags=0b011000, Ins=0xe000, InsMask=0xf180, Op1Mask=0x0f00, Op2Mask=0x007f},
            new U8Instruction() { Id=U8Decoder.U8_MOV_R, Name="mov", Length=1, Operands=2, Flags=0b011000, Ins=0x8000, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_MOV_O, Name="mov", Length=1, Operands=2, Flags=0b011000, Ins=0x0000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_OR_R, Name="or", Length=1, Operands=2, Flags=0b011000, Ins=0x8003, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_OR_O, Name="or", Length=1, Operands=2, Flags=0b011000, Ins=0x3000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_XOR_R, Name="xor", Length=1, Operands=2, Flags=0b011000, Ins=0x8004, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_XOR_O, Name="xor", Length=1, Operands=2, Flags=0b011000, Ins=0x4000, InsMask=0xf000, Op1Mask=0x0f00, Op2Mask=0x00ff},
            new U8Instruction() { Id=U8Decoder.U8_CMP_ER, Name="cmp", Length=1, Operands=2, Flags=0b111101, Ins=0xf007, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SUB_R, Name="sub", Length=1, Operands=2, Flags=0b111101, Ins=0x8008, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SUBC_R, Name="subc", Length=1, Operands=2, Flags=0b111101, Ins=0x8009, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},

	    // Shift instructions
		    new U8Instruction() { Id=U8Decoder.U8_SLL_R, Name="sll", Length=1, Operands=2, Flags=0b100000, Ins=0x800a, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SLL_O, Name="sll", Length=1, Operands=2, Flags=0b100000, Ins=0x900a, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
            new U8Instruction() { Id=U8Decoder.U8_SLLC_R, Name="sllc", Length=1, Operands=2, Flags=0b100000, Ins=0x800b, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SLLC_O, Name="sllc", Length=1, Operands=2, Flags=0b100000, Ins=0x900b, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
            new U8Instruction() { Id=U8Decoder.U8_SRA_R, Name="sra", Length=1, Operands=2, Flags=0b100000, Ins=0x800e, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SRA_O, Name="sra", Length=1, Operands=2, Flags=0b100000, Ins=0x900e, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
            new U8Instruction() { Id=U8Decoder.U8_SRL_R, Name="srl", Length=1, Operands=2, Flags=0b100000, Ins=0x800c, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SRL_O, Name="srl", Length=1, Operands=2, Flags=0b100000, Ins=0x900c, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
            new U8Instruction() { Id=U8Decoder.U8_SRLC_R, Name="srlc", Length=1, Operands=2, Flags=0b100000, Ins=0x800d, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_SRLC_O, Name="srlc", Length=1, Operands=2, Flags=0b100000, Ins=0x900d, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},

	    // Load/store instructions
		    new U8Instruction() { Id=U8Decoder.U8_L_ER_EA, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9032, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_EAP, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9052, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_ER, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0x9002, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_D16_ER, Name="l", Length=2, Operands=2, Flags=0b011000, Ins=0xa008, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_D6_BP, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0xb000, InsMask=0xf1c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_D6_FP, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0xb040, InsMask=0xf1c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_L_ER_DA, Name="l", Length=2, Operands=1, Flags=0b011000, Ins=0x9012, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_R_EA, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9030, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_R_EAP, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9050, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_R_ER, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0x9000, InsMask=0xf01f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_L_R_D16_ER, Name="l", Length=2, Operands=2, Flags=0b011000, Ins=0x9008, InsMask=0xf01f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_L_R_D6_BP, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0xd000, InsMask=0xf0c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_L_R_D6_FP, Name="l", Length=1, Operands=2, Flags=0b011000, Ins=0xd040, InsMask=0xf0c0, Op1Mask=0x0f00, Op2Mask=0x003f},
	    // Per core ref.:
	    //{.id=U8_L_R_DA, name="l", len=2, ops=1, flags=0b011000, ins=0x9010, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
	    // ....but, we match SDK disassembler behaviour, w.r.t. third nibble:
		    new U8Instruction() { Id=U8Decoder.U8_L_R_DA, Name="l", Length=2, Operands=1, Flags=0b011000, Ins=0x9010, InsMask=0xf01f, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_XR_EA, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9034, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_XR_EAP, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9054, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_QR_EA, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9036, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_L_QR_EAP, Name="l", Length=1, Operands=1, Flags=0b011000, Ins=0x9056, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_EA, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9033, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_EAP, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9053, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_ER, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0x9003, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_D16_ER, Name="st", Length=2, Operands=2, Flags=0b000000, Ins=0xa009, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_D6_BP, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0xb080, InsMask=0xf1c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_D6_FP, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0xb0c0, InsMask=0xf1c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_ST_ER_DA, Name="st", Length=2, Operands=1, Flags=0b000000, Ins=0x9013, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_EA, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9031, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_EAP, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9051, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_ER, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0x9001, InsMask=0xf01f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_D16_ER, Name="st", Length=2, Operands=2, Flags=0b000000, Ins=0x9009, InsMask=0xf01f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_D6_BP, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0xd080, InsMask=0xf0c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_D6_FP, Name="st", Length=1, Operands=2, Flags=0b000000, Ins=0xd0c0, InsMask=0xf0c0, Op1Mask=0x0f00, Op2Mask=0x003f},
            new U8Instruction() { Id=U8Decoder.U8_ST_R_DA, Name="st", Length=2, Operands=1, Flags=0b000000, Ins=0x9011, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_XR_EA, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9035, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_XR_EAP, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9055, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_QR_EA, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9037, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_ST_QR_EAP, Name="st", Length=1, Operands=1, Flags=0b000000, Ins=0x9057, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},

	    // Control register access instructions
		    new U8Instruction() { Id=U8Decoder.U8_ADD_SP_O, Name="add", Length=1, Operands=1, Flags=0b000000, Ins=0xe100, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ECSR_R, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa00f, InsMask=0xff0f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ELR_ER, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa00d, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EPSW_R, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa00c, InsMask=0xff0f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ER_ELR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa005, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_ER_SP, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa01a, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_PSW_R, Name="mov", Length=1, Operands=1, Flags=0b111111, Ins=0xa00b, InsMask=0xff0f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_PSW_O, Name="mov", Length=1, Operands=1, Flags=0b111111, Ins=0xa00b, InsMask=0xff0f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_R_ECSR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa007, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_R_EPSW, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa004, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_R_PSW, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa003, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_SP_ER, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xa10a, InsMask=0xff1f, Op1Mask=0x00f0, Op2Mask=0x0000},

	    // Push/pop instructions
		    new U8Instruction() { Id=U8Decoder.U8_PUSH_ER, Name="push", Length=1, Operands=1, Flags=0b000000, Ins=0xf05e, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PUSH_QR, Name="push", Length=1, Operands=1, Flags=0b000000, Ins=0xf07e, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PUSH_R, Name="push", Length=1, Operands=1, Flags=0b000000, Ins=0xf04e, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PUSH_XR, Name="push", Length=1, Operands=1, Flags=0b000000, Ins=0xf06e, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PUSH_RL, Name="push", Length=1, Operands=1, Flags=0b000000, Ins=0xf0ce, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_POP_ER, Name="pop", Length=1, Operands=1, Flags=0b000000, Ins=0xf01e, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_POP_QR, Name="pop", Length=1, Operands=1, Flags=0b000000, Ins=0xf03e, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_POP_R, Name="pop", Length=1, Operands=1, Flags=0b000000, Ins=0xf00e, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_POP_XR, Name="pop", Length=1, Operands=1, Flags=0b000000, Ins=0xf02e, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_POP_RL, Name="pop", Length=1, Operands=1, Flags=0b111111, Ins=0xf08e, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},

	    // Coprocessor data transfer instructions
		    new U8Instruction() { Id=U8Decoder.U8_MOV_CR_R, Name="mov", Length=1, Operands=2, Flags=0b000000, Ins=0xa00e, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CER_EA, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf02d, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CER_EAP, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf03d, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CR_EA, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf00d, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CR_EAP, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf01d, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CXR_EA, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf04d, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CXR_EAP, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf05d, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CQR_EA, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf06d, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_CQR_EAP, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf07d, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_R_CR, Name="mov", Length=1, Operands=2, Flags=0b000000, Ins=0xa006, InsMask=0xf00f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EA_CER, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0ad, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EAP_CER, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0bd, InsMask=0xf1ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EA_CR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf08d, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EAP_CR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf09d, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EA_CXR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0cd, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EAP_CXR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0dd, InsMask=0xf3ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EA_CQR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0ed, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_MOV_EAP_CQR, Name="mov", Length=1, Operands=1, Flags=0b000000, Ins=0xf0fd, InsMask=0xf7ff, Op1Mask=0x0f00, Op2Mask=0x0000},

	    // EA register data transfer instructions
		    new U8Instruction() { Id=U8Decoder.U8_LEA_ER, Name="lea", Length=1, Operands=1, Flags=0b000000, Ins=0xf00a, InsMask=0xf01f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_LEA_D16_ER, Name="lea", Length=2, Operands=1, Flags=0b000000, Ins=0xf00b, InsMask=0xf01f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_LEA_DA, Name="lea", Length=2, Operands=1, Flags=0b000000, Ins=0xf00c, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},

	    // ALU Instructions
		    new U8Instruction() { Id=U8Decoder.U8_DAA_R, Name="daa", Length=1, Operands=1, Flags=0b111001, Ins=0x801f, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_DAS_R, Name="das", Length=1, Operands=1, Flags=0b111001, Ins=0x803f, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_NEG_R, Name="neg", Length=1, Operands=1, Flags=0b111101, Ins=0x805f, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},

	    // Bit access instructions
		    new U8Instruction() { Id=U8Decoder.U8_SB_R, Name="sb", Length=1, Operands=2, Flags=0b010000, Ins=0xa000, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
            new U8Instruction() { Id=U8Decoder.U8_SB_DBIT, Name="sb", Length=2, Operands=1, Flags=0b010000, Ins=0xa080, InsMask=0xff8f, Op1Mask=0x0070, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_RB_R, Name="rb", Length=1, Operands=2, Flags=0b010000, Ins=0xa002, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
	    // Per core ref.:
    //	{.id=U8_RB_DBIT, name="rb", len=2, ops=1, flags=0b010000, ins=0xa082, ins_mask=0xff8f, op1_mask=0x0070, op2_mask=0x0000},
	    // ....but, we match SDK disassembler behaviour, w.r.t. second nibble:
		    new U8Instruction() { Id=U8Decoder.U8_RB_DBIT, Name="rb", Length=2, Operands=1, Flags=0b010000, Ins=0xa082, InsMask=0xf08f, Op1Mask=0x0070, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_TB_R, Name="tb", Length=1, Operands=2, Flags=0b010000, Ins=0xa001, InsMask=0xf08f, Op1Mask=0x0f00, Op2Mask=0x0070},
    //	{.id=U8_TB_DBIT, name="tb", len=2, ops=1, flags=0b010000, ins=0xa081, ins_mask=0xff8f, op1_mask=0x0070, op2_mask=0x0000},
		    new U8Instruction() { Id=U8Decoder.U8_TB_DBIT, Name="tb", Length=2, Operands=1, Flags=0b010000, Ins=0xa081, InsMask=0xf08f, Op1Mask=0x0070, Op2Mask=0x0000},

	    // PSW access instructions
		    new U8Instruction() { Id=U8Decoder.U8_EI, Name="ei", Length=1, Operands=0, Flags=0b000010, Ins=0xed08, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_DI, Name="di", Length=1, Operands=0, Flags=0b000010, Ins=0xebf7, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_SC, Name="sc", Length=1, Operands=0, Flags=0b100000, Ins=0xed80, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_RC, Name="rc", Length=1, Operands=0, Flags=0b100000, Ins=0xeb7f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_CPLC, Name="cplc", Length=1, Operands=0, Flags=0b100000, Ins=0xfecf, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},

	    // Conditional relative branch instructions
		    new U8Instruction() { Id=U8Decoder.U8_BGE_RAD, Name="bge", Length=1, Operands=1, Flags=0b000000, Ins=0xc000, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BLT_RAD, Name="blt", Length=1, Operands=1, Flags=0b000000, Ins=0xc100, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BGT_RAD, Name="bgt", Length=1, Operands=1, Flags=0b000000, Ins=0xc200, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BLE_RAD, Name="ble", Length=1, Operands=1, Flags=0b000000, Ins=0xc130, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BGES_RAD, Name="bges", Length=1, Operands=1, Flags=0b000000, Ins=0xc400, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BLTS_RAD, Name="blts", Length=1, Operands=1, Flags=0b000000, Ins=0xc500, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BGTS_RAD, Name="bgts", Length=1, Operands=1, Flags=0b000000, Ins=0xc600, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BLES_RAD, Name="bles", Length=1, Operands=1, Flags=0b000000, Ins=0xc700, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BNE_RAD, Name="bne", Length=1, Operands=1, Flags=0b000000, Ins=0xc800, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BEQ_RAD, Name="beq", Length=1, Operands=1, Flags=0b000000, Ins=0xc900, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BNV_RAD, Name="bnv", Length=1, Operands=1, Flags=0b000000, Ins=0xca00, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BOV_RAD, Name="bov", Length=1, Operands=1, Flags=0b000000, Ins=0xcb00, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BPS_RAD, Name="bps", Length=1, Operands=1, Flags=0b000000, Ins=0xcc00, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BNS_RAD, Name="bns", Length=1, Operands=1, Flags=0b000000, Ins=0xcd00, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BAL_RAD, Name="bal", Length=1, Operands=1, Flags=0b000000, Ins=0xce00, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},

	    // Sign extension instruction
		    new U8Instruction() { Id=U8Decoder.U8_EXTBW_ER, Name="extbw", Length=1, Operands=2, Flags=0b011000, Ins=0x810f, InsMask=0xf11f, Op1Mask=0x0f00, Op2Mask=0x00f0},

	    // Software interrupt instructions
		    new U8Instruction() { Id=U8Decoder.U8_SWI_O, Name="swi", Length=1, Operands=1, Flags=0b000010, Ins=0xe500, InsMask=0xffc0, Op1Mask=0x003f, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BRK, Name="brk", Length=1, Operands=0, Flags=0b000000, Ins=0xffff, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},

	    // Branch instructions
		    new U8Instruction() { Id=U8Decoder.U8_B_AD, Name="b", Length=2, Operands=1, Flags=0b000000, Ins=0xf000, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_B_ER, Name="b", Length=1, Operands=1, Flags=0b000000, Ins=0xf002, InsMask=0xff1f, Op1Mask=0x00f0, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BL_AD, Name="bl", Length=2, Operands=1, Flags=0b000000, Ins=0xf001, InsMask=0xf0ff, Op1Mask=0x0f00, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_BL_ER, Name="bl", Length=1, Operands=1, Flags=0b000000, Ins=0xf003, InsMask=0xf00f, Op1Mask=0x00f0, Op2Mask=0x0000},

	    // Multiplication and division instructions
		    new U8Instruction() { Id=U8Decoder.U8_MUL_ER, Name="mul", Length=1, Operands=2, Flags=0b010000, Ins=0xf004, InsMask=0xf10f, Op1Mask=0x0f00, Op2Mask=0x00f0},
            new U8Instruction() { Id=U8Decoder.U8_DIV_ER, Name="div", Length=1, Operands=2, Flags=0b110000, Ins=0xf009, InsMask=0xf10f, Op1Mask=0x0f00, Op2Mask=0x00f0},

	    // Miscellaneous
		    new U8Instruction() { Id=U8Decoder.U8_INC_EA, Name="inc", Length=1, Operands=0, Flags=0b011101, Ins=0xfe2f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_DEC_EA, Name="dec", Length=1, Operands=0, Flags=0b011101, Ins=0xfe3f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_RT, Name="rt", Length=1, Operands=0, Flags=0b000000, Ins=0xfe1f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_RTI, Name="rti", Length=1, Operands=0, Flags=0b111111, Ins=0xfe0f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_NOP, Name="nop", Length=1, Operands=0, Flags=0b000000, Ins=0xfe8f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},

	    // DSR prefix 'instructions'
		    new U8Instruction() { Id=U8Decoder.U8_PRE_PSEG, Name="dsr", Length=2, Operands=1, Flags=0b011000, Ins=0xe300, InsMask=0xff00, Op1Mask=0x00ff, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PRE_DSR, Name="dsr", Length=2, Operands=0, Flags=0b011000, Ins=0xfe9f, InsMask=0xffff, Op1Mask=0x0000, Op2Mask=0x0000},
            new U8Instruction() { Id=U8Decoder.U8_PRE_R, Name="dsr", Length=2, Operands=1, Flags=0b011000, Ins=0x900f, InsMask=0xff0f, Op1Mask=0x00f0, Op2Mask=0x0000},

            new U8Instruction() { Id=U8Decoder.U8_ILL, Name="dw", Length=1, Operands=0, Flags=0b000000, Ins=0xffff, InsMask=0x0000, Op1Mask=0x0000, Op2Mask=0x0000}
        };

        public static U8UserDefinition[] Definition =
        {
            new U8UserDefinition() { Name = "addc", Description = "Addition with carry"},
            new U8UserDefinition() { Name = "cmpc", Description = "Comparison with carry"},
            new U8UserDefinition() { Name = "and", Description = "Bitwise AND"},
            new U8UserDefinition() { Name = "or", Description = "Bitwise OR"},
            new U8UserDefinition() { Name = "xor", Description = "Bitwise exlusive OR"},
            new U8UserDefinition() { Name = "subc", Description = "Subtract with carry"},
            new U8UserDefinition() { Name = "sub", Description = "Subtract"},
            new U8UserDefinition() { Name = "cmp", Description = "Comparison"},
            new U8UserDefinition() { Name = "sll", Description = "Byte-sized shit left logical"},
            new U8UserDefinition() { Name = "sllc", Description = "Shift left logical continued"},
            new U8UserDefinition() { Name = "sra", Description = "Shift right arithmetic"},
            new U8UserDefinition() { Name = "srl", Description = "Shift right logical"},
            new U8UserDefinition() { Name = "srlc", Description = "Shift right logical continued"},
            new U8UserDefinition() { Name = "l", Description = "data transfer"},
            new U8UserDefinition() { Name = "st", Description = "data transfer"},
            new U8UserDefinition() { Name = "add", Description = "Addition"},
            new U8UserDefinition() { Name = "push", Description = "General register save"},
            new U8UserDefinition() { Name = "pop", Description = "General register restore"},
            new U8UserDefinition() { Name = "mov", Description = "Data transfer"},
            new U8UserDefinition() { Name = "lea", Description = "Data transfer to EA"},
            new U8UserDefinition() { Name = "daa", Description = "Byte-sized decimal adjustment for addition"},
            new U8UserDefinition() { Name = "das", Description = "Byte-sized decimal adjustment for subtraction"},
            new U8UserDefinition() { Name = "neg", Description = "Negate"},
            new U8UserDefinition() { Name = "sb", Description = "Set bit"},
            new U8UserDefinition() { Name = "rb", Description = "Reset bit"},
            new U8UserDefinition() { Name = "tb", Description = "Test bit"},
            new U8UserDefinition() { Name = "ei", Description = "Enable interrupts"},
            new U8UserDefinition() { Name = "di", Description = "Disable interrupts"},
            new U8UserDefinition() { Name = "sc", Description = "Set carry flag"},
            new U8UserDefinition() { Name = "rc", Description = "Reset carry flag"},
            new U8UserDefinition() { Name = "cplc", Description = "Complement carry flag"},
            new U8UserDefinition() { Name = "bc", Description = "Conditional branch"},
            new U8UserDefinition() { Name = "extbw", Description = "Extend sign"},
            new U8UserDefinition() { Name = "swi", Description = "Software interrupt instruction"},
            new U8UserDefinition() { Name = "brk", Description = "Break instruction"},
            new U8UserDefinition() { Name = "b", Description = "Branch Instruction"},
            new U8UserDefinition() { Name = "bl", Description = "Branch Instruction"},
            new U8UserDefinition() { Name = "mul", Description = "Multiplication"},
            new U8UserDefinition() { Name = "div", Description = "Division"},
            new U8UserDefinition() { Name = "inc", Description = "Memory increment"},
            new U8UserDefinition() { Name = "dec", Description = "Memory decrement"},
            new U8UserDefinition() { Name = "rt", Description = "Return from subroutine"},
            new U8UserDefinition() { Name = "rti", Description = "Return from interrupt"},
            new U8UserDefinition() { Name = "nop", Description = "No-operation"}
        };

        // NOTEs:
        /*
            PC - Program Counter
            CSR - Code Segment Regsiter
            LR, ELR1, ELR2, ELR3 - Link Registers
            LCSR, ECSR1, ECSR2, ECSR3 - CSR Backup Registers
            EPSW1, EPSW2, EPSW3 - PSW Backup Registers
            SP - Stack Pointer
            EA - EA Register
            AR - Address Register
            DSR - Data Segment Register
            NMI - Nonmaskable Interrupts
            MI - Maskable Interrupts
            SWI - Software Interrupts
         */

        // 0:0000h ~ 0:00FEh = Vector Region
        // #0
        // 00 ~ 04 = Reset Vectors
        // -> 00: init value of SP, 02: reset routune entry for extern/BRK with ELEVEL 2/3, 04: reset routine entry for BRK with ELEVEL 0/1
        // 06 ~ 7E = Hardware interrupt vectors
        // -> 06: NMICE interrupt, 08: NMI interrupt + maskable * 59
        // 80 ~ FE = Software interrupt vectors
        // -> SWI #0 ~ SWI #63
        // 0100: entry point?
    }
}
