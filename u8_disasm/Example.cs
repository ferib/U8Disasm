using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using U8Disasm.Structs;
using U8Disasm.Disassembler;
using U8Disasm.Analyser;

namespace u8_Console
{
    public class Example
    {
        public string FilePath;
        public byte[] Buffer;
        public U8Flow FlowAnalyses;

        // TODO: cache disassembled lines?
        public Dictionary<int, string> CachedDisassembly;

        public int Index; // current RIP-ish

        public Example(string Path)
        {
            if (!File.Exists(Path))
                return;
            this.FilePath = Path;
            this.Buffer = File.ReadAllBytes(this.FilePath);
            this.CachedDisassembly = new Dictionary<int, string>();
            this.Index = 0;
            this.FlowAnalyses = new U8Flow(this.Buffer);
        }

        private void _CacheDisassembly(int i, string Str)
        {
            if (this.CachedDisassembly.ContainsKey(i))
                this.CachedDisassembly[i] = Str;
            else
                this.CachedDisassembly.Add(i, Str);
        }

        public string[] Disassemble(int LineCount = 1, bool PrintLines = false)
        {
            int ret = 2; // word
            List<string> tmpResult = new List<string>();
            while (tmpResult.Count < LineCount)
            {
                string result = $"{ this.Index.ToString("X8")} ";

                // TODO: FIX THIS MESS WTF?
                int grabSize = 6;
                if ((this.Index + grabSize) - this.Buffer.Length > 0)
                    grabSize = (this.Index + grabSize) - this.Buffer.Length;
                byte[] buf = new byte[grabSize]; // FIX End of array error?

                // fill temp buff
                for (int b = 0; b < buf.Length; b++)
                    buf[b] = this.Buffer[this.Index + b];

                U8Cmd cmd = new U8Cmd();
                ret = U8Decoder.DecodeOpcode(buf, ref cmd);
                if (ret == -1)
                {
                    result += "err";
                    tmpResult.Add(result);
                    if (PrintLines)
                        PrintDisasm(result);
                    break;
                }

                this.Index += ret;

                string bytestr = "";
                for (int j = 0; j < ret; j += 2)
                    bytestr += BitConverter.ToUInt16(buf, j).ToString("X4") + " ";
                result += bytestr.PadRight(15) + cmd.Instruction.PadRight(10) + cmd.Operands;
                tmpResult.Add(result);

                _CacheDisassembly(this.Index, result);

                if (PrintLines)
                    PrintDisasm(result);
            }

            return tmpResult.ToArray();
        }


        // TODO: Port to GUI class
        private void PrintDisasm(string disasm)
        {
            ConsoleColor fold = Console.ForegroundColor;
            ConsoleColor bold = Console.BackgroundColor;
            // TODO: red for register name, blue for numbers, black for other
            //  green for labels/offseted labels?

            // TODO: add short explenation for what those stand for?
            string[] RedWords = { "ea", "r", "lr", "elr", "er", "xr", "qr", "sp", "csr", "ecsr", "psw", "epsw", "pc", "cr", "cer", "ea", "cxr", "cqr" };
            // everything that parses as Int 16 will be blue/cyan

            // disect string and find stuff to paint
            string[] splits = disasm.Split(new char[] { ',', ' ', ':', '-', '+', '#' }); // split using all possible prefixes

            // TODO: use class?
            List<ConsoleColor> StrColors = new List<ConsoleColor>();
            List<int> StrStart = new List<int>();
            List<int> StrEnd = new List<int>();

            int len = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                // start addr - Gray
                if (len == 0)
                {
                    StrColors.Add(ConsoleColor.DarkGray);
                    StrStart.Add(0);
                    StrEnd.Add(len + splits[i].Length);
                    len += splits[i].Length + 1;
                    continue;
                }

                // Opcodes - White
                bool hasMatch = false;
                foreach (var u8_inst in U8Instructions.Table)
                {
                    if (splits[i] == u8_inst.Name)
                    {
                        StrColors.Add(ConsoleColor.Cyan);
                        StrStart.Add(len - 1);
                        StrEnd.Add(len + splits[i].Length);
                        len += splits[i].Length + 1;
                        hasMatch = true;
                        break;
                    }
                }
                if (hasMatch)
                    continue;

                // Registers/Keywords - Red
                foreach (var rw in RedWords)
                {
                    if (splits[i].Contains(rw))
                    {
                        StrColors.Add(ConsoleColor.Magenta);
                        StrStart.Add(len - 1);
                        StrEnd.Add(len + splits[i].Length);
                        hasMatch = true;
                        len += splits[i].Length + 1;
                        break;
                    }
                }
                if (hasMatch)
                    continue;

                // Numeric - Blue
                int ires = 0;
                if (int.TryParse(splits[i].Replace("h", "").Replace("#", ""), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out ires))
                {
                    StrColors.Add(ConsoleColor.Yellow);
                    StrStart.Add(len - 1);
                    StrEnd.Add(len + splits[i].Length);
                    len += splits[i].Length + 1;
                    continue;
                }

                // Whatever is left - Gray?
                StrColors.Add(ConsoleColor.Gray);
                StrStart.Add(len - 1);
                StrEnd.Add(len + splits[i].Length);
                len += splits[i].Length + 1; // keep track of character index by counting length
            }

            // This gonna take a lot of syscals... 
            // But who cares about performance? not me, im writing this thing in freakn' C#!

            for (int i = 0; i < StrColors.Count; i++)
            {
                PrintC(disasm.Substring(StrStart[i], StrEnd[i] - StrStart[i]), StrColors[i]);
            }

            void PrintC(string str, ConsoleColor c)
            {
                Console.ForegroundColor = c;
                Console.Write(str);
            }

            Console.ForegroundColor = fold;
            Console.BackgroundColor = bold;
            Console.WriteLine();
        }
    }
}
