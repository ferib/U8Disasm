using System;
using System.Collections.Generic;
using System.Text;
using U8Disasm.Disassembler;
using U8Disasm.Structs;
using U8Disasm.Analyser;

namespace U8Disasm.Decompiler
{
    // NOTE: this compiler is based on the following paper: https://core.ac.uk/download/pdf/145692172.pdf

    // Register:
    //
    //  r0 ~ r15, 8 bit
    //  
    //  CSR (Code Segment register) 4bit
    //  PSW (Program Status word) 8bit
    //  SP (Stack pointer) 16bit (set to 00h~01h at start)
    //  LR1 ~ LR3 (Link register) 16bit
    //  LR 16 bit (save PC on subroutine)
    //  EPSW1 ~ EPSW3 (PSW Backup register) 8bit
    //  EA register 16bit
    //  AR register 16bit (U8 Core Exclusive, no access for programs)
    //  DSR (Data Segment Register) 8 bit
    //
    // QR0 = [XR0 XR4] 32bit*2
    // XR0 = [ER0, ER2] 16bit*2
    // ER0 = [R0, R1] 8bit*2

    // ER12 = BP (Base pointer)
    // ER14 = FP (Frame pointer)

    // Control Register:
    //  PSW:
    //  7: C - Carry flag
    //  6: Z - Zero flag
    //  5: S - Sign flag
    //  4: OV - overflow flag
    //  3: MIE - Mask interrupt enable bit
    //  2: HC - Half carry flag
    //  1: ELEVEL[2:1] - Exception level
    //  0: ELEVEL[1:0]

    public class U8Decompiler
    {
        private U8Stack U8Stack;
        private U8Graph U8Graph;
        private U8Output U8Output;

        public List<U8CodeBlock> Subroutine; // the thing we about to analyse

        public U8Decompiler(ref List<U8CodeBlock> sub)
        {
            this.U8Stack = new U8Stack(ref sub);
            this.U8Graph = new U8Graph(ref sub);
            this.U8Output = new U8Output();
        }

        public void Decompile()
        {

        }
    }
}
