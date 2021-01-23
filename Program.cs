using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace u8_disasm
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] ROM = File.ReadAllBytes(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
            int ret = 2; // start?
            for(int i = 0; i < 1000; i+= ret)
            {
                string result = $"{i.ToString("X8")} ";
                byte[] buf = new byte[10];

                for (int b = 0; b < buf.Length; b++)
                    buf[b] = ROM[i+b];

                u8_cmd cmd = new u8_cmd();
                ret = asm_u8.dissaseble(ref buf, buf.Length, ref cmd);
                if (ret == -1)
                {
                    result += "err";
                    //Console.WriteLine(result);
                    PrintDisasm(result);
                    break;
                }

                string bytestr = "";
                for (int j = 0; j < ret; j += 2)
                    bytestr += BitConverter.ToUInt16(buf, j).ToString("X4") + " ";
                result += bytestr.PadRight(15) + cmd.instr.PadRight(10) + cmd.operands;
                //Console.WriteLine(result);
                PrintDisasm(result);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static void PrintDisasm(string disasm)
        {
            ConsoleColor fold = Console.ForegroundColor;
            ConsoleColor bold = Console.BackgroundColor;
            // TODO: red for register name, blue for numbers, black for other
            //  green for labels/offseted labels?

            // TODO: add short explenation for what those stand for?
            string[] RedWords = { "ea", "r", "lr", "elr", "er", "xr", "qr", "sp", "csr", "ecsr", "psw", "epsw", "pc", "cr", "cer", "ea", "cxr","cqr" };
            // everything that parses as Int 16 will be blue/cyan

            // disect string and find stuff to paint
            string[] splits = disasm.Split(new char[] { ',', ' ',':','-','+','#' }); // split using all possible prefixes

            // TODO: use class?
            List<ConsoleColor> StrColors = new List<ConsoleColor>();
            List<int> StrStart = new List<int>();
            List<int> StrEnd = new List<int>();

            int len = 0;
            for(int i = 0; i < splits.Length; i++)
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
                foreach(var rw in RedWords)
                {
                    if(splits[i].Contains(rw))
                    {
                        StrColors.Add(ConsoleColor.Magenta);
                        StrStart.Add(len - 1);
                        StrEnd.Add(len + splits[i].Length);
                        hasMatch = true;
                        len += splits[i].Length+1;
                        break;
                    }
                }
                if (hasMatch)
                    continue;

                // Numeric - Blue
                int ires = 0;
                if(int.TryParse(splits[i].Replace("h","").Replace("#", ""), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ires))
                {
                    StrColors.Add(ConsoleColor.Yellow);
                    StrStart.Add(len - 1);
                    StrEnd.Add(len + splits[i].Length);
                    len += splits[i].Length+1;
                    continue;
                }

                // Whatever is left - Gray?
                StrColors.Add(ConsoleColor.Gray);
                StrStart.Add(len-1);
                StrEnd.Add(len + splits[i].Length);
                len += splits[i].Length+1; // keep track of character index by counting length
            }

            // This gonna take a lot of syscals... 
            // But who cares about performance? not me, im writing this thing in freakn' C#!

            for(int i = 0; i < StrColors.Count; i++)
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
