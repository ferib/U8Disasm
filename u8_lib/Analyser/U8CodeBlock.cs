using U8Disasm.Structs;

namespace U8Disasm.Analyser
{
    public class U8CodeBlock
    {
        public int Address;
        //public byte[] Bytes;
        public U8Cmd[] Ops;
        public string[] Disassembly; // cache?

        // We only need one jump, and then figure if its conditional or not?
        public int JumpsToBlock = -1;
        public int NextBlock = -1;


        public U8CodeBlock(U8Cmd[] ops)
        {
            this.Ops = ops;
        }

        public override string ToString()
        {
            return $"{Ops.Length.ToString().PadRight(3)} @ { this.Address.ToString("X8")}";
        }

    }

}
