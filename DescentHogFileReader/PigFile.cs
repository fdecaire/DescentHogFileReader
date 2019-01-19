using System;
using System.Collections.Generic;

namespace DescentHogFileReader
{
    //http://www.descent2.com/ddn/specs/pig/#pigd1full10
    public class PigFile
    {
        public int TotalTextures { get; set; }
        public int TotalSounds { get; set; }
        public List<PigTexture> Textures { get; set; } = new List<PigTexture>();
        public int PiggyStartData { get; set; }

        public PigFile(byte [] buffer)
        {
            int offset = BitConverter.ToInt32(buffer, 0);
            PiggyStartData = offset;
            TotalTextures = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            TotalSounds = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            
            for (var i = 0; i < TotalTextures; i++)
            {
                var pigTexture = new PigTexture();
              
                pigTexture.Name = buffer.ByteArrayToString(offset, 8);
                offset += 8;

                pigTexture.DFlags = buffer[offset++];
                pigTexture.Width = buffer[offset++];
                pigTexture.Height = buffer[offset++];
                pigTexture.Flags = buffer[offset++];
                pigTexture.AveColor = buffer[offset++];
                pigTexture.Offset = BitConverter.ToInt32(buffer, offset);
                offset += 4;
                Textures.Add(pigTexture);
            }

            // read all the texture data
            foreach (var texture in Textures)
            {
                offset = texture.Offset + PiggyStartData + 8 + 8;
                var rowSize = texture.Width;

                if ((texture.DFlags & 128) != 0) // DBM_FLAG_LARGE
                {
                    rowSize += 256;
                }

                
            }
        }
    }
}