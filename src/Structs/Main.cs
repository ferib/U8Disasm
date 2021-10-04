using System;
using U8Disasm.Disassembler;

namespace U8Disasm.Structs
{
    public struct U8Instruction
    {
        public U8_OP Id;
        public string Name;
        public int Length;
        public int Operands;

        public byte Flags;
        public UInt16 Ins;
        public UInt16 InsMask;
        public UInt16 Op1Mask;
        public UInt16 Op2Mask;

        public UInt16 Prefix;
    }

    public struct U8Cmd
    {
        public int Address;     // location in memory
        public U8_OP Type;        // index in instruction table
        public UInt16 Opcode;   // instruction word
        public UInt16 Op1;      // first decoded operand
        public UInt16 Op2;      // second decoded operand
        public UInt16 sWord;    // optional second data word

        // String of assembly operation mnemonic.
        public string Instruction;

        // String of formatted operands.
        public string Operands;
    };

    // definition/descrpition for humans
    public struct U8UserDefinition
    {
        public string Name;
        public string Description; // gives a bit more detail/full name
    }
}
