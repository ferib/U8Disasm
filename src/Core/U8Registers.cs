using System;
using System.Collections.Generic;
using System.Text;

namespace U8Disasm.Core
{
    public class U8Registers
    {
        public byte r0 = 0;
        public byte r1 = 0;
        public byte r2 = 0;
        public byte r3 = 0;
        public byte r4 = 0;
        public byte r5 = 0;
        public byte r6 = 0;
        public byte r7 = 0;
        public byte r8 = 0;
        public byte r9 = 0;
        public byte r10 = 0;
        public byte r11 = 0;
        public byte r12 = 0;
        public byte r13 = 0;
        public byte r14 = 0;
        public byte r15 = 0;

        public ushort ER0
        {
            get
            {
                return (ushort)((r0 * 0x100) + r1);
            }
            set
            {
                r0 = (byte)(value / 0x100);
                r1 = (byte)(value);
            }
        }

        public ushort ER2
        {
            get
            {
                return (ushort)((r2 * 0x100) + r3);
            }
            set
            {
                r2 = (byte)(value / 0x100);
                r3 = (byte)(value);
            }
        }

        public ushort ER4
        {
            get
            {
                return (ushort)((r4 * 0x100) + r5);
            }
            set
            {
                r4 = (byte)(value / 0x100);
                r5 = (byte)(value);
            }
        }

        public ushort ER6
        {
            get
            {
                return (ushort)((r6 * 0x100) + r7);
            }
            set
            {
                r6 = (byte)(value / 0x100);
                r7 = (byte)(value);
            }
        }

        public ushort ER8
        {
            get
            {
                return (ushort)((r8 * 0x100) + r9);
            }
            set
            {
                r8 = (byte)(value / 0x100);
                r9 = (byte)(value);
            }
        }

        public ushort ER10
        {
            get
            {
                return (ushort)((r10 * 0x100) + r11);
            }
            set
            {
                r10 = (byte)(value / 0x100);
                r11 = (byte)(value);
            }
        }

        public ushort ER12 // BP, Base pointer
        {
            get
            {
                return (ushort)((r12 * 0x100) + r13);
            }
            set
            {
                r12 = (byte)(value / 0x100);
                r13 = (byte)(value);
            }
        }

        public ushort ER14 // FP, Frame pointer
        {
            get
            {
                return (ushort)((r14 * 0x100) + r15);
            }
            set
            {
                r14 = (byte)(value / 0x100);
                r15 = (byte)(value);
            }
        }

        public uint XR0
        {
            get
            {
                return (uint)((ER0 * 0x10000) + ER2);
            }set
            {
                ER0 = (ushort)(value / 0x10000);
                ER2 = (ushort)(value);
            }
        }

        public uint XR4
        {
            get
            {
                return (uint)((ER4 * 0x10000) + ER6);
            }
            set
            {
                ER4 = (ushort)(value / 0x10000);
                ER6 = (ushort)(value);
            }
        }

        public uint XR8
        {
            get
            {
                return (uint)((ER8 * 0x10000) + ER10);
            }
            set
            {
                ER8 = (ushort)(value / 0x10000);
                ER10 = (ushort)(value);
            }
        }

        public uint XR12
        {
            get
            {
                return (uint)((ER12 * 0x10000) + ER14);
            }
            set
            {
                ER12 = (ushort)(value / 0x10000);
                ER14 = (ushort)(value);
            }
        }

        public ulong QR0
        {
            get
            {
                return (ulong)((XR0 * 0x100000000) + XR4);
            }
            set
            {
                XR0 = (ushort)(value / 0x100000000);
                XR4 = (ushort)(value);
            }
        }

        public ulong QR8
        {
            get
            {
                return (ulong)((XR8 * 0x100000000) + XR12);
            }
            set
            {
                XR8 = (ushort)(value / 0x100000000);
                XR12 = (ushort)(value);
            }
        }


        // QR0 = [XR0 XR4] 32bit*2
        // XR0 = [ER0, ER2] 16bit*2
        // ER0 = [R0, R1] 8bit*2

        // ER12 = BP (Base pointer)
        // ER14 = FP (Frame pointer)

        public ushort PC;   // PC (Program Counter) 16bit
        public byte CSR;    // CSR (Code Segment register) 4bit
        public byte PSW;    // PSW(Program Status word) 8bit
        public ushort SP;   // SP (Stack pointer) 16bit (set to 00h~01h at start)

        public ushort LR;   // LR 16 bit (save PC on subroutine)
        public ushort LR1;  // LR1 ~ LR3 (Link register) 16bit
        public ushort LR2;
        public ushort LR3;

        public ushort EPSW1;    // EPSW1 ~ EPSW3 (PSW Backup register) 8bit
        public ushort EPSW2;
        public ushort EPSW3;

        public ushort EA;   // EA register 16bit
        public ushort AR;   // AR register 16bit (U8 Core Exclusive, no access for programs)
        public byte DSR;    // DSR (Data Segment Register) 8 bit

        //  PSW:
        //  7: C - Carry flag
        //  6: Z - Zero flag
        //  5: S - Sign flag
        //  4: OV - overflow flag
        //  3: MIE - Mask interrupt enable bit
        //  2: HC - Half carry flag
        //  1: ELEVEL[2:1] - Exception level
        //  0: ELEVEL[1:0]

        public U8Registers()
        {

        }

        public byte GetRegisterByIndex(byte Index)
        {
            byte[] regs = new byte[] { r0, r1, r2, r3, r4, r5, r6, r7, r8, r8, r10, r11, r12, r13, r14, r15 };
            return regs[Index];
        }

        public ushort GetERegisterByIndex(byte Index)
        {
            ushort[] regs = new ushort[] { ER0, ER2, ER4, ER6, ER8, ER10, ER12, ER14 };
            return regs[Index];
        }

        public uint GetXRegisterByIndex(byte Index)
        {
            uint[] regs = new uint[]{ XR0, XR4, XR8, XR12};
            return regs[Index];
        }

        public ulong GetQRegisterByIndex(byte Index)
        {
            ulong[] regs = new ulong[] { QR0, QR8 };
            return regs[Index];
        }

        public void SetRegisterByIndex(byte Index, byte value)
        {
            byte[] regs = new byte[] { r0, r1, r2, r3, r4, r5, r6, r7, r8, r8, r10, r11, r12, r13, r14, r15 };
            regs[Index] = value;
        }

        public void SetERegisterByIndex(byte Index, ushort value)
        {
            ushort[] regs = new ushort[] { ER0, ER2, ER4, ER6, ER8, ER10, ER12, ER14 };
            regs[Index] = value;
        }

        public void SetXRegisterByIndex(byte Index, ushort value)
        {
            uint[] regs = new uint[] { XR0, XR4, XR8, XR12 };
            regs[Index] = value;
        }

        public void SetQRegisterByIndex(byte Index, ushort value)
        {
            ulong[] regs = new ulong[] { QR0, QR8 };
            regs[Index] = value;
        }

    }
}
