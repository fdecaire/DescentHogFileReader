using System.Drawing;

namespace DescentHogFileReader
{
    public class Palette
    {
        public Color[] paletteColors { get; set; } = new Color [256];
    
        public Palette(byte[] buffer)
        {
            var offset = 0;
            for (var i = 0; i < 256; i++)
            {
                paletteColors[i] = Color.FromArgb(buffer[offset + 2] * 4, buffer[offset + 1] * 4, buffer[offset] * 4);
                offset += 3;
            }
        }
    }
}
