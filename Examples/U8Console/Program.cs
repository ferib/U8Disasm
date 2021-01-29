using System;

namespace U8Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("U8Disasm Console Example.");

            // NOTE: TESTing
            //U8Disasm.Core.U8Registers reg = new U8Disasm.Core.U8Registers();

            //reg.r1 = 0x00;
            //reg.r0 = 0x10;

            //var qwe = reg.ER0;

            // Insert your ROM file here
            Example test = new Example(@"L:\Projects\Calculator\Casio\ROM_Dump.mem");
            test.Disassemble(LineCount: 2000, PrintLines: true);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
