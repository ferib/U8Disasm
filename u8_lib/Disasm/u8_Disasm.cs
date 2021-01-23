using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace u8_lib.Disasm
{
    public class u8_Disasm
    {
        public string FilePath;
        public byte[] Buffer;
        public u8_Flow FlowAnalyses;

        // TODO: cache disassembled lines?
        public Dictionary<int, string> CachedDisassembly;

        public int Index; // current RIP-ish

        public u8_Disasm(string path)
        {
            if (!File.Exists(path))
                return;
            this.FilePath = path;
            this.Buffer = File.ReadAllBytes(this.FilePath);
            this.CachedDisassembly = new Dictionary<int, string>();
            this.Index = 0;
            this.FlowAnalyses = new u8_Flow(this);
            this.FlowAnalyses.Analyse();
        }

        private void _CacheDisassembly(int i, string str)
        {
            if (this.CachedDisassembly.ContainsKey(i))
                this.CachedDisassembly[i] = str;
            else
                this.CachedDisassembly.Add(i, str);
        }

        public string[] Disassemble(int lineCount = 1, bool printLines = false)
        {
            int ret = 2; // word
            List<string> tmpResult = new List<string>(); 
            while(tmpResult.Count < lineCount)
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

                u8_cmd cmd = new u8_cmd();
                ret = asm_u8.dissaseble(ref buf, buf.Length, ref cmd);
                if (ret == -1)
                {
                    result += "err";
                    tmpResult.Add(result);
                    if (printLines)
                        PrintDisasm(result);
                    break;
                }

                this.Index += ret;

                string bytestr = "";
                for (int j = 0; j < ret; j += 2)
                    bytestr += BitConverter.ToUInt16(buf, j).ToString("X4") + " ";
                result += bytestr.PadRight(15) + cmd.instr.PadRight(10) + cmd.operands;
                tmpResult.Add(result);

                _CacheDisassembly(this.Index, result); 

                if (printLines)
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
                foreach (var u8_inst in u8_inst.u8inst)
                {
                    if (splits[i] == u8_inst.name)
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
