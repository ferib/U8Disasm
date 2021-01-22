using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                string result = $"{i.ToString("X8")}: ";
                byte[] buf = new byte[10];

                for (int b = 0; b < buf.Length; b++)
                    buf[b] = ROM[i+b];

                u8_cmd cmd = new u8_cmd();
                ret = asm_u8.dissaseble(ref buf, buf.Length, ref cmd);
                if (ret == -1)
                {
                    result += "err";
                    Console.WriteLine(result);
                    break;
                }

                string bytestr = "";
                for (int j = 0; j < ret; j += 2)
                    bytestr += BitConverter.ToUInt16(buf, j).ToString("X4") + " ";
                result += bytestr.PadRight(15) + cmd.instr.PadRight(10) + cmd.operands;
                Console.WriteLine(result);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
