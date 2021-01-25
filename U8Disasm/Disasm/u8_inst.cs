using System;

namespace u8_lib.Disasm
{
    public struct u8inst_t
    {
        public uint id;
        public string name;
        public int len;
        public int ops;

        public byte flags;
        public UInt16 ins;
        public UInt16 ins_mask;
        public UInt16 op1_mask;
        public UInt16 op2_mask;

        public UInt16 prefix;
    }

    public struct u8_cmd
    {
        public int address;
        public int type;        // index in instruction table
        public UInt16 opcode;   // instruction word
        public UInt16 op1;      // first decoded operand
        public UInt16 op2;      // second decoded operand
        public UInt16 s_word;   // optional second data word

        // String of assembly operation mnemonic.
        public string instr;

        // String of formatted operands.
        public string operands;
    };

    public struct u8_usrdef
    {
        public string name;
        public string description; // gives a bit more detail/full name
    }

    public static class u8_inst
    {
        // Thanks : https://github.com/ferib/u8_r2_plugin/blob/master/u8_inst.c
        public static u8inst_t[] u8inst =
        {
            // Arithmetic instructions
		    new u8inst_t() { id=u8_disas.U8_ADD_R, name="add", len=1, ops=2, flags=0b111101, ins=0x8001, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ADD_O, name="add", len=1, ops=2, flags=0b111101, ins=0x1000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_ADD_ER, name="add", len=1, ops=2, flags=0b111101, ins=0xf006, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ADD_ER_O, name="add", len=1, ops=2, flags=0b111101, ins=0xe080, ins_mask=0xf180, op1_mask=0x0f00, op2_mask=0x007f},
            new u8inst_t() { id=u8_disas.U8_ADDC_R, name="addc", len=1, ops=2, flags=0b111101, ins=0x8006, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ADDC_O, name="addc", len=1, ops=2, flags=0b111101, ins=0x6000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_AND_R, name="and", len=1, ops=2, flags=0b011000, ins=0x8002, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_AND_O, name="and", len=1, ops=2, flags=0b011000, ins=0x2000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_CMP_R, name="cmp", len=1, ops=2, flags=0b111101, ins=0x8007, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_CMP_O, name="cmp", len=1, ops=2, flags=0b111101, ins=0x7000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_CMPC_R, name="cmpc", len=1, ops=2, flags=0b111101, ins=0x8005, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_CMPC_O, name="cmpc", len=1, ops=2, flags=0b111101, ins=0x5000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_MOV_ER, name="mov", len=1, ops=2, flags=0b011000, ins=0xf005, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_MOV_ER_O, name="mov", len=1, ops=2, flags=0b011000, ins=0xe000, ins_mask=0xf180, op1_mask=0x0f00, op2_mask=0x007f},
            new u8inst_t() { id=u8_disas.U8_MOV_R, name="mov", len=1, ops=2, flags=0b011000, ins=0x8000, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_MOV_O, name="mov", len=1, ops=2, flags=0b011000, ins=0x0000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_OR_R, name="or", len=1, ops=2, flags=0b011000, ins=0x8003, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_OR_O, name="or", len=1, ops=2, flags=0b011000, ins=0x3000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_XOR_R, name="xor", len=1, ops=2, flags=0b011000, ins=0x8004, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_XOR_O, name="xor", len=1, ops=2, flags=0b011000, ins=0x4000, ins_mask=0xf000, op1_mask=0x0f00, op2_mask=0x00ff},
            new u8inst_t() { id=u8_disas.U8_CMP_ER, name="cmp", len=1, ops=2, flags=0b111101, ins=0xf007, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SUB_R, name="sub", len=1, ops=2, flags=0b111101, ins=0x8008, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SUBC_R, name="subc", len=1, ops=2, flags=0b111101, ins=0x8009, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},

	    // Shift instructions
		    new u8inst_t() { id=u8_disas.U8_SLL_R, name="sll", len=1, ops=2, flags=0b100000, ins=0x800a, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SLL_O, name="sll", len=1, ops=2, flags=0b100000, ins=0x900a, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
            new u8inst_t() { id=u8_disas.U8_SLLC_R, name="sllc", len=1, ops=2, flags=0b100000, ins=0x800b, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SLLC_O, name="sllc", len=1, ops=2, flags=0b100000, ins=0x900b, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
            new u8inst_t() { id=u8_disas.U8_SRA_R, name="sra", len=1, ops=2, flags=0b100000, ins=0x800e, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SRA_O, name="sra", len=1, ops=2, flags=0b100000, ins=0x900e, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
            new u8inst_t() { id=u8_disas.U8_SRL_R, name="srl", len=1, ops=2, flags=0b100000, ins=0x800c, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SRL_O, name="srl", len=1, ops=2, flags=0b100000, ins=0x900c, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
            new u8inst_t() { id=u8_disas.U8_SRLC_R, name="srlc", len=1, ops=2, flags=0b100000, ins=0x800d, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_SRLC_O, name="srlc", len=1, ops=2, flags=0b100000, ins=0x900d, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},

	    // Load/store instructions
		    new u8inst_t() { id=u8_disas.U8_L_ER_EA, name="l", len=1, ops=1, flags=0b011000, ins=0x9032, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_ER_EAP, name="l", len=1, ops=1, flags=0b011000, ins=0x9052, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_ER_ER, name="l", len=1, ops=2, flags=0b011000, ins=0x9002, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_L_ER_D16_ER, name="l", len=2, ops=2, flags=0b011000, ins=0xa008, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_L_ER_D6_BP, name="l", len=1, ops=2, flags=0b011000, ins=0xb000, ins_mask=0xf1c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_L_ER_D6_FP, name="l", len=1, ops=2, flags=0b011000, ins=0xb040, ins_mask=0xf1c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_L_ER_DA, name="l", len=2, ops=1, flags=0b011000, ins=0x9012, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_R_EA, name="l", len=1, ops=1, flags=0b011000, ins=0x9030, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_R_EAP, name="l", len=1, ops=1, flags=0b011000, ins=0x9050, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_R_ER, name="l", len=1, ops=2, flags=0b011000, ins=0x9000, ins_mask=0xf01f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_L_R_D16_ER, name="l", len=2, ops=2, flags=0b011000, ins=0x9008, ins_mask=0xf01f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_L_R_D6_BP, name="l", len=1, ops=2, flags=0b011000, ins=0xd000, ins_mask=0xf0c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_L_R_D6_FP, name="l", len=1, ops=2, flags=0b011000, ins=0xd040, ins_mask=0xf0c0, op1_mask=0x0f00, op2_mask=0x003f},
	    // Per core ref.:
	    //{.id=U8_L_R_DA, name="l", len=2, ops=1, flags=0b011000, ins=0x9010, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
	    // ....but, we match SDK disassembler behaviour, w.r.t. third nibble:
		    new u8inst_t() { id=u8_disas.U8_L_R_DA, name="l", len=2, ops=1, flags=0b011000, ins=0x9010, ins_mask=0xf01f, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_XR_EA, name="l", len=1, ops=1, flags=0b011000, ins=0x9034, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_XR_EAP, name="l", len=1, ops=1, flags=0b011000, ins=0x9054, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_QR_EA, name="l", len=1, ops=1, flags=0b011000, ins=0x9036, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_L_QR_EAP, name="l", len=1, ops=1, flags=0b011000, ins=0x9056, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_ER_EA, name="st", len=1, ops=1, flags=0b000000, ins=0x9033, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_ER_EAP, name="st", len=1, ops=1, flags=0b000000, ins=0x9053, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_ER_ER, name="st", len=1, ops=2, flags=0b000000, ins=0x9003, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ST_ER_D16_ER, name="st", len=2, ops=2, flags=0b000000, ins=0xa009, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ST_ER_D6_BP, name="st", len=1, ops=2, flags=0b000000, ins=0xb080, ins_mask=0xf1c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_ST_ER_D6_FP, name="st", len=1, ops=2, flags=0b000000, ins=0xb0c0, ins_mask=0xf1c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_ST_ER_DA, name="st", len=2, ops=1, flags=0b000000, ins=0x9013, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_R_EA, name="st", len=1, ops=1, flags=0b000000, ins=0x9031, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_R_EAP, name="st", len=1, ops=1, flags=0b000000, ins=0x9051, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_R_ER, name="st", len=1, ops=2, flags=0b000000, ins=0x9001, ins_mask=0xf01f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ST_R_D16_ER, name="st", len=2, ops=2, flags=0b000000, ins=0x9009, ins_mask=0xf01f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_ST_R_D6_BP, name="st", len=1, ops=2, flags=0b000000, ins=0xd080, ins_mask=0xf0c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_ST_R_D6_FP, name="st", len=1, ops=2, flags=0b000000, ins=0xd0c0, ins_mask=0xf0c0, op1_mask=0x0f00, op2_mask=0x003f},
            new u8inst_t() { id=u8_disas.U8_ST_R_DA, name="st", len=2, ops=1, flags=0b000000, ins=0x9011, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_XR_EA, name="st", len=1, ops=1, flags=0b000000, ins=0x9035, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_XR_EAP, name="st", len=1, ops=1, flags=0b000000, ins=0x9055, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_QR_EA, name="st", len=1, ops=1, flags=0b000000, ins=0x9037, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_ST_QR_EAP, name="st", len=1, ops=1, flags=0b000000, ins=0x9057, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},

	    // Control register access instructions
		    new u8inst_t() { id=u8_disas.U8_ADD_SP_O, name="add", len=1, ops=1, flags=0b000000, ins=0xe100, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_ECSR_R, name="mov", len=1, ops=1, flags=0b000000, ins=0xa00f, ins_mask=0xff0f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_ELR_ER, name="mov", len=1, ops=1, flags=0b000000, ins=0xa00d, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EPSW_R, name="mov", len=1, ops=1, flags=0b000000, ins=0xa00c, ins_mask=0xff0f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_ER_ELR, name="mov", len=1, ops=1, flags=0b000000, ins=0xa005, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_ER_SP, name="mov", len=1, ops=1, flags=0b000000, ins=0xa01a, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_PSW_R, name="mov", len=1, ops=1, flags=0b111111, ins=0xa00b, ins_mask=0xff0f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_PSW_O, name="mov", len=1, ops=1, flags=0b111111, ins=0xa00b, ins_mask=0xff0f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_R_ECSR, name="mov", len=1, ops=1, flags=0b000000, ins=0xa007, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_R_EPSW, name="mov", len=1, ops=1, flags=0b000000, ins=0xa004, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_R_PSW, name="mov", len=1, ops=1, flags=0b000000, ins=0xa003, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_SP_ER, name="mov", len=1, ops=1, flags=0b000000, ins=0xa10a, ins_mask=0xff1f, op1_mask=0x00f0, op2_mask=0x0000},

	    // Push/pop instructions
		    new u8inst_t() { id=u8_disas.U8_PUSH_ER, name="push", len=1, ops=1, flags=0b000000, ins=0xf05e, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PUSH_QR, name="push", len=1, ops=1, flags=0b000000, ins=0xf07e, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PUSH_R, name="push", len=1, ops=1, flags=0b000000, ins=0xf04e, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PUSH_XR, name="push", len=1, ops=1, flags=0b000000, ins=0xf06e, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PUSH_RL, name="push", len=1, ops=1, flags=0b000000, ins=0xf0ce, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_POP_ER, name="pop", len=1, ops=1, flags=0b000000, ins=0xf01e, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_POP_QR, name="pop", len=1, ops=1, flags=0b000000, ins=0xf03e, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_POP_R, name="pop", len=1, ops=1, flags=0b000000, ins=0xf00e, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_POP_XR, name="pop", len=1, ops=1, flags=0b000000, ins=0xf02e, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_POP_RL, name="pop", len=1, ops=1, flags=0b111111, ins=0xf08e, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},

	    // Coprocessor data transfer instructions
		    new u8inst_t() { id=u8_disas.U8_MOV_CR_R, name="mov", len=1, ops=2, flags=0b000000, ins=0xa00e, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_MOV_CER_EA, name="mov", len=1, ops=1, flags=0b000000, ins=0xf02d, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CER_EAP, name="mov", len=1, ops=1, flags=0b000000, ins=0xf03d, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CR_EA, name="mov", len=1, ops=1, flags=0b000000, ins=0xf00d, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CR_EAP, name="mov", len=1, ops=1, flags=0b000000, ins=0xf01d, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CXR_EA, name="mov", len=1, ops=1, flags=0b000000, ins=0xf04d, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CXR_EAP, name="mov", len=1, ops=1, flags=0b000000, ins=0xf05d, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CQR_EA, name="mov", len=1, ops=1, flags=0b000000, ins=0xf06d, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_CQR_EAP, name="mov", len=1, ops=1, flags=0b000000, ins=0xf07d, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_R_CR, name="mov", len=1, ops=2, flags=0b000000, ins=0xa006, ins_mask=0xf00f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_MOV_EA_CER, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0ad, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EAP_CER, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0bd, ins_mask=0xf1ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EA_CR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf08d, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EAP_CR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf09d, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EA_CXR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0cd, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EAP_CXR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0dd, ins_mask=0xf3ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EA_CQR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0ed, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_MOV_EAP_CQR, name="mov", len=1, ops=1, flags=0b000000, ins=0xf0fd, ins_mask=0xf7ff, op1_mask=0x0f00, op2_mask=0x0000},

	    // EA register data transfer instructions
		    new u8inst_t() { id=u8_disas.U8_LEA_ER, name="lea", len=1, ops=1, flags=0b000000, ins=0xf00a, ins_mask=0xf01f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_LEA_D16_ER, name="lea", len=2, ops=1, flags=0b000000, ins=0xf00b, ins_mask=0xf01f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_LEA_DA, name="lea", len=2, ops=1, flags=0b000000, ins=0xf00c, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},

	    // ALU Instructions
		    new u8inst_t() { id=u8_disas.U8_DAA_R, name="daa", len=1, ops=1, flags=0b111001, ins=0x801f, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_DAS_R, name="das", len=1, ops=1, flags=0b111001, ins=0x803f, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_NEG_R, name="neg", len=1, ops=1, flags=0b111101, ins=0x805f, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},

	    // Bit access instructions
		    new u8inst_t() { id=u8_disas.U8_SB_R, name="sb", len=1, ops=2, flags=0b010000, ins=0xa000, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
            new u8inst_t() { id=u8_disas.U8_SB_DBIT, name="sb", len=2, ops=1, flags=0b010000, ins=0xa080, ins_mask=0xff8f, op1_mask=0x0070, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_RB_R, name="rb", len=1, ops=2, flags=0b010000, ins=0xa002, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
	    // Per core ref.:
    //	{.id=U8_RB_DBIT, name="rb", len=2, ops=1, flags=0b010000, ins=0xa082, ins_mask=0xff8f, op1_mask=0x0070, op2_mask=0x0000},
	    // ....but, we match SDK disassembler behaviour, w.r.t. second nibble:
		    new u8inst_t() { id=u8_disas.U8_RB_DBIT, name="rb", len=2, ops=1, flags=0b010000, ins=0xa082, ins_mask=0xf08f, op1_mask=0x0070, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_TB_R, name="tb", len=1, ops=2, flags=0b010000, ins=0xa001, ins_mask=0xf08f, op1_mask=0x0f00, op2_mask=0x0070},
    //	{.id=U8_TB_DBIT, name="tb", len=2, ops=1, flags=0b010000, ins=0xa081, ins_mask=0xff8f, op1_mask=0x0070, op2_mask=0x0000},
		    new u8inst_t() { id=u8_disas.U8_TB_DBIT, name="tb", len=2, ops=1, flags=0b010000, ins=0xa081, ins_mask=0xf08f, op1_mask=0x0070, op2_mask=0x0000},

	    // PSW access instructions
		    new u8inst_t() { id=u8_disas.U8_EI, name="ei", len=1, ops=0, flags=0b000010, ins=0xed08, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_DI, name="di", len=1, ops=0, flags=0b000010, ins=0xebf7, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_SC, name="sc", len=1, ops=0, flags=0b100000, ins=0xed80, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_RC, name="rc", len=1, ops=0, flags=0b100000, ins=0xeb7f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_CPLC, name="cplc", len=1, ops=0, flags=0b100000, ins=0xfecf, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},

	    // Conditional relative branch instructions
		    new u8inst_t() { id=u8_disas.U8_BGE_RAD, name="bge", len=1, ops=1, flags=0b000000, ins=0xc000, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BLT_RAD, name="blt", len=1, ops=1, flags=0b000000, ins=0xc100, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BGT_RAD, name="bgt", len=1, ops=1, flags=0b000000, ins=0xc200, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BLE_RAD, name="ble", len=1, ops=1, flags=0b000000, ins=0xc130, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BGES_RAD, name="bges", len=1, ops=1, flags=0b000000, ins=0xc400, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BLTS_RAD, name="blts", len=1, ops=1, flags=0b000000, ins=0xc500, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BGTS_RAD, name="bgts", len=1, ops=1, flags=0b000000, ins=0xc600, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BLES_RAD, name="bles", len=1, ops=1, flags=0b000000, ins=0xc700, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BNE_RAD, name="bne", len=1, ops=1, flags=0b000000, ins=0xc800, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BEQ_RAD, name="beq", len=1, ops=1, flags=0b000000, ins=0xc900, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BNV_RAD, name="bnv", len=1, ops=1, flags=0b000000, ins=0xca00, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BOV_RAD, name="bov", len=1, ops=1, flags=0b000000, ins=0xcb00, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BPS_RAD, name="bps", len=1, ops=1, flags=0b000000, ins=0xcc00, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BNS_RAD, name="bns", len=1, ops=1, flags=0b000000, ins=0xcd00, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BAL_RAD, name="bal", len=1, ops=1, flags=0b000000, ins=0xce00, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},

	    // Sign extension instruction
		    new u8inst_t() { id=u8_disas.U8_EXTBW_ER, name="extbw", len=1, ops=2, flags=0b011000, ins=0x810f, ins_mask=0xf11f, op1_mask=0x0f00, op2_mask=0x00f0},

	    // Software interrupt instructions
		    new u8inst_t() { id=u8_disas.U8_SWI_O, name="swi", len=1, ops=1, flags=0b000010, ins=0xe500, ins_mask=0xffc0, op1_mask=0x003f, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BRK, name="brk", len=1, ops=0, flags=0b000000, ins=0xffff, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},

	    // Branch instructions
		    new u8inst_t() { id=u8_disas.U8_B_AD, name="b", len=2, ops=1, flags=0b000000, ins=0xf000, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_B_ER, name="b", len=1, ops=1, flags=0b000000, ins=0xf002, ins_mask=0xff1f, op1_mask=0x00f0, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BL_AD, name="bl", len=2, ops=1, flags=0b000000, ins=0xf001, ins_mask=0xf0ff, op1_mask=0x0f00, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_BL_ER, name="bl", len=1, ops=1, flags=0b000000, ins=0xf003, ins_mask=0xf00f, op1_mask=0x00f0, op2_mask=0x0000},

	    // Multiplication and division instructions
		    new u8inst_t() { id=u8_disas.U8_MUL_ER, name="mul", len=1, ops=2, flags=0b010000, ins=0xf004, ins_mask=0xf10f, op1_mask=0x0f00, op2_mask=0x00f0},
            new u8inst_t() { id=u8_disas.U8_DIV_ER, name="div", len=1, ops=2, flags=0b110000, ins=0xf009, ins_mask=0xf10f, op1_mask=0x0f00, op2_mask=0x00f0},

	    // Miscellaneous
		    new u8inst_t() { id=u8_disas.U8_INC_EA, name="inc", len=1, ops=0, flags=0b011101, ins=0xfe2f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_DEC_EA, name="dec", len=1, ops=0, flags=0b011101, ins=0xfe3f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_RT, name="rt", len=1, ops=0, flags=0b000000, ins=0xfe1f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_RTI, name="rti", len=1, ops=0, flags=0b111111, ins=0xfe0f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_NOP, name="nop", len=1, ops=0, flags=0b000000, ins=0xfe8f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},

	    // DSR prefix 'instructions'
		    new u8inst_t() { id=u8_disas.U8_PRE_PSEG, name="dsr", len=2, ops=1, flags=0b011000, ins=0xe300, ins_mask=0xff00, op1_mask=0x00ff, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PRE_DSR, name="dsr", len=2, ops=0, flags=0b011000, ins=0xfe9f, ins_mask=0xffff, op1_mask=0x0000, op2_mask=0x0000},
            new u8inst_t() { id=u8_disas.U8_PRE_R, name="dsr", len=2, ops=1, flags=0b011000, ins=0x900f, ins_mask=0xff0f, op1_mask=0x00f0, op2_mask=0x0000},

            new u8inst_t() { id=u8_disas.U8_ILL, name="dw", len=1, ops=0, flags=0b000000, ins=0xffff, ins_mask=0x0000, op1_mask=0x0000, op2_mask=0x0000}
        };

        public static u8_usrdef[] u8userdefinitions =
        {
            new u8_usrdef() { name = "addc", description = "Addition with carry"},
            new u8_usrdef() { name = "cmpc", description = "Comparison with carry"},
            new u8_usrdef() { name = "and", description = "Bitwise AND"},
            new u8_usrdef() { name = "or", description = "Bitwise OR"},
            new u8_usrdef() { name = "xor", description = "Bitwise exlusive OR"},
            new u8_usrdef() { name = "subc", description = "Subtract with carry"},
            new u8_usrdef() { name = "sub", description = "Subtract"},
            new u8_usrdef() { name = "cmp", description = "Comparison"},
            new u8_usrdef() { name = "sll", description = "Byte-sized shit left logical"},
            new u8_usrdef() { name = "sllc", description = "Shift left logical continued"},
            new u8_usrdef() { name = "sra", description = "Shift right arithmetic"},
            new u8_usrdef() { name = "srl", description = "Shift right logical"},
            new u8_usrdef() { name = "srlc", description = "Shift right logical continued"},
            new u8_usrdef() { name = "l", description = "data transfer"},
            new u8_usrdef() { name = "st", description = "data transfer"},
            new u8_usrdef() { name = "add", description = "Addition"},
            new u8_usrdef() { name = "push", description = "General register save"},
            new u8_usrdef() { name = "pop", description = "General register restore"},
            new u8_usrdef() { name = "mov", description = "Data transfer"},
            new u8_usrdef() { name = "lea", description = "Data transfer to EA"},
            new u8_usrdef() { name = "daa", description = "Byte-sized decimal adjustment for addition"},
            new u8_usrdef() { name = "das", description = "Byte-sized decimal adjustment for subtraction"},
            new u8_usrdef() { name = "neg", description = "Negate"},
            new u8_usrdef() { name = "sb", description = "Set bit"},
            new u8_usrdef() { name = "rb", description = "Reset bit"},
            new u8_usrdef() { name = "tb", description = "Test bit"},
            new u8_usrdef() { name = "ei", description = "Enable interrupts"},
            new u8_usrdef() { name = "di", description = "Disable interrupts"},
            new u8_usrdef() { name = "sc", description = "Set carry flag"},
            new u8_usrdef() { name = "rc", description = "Reset carry flag"},
            new u8_usrdef() { name = "cplc", description = "Complement carry flag"},
            new u8_usrdef() { name = "bc", description = "Conditional branch"},
            new u8_usrdef() { name = "extbw", description = "Extend sign"},
            new u8_usrdef() { name = "swi", description = "Software interrupt instruction"},
            new u8_usrdef() { name = "brk", description = "Break instruction"},
            new u8_usrdef() { name = "b", description = "Branch Instruction"},
            new u8_usrdef() { name = "bl", description = "Branch Instruction"},
            new u8_usrdef() { name = "mul", description = "Multiplication"},
            new u8_usrdef() { name = "div", description = "Division"},
            new u8_usrdef() { name = "inc", description = "Memory increment"},
            new u8_usrdef() { name = "dec", description = "Memory decrement"},
            new u8_usrdef() { name = "rt", description = "Return from subroutine"},
            new u8_usrdef() { name = "rti", description = "Return from interrupt"},
            new u8_usrdef() { name = "nop", description = "No-operation"}
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
