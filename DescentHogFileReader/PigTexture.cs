namespace DescentHogFileReader
{
    public class PigTexture
    {
        public string Name { get; set; }
        public int Frame { get; set; }
        public int DFlags { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Flags { get; set; }
        public int AveColor { get; set; }
        public int Offset { get; set; }
    }

    /*
pedef struct DiskBitmapHeader {
	char name[8];
	ubyte dflags;
	ubyte	width;	
	ubyte height;
	ubyte flags;
	ubyte avg_color;
	int offset;
} DiskBitmapHeader;
     */
}
