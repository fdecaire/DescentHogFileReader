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
        public List<PigSound> Sounds { get; set; } = new List<PigSound>();

        public PigFile(byte [] buffer)
        {
            int offset = BitConverter.ToInt32(buffer, 0);
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

            for (var i = 0; i < TotalSounds; i++)
            {
                var pigSound = new PigSound();

                pigSound.Name = buffer.ByteArrayToString(offset, 8);
                offset += 8;
                pigSound.Length = BitConverter.ToInt32(buffer, offset);
                offset += 4;
                pigSound.DataLength = BitConverter.ToInt32(buffer, offset);
                offset += 4;
                pigSound.Offset = BitConverter.ToInt32(buffer, offset);
                offset += 4;

                Sounds.Add(pigSound);
            }

            // read all the texture data
            foreach (var texture in Textures)
            {
                var rowSize = texture.Width;

                if ((texture.DFlags & 128) != 0) // DBM_FLAG_LARGE
                {
                    rowSize += 256;
                }

                if (texture.RunLengthEncoded)
                {
                    var size = BitConverter.ToInt32(buffer, offset);
                    texture.Data = new byte[size];
                    for (var i = 0; i < size; i++)
                    {
                        texture.Data[i] = buffer[i + offset];
                    }

                    // uncompress
                    var result = new List<byte>();
                    int j = 0;
                    while (j < size)
                    {
                        var color = texture.Data[j];
                        if ((color & 0xe0) == 0xe0)
                        {
                            j++;
                            var count = color & 0x1f;
                            for (var k = 0; k < count; k++)
                            {
                                result.Add(texture.Data[j]);
                            }
                        }
                        else
                        {
                            result.Add(texture.Data[j++]);
                        }
                    }

                    // replace with decoded data
                    texture.Data = result.ToArray();

                    //TODO: need to convert indexed data into 32 bit per pixel true color
                }
                else
                {
                    var size = texture.Height * texture.Width;
                    texture.Data = new byte[size];
                    for (var i = 0; i < size; i++)
                    {
                        texture.Data[i] = buffer[i + offset];
                    }
                }
            }
        }
    }
}