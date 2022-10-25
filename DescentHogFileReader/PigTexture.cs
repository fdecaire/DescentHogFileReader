namespace DescentHogFileReader
{
    public class PigTexture
    {
        public string Name { get; set; }
        public int DFlags { get; set; } //dflags -> 0x40 = animated texture, frame mask = 0x3f (frame number)
        public int Width { get; set; }
        public int Height { get; set; }
        public int Flags { get; set; }
        public int AveColor { get; set; }
        public int Offset { get; set; }
        public byte[] Data { get; set; }
        public bool RunLengthEncoded => (Flags & 8) != 0;
        public bool Transparent => (Flags & 1) != 0;
        public bool SuperTransparent => (Flags & 2) != 0;
        public bool NoLighting => (Flags & 4) != 0;
        public bool PagedOut => (Flags & 16) != 0;
        public bool Animation { get; set; }
    }
}
