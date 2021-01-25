using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Threading;

namespace u8_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("U8Disasm Console Example.");

            // Insert your ROM file here
            Example test = new Example(@"L:\Projects\Calculator\Casio\ROM_Dump.mem"); 
            test.Disassemble(LineCount: 2000, PrintLines: true);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
