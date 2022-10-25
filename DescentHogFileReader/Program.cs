using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DescentHogFileReader
{
    class Program
    {
        private static string _outputDirectory = @"c:\temp\DescentAssets\";
        private static string _textureOutputDirectory = @"c:\temp\DescentAssets\Textures\";

        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream("DescentHogFileReader.DESCENT.HOG"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                char[] sig = {(char) buffer[0], (char) buffer[1], (char) buffer[2]};

                int index = 3;

                var fileData = new List<HogFile>();

                int totalFiles = 0;
                while (index < buffer.Length)
                {
                    fileData.Add(new HogFile(buffer, index));
                    index += fileData[fileData.Count - 1].FileSize + 13 + 4;

                    // write file to output directory
                    File.WriteAllBytes(_outputDirectory + fileData[fileData.Count - 1].FileName, fileData[fileData.Count - 1].Data);

                    //Console.WriteLine(fileData[fileData.Count - 1].FileName + "   (" + fileData[fileData.Count - 1].FileSize.ToString("N0") + ")");
                    totalFiles++;
                }

                
            }

            // read the palette
            Palette paletteData;
            using (var stream = assembly.GetManifestResourceStream("DescentHogFileReader.palette.256"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                paletteData = new Palette(buffer);
            }


            // read the textures
            using (var stream = assembly.GetManifestResourceStream("DescentHogFileReader.DESCENT.PIG"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                // read the textures and sounds
                var pigData = new PigFile(buffer, paletteData);

                // dump all the textures
                if (!Directory.Exists(_textureOutputDirectory))
                {
                    Directory.CreateDirectory(_textureOutputDirectory);
                }

                File.Delete(_textureOutputDirectory + "texture_list.txt");
                File.Delete(_textureOutputDirectory + "texture_names.txt");
                foreach (var texture in pigData.Textures)
                {
                    try
                    {
                        var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format24bppRgb);
                        var bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        var pNative = bmData.Scan0;
                        Marshal.Copy(texture.Data, 0, pNative, texture.Width * texture.Height * 3);
                        bitmap.UnlockBits(bmData);
                        bitmap.Save(_textureOutputDirectory + texture.Name.Trim() + (texture.Animation ? "_" + (texture.DFlags & 63).ToString() : "") + ".png", ImageFormat.Png);
                    }
                    catch
                    {
                        // ignored
                    }

                    File.AppendAllText(_textureOutputDirectory + "texture_list.txt", texture.Name + " (" + texture.Width + " x " + texture.Height + $", Frame:{texture.DFlags & 63}){(texture.RunLengthEncoded ? " RLE" : "")} " + (texture.Transparent ? " Transparent" : "") + (texture.SuperTransparent ? " SuperTransparent" : "") + (texture.PagedOut ? " PagedOut" : "") + Environment.NewLine);
                    //File.AppendAllText(_textureOutputDirectory + "texture_names.txt", texture.Name + Environment.NewLine);
                }

                foreach (var texture in pigData.Textures.Select(n => n.Name).Distinct())
                {
                    File.AppendAllText(_textureOutputDirectory + "texture_names.txt", texture + Environment.NewLine);
                }
                
            }


            // scan each file to see if it contains texture names
            using (var stream = assembly.GetManifestResourceStream("DescentHogFileReader.DESCENT.HOG"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                //rock169
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 'r' || buffer[i] == 'R')
                    {
                        if (i + 1 < buffer.Length && (buffer[i + 1] == 'o' || buffer[i + 1] == 'O'))
                        {
                            if (i + 2 < buffer.Length && (buffer[i + 2] == 'c' || buffer[i + 2] == 'C'))
                            {
                                if (i + 3 < buffer.Length && (buffer[i + 3] == 'k' || buffer[i + 3] == 'K'))
                                {
                                    if (i + 4 < buffer.Length && buffer[i + 4] == '0')
                                    {
                                        if (i + 5 < buffer.Length && buffer[i + 5] == '0')
                                        {
                                            if (i + 6 < buffer.Length && buffer[i + 6] == '1')
                                            {
                                                string ignore = "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //Console.ReadKey();
        }
    }
}
