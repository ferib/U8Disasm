using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using U8Disasm.Disasm;
using System.Threading;

namespace u8_Console
{
    class Program
    {
        static void Main(string[] args)
        {

            u8_Disasm disasm;

            Thread t = new Thread(doTask);
            t.Start();

            void doTask()
            {
                disasm = new u8_Disasm(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
