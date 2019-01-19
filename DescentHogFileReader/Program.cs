using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DescentHogFileReader
{
    class Program
    {
        private static string _outputDirectory = @"c:\temp\DescentAssets\";

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

                    Console.WriteLine(fileData[fileData.Count - 1].FileName + "   (" + fileData[fileData.Count - 1].FileSize.ToString("N0") + ")");
                    totalFiles++;
                }

            }

            using (var stream = assembly.GetManifestResourceStream("DescentHogFileReader.DESCENT.PIG"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                // read the textures and sounds
                var pigData = new PigFile(buffer);


            }


            Console.ReadKey();
        }
    }
}
