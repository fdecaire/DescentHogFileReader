namespace DescentHogFileReader
{
    public class PigTexture
    {
        public string Name { get; set; }
        public int DFlags { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Flags { get; set; }
        public int AveColor { get; set; }
        public int Offset { get; set; }
        public byte[] Data { get; set; }
        public bool RunLengthEncoded => (Flags & 8) != 0;
        public bool Animation { get; set; }
    }
}
