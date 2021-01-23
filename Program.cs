using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using u8_disasm.Disasm;

namespace u8_disasm
{
    class Program
    {
        static void Main(string[] args)
        {
            u8_Disasm disasm = new u8_Disasm(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
            disasm.Disassemble(2000, true);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
