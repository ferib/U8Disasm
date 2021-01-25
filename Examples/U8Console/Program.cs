using System;

namespace U8Console
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
