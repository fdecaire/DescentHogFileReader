using System;

namespace DescentHogFileReader
{
    public class HogFile
    {
        public string FileName { get; set; }
        private HogFileType ? _fileType;
        public HogFileType FileType {
            get
            {
                if (_fileType == null)
                {
                    switch (FileName.Substring(FileName.Length - 3, 3).ToUpper())
                    {
                        case "RDL":
                            _fileType = HogFileType.RDL;
                            break;
                        default:
                            _fileType = HogFileType.Unknown;
                            break;
                    }
                }

                return _fileType ?? HogFileType.Unknown;
            }
        }
        public int FileSize { get; set; }
        public byte[] Data { get; set; }

        public HogFile(byte[] buffer, int index)
        {
            /*
            var fileName = new char[13];

            var lastChar = false;
            for (int i = 0; i < 13; i++)
            {
                if (!lastChar)
                    fileName[i] = (char) buffer[i + index];

                if (fileName[i] == 0 || lastChar)
                {
                    fileName[i] = ' ';
                    lastChar = true;
                }
            }

            FileName = new string(fileName);
            */
            FileName = buffer.ByteArrayToString(index, 13);
            FileName = FileName.Trim();

            FileSize = BitConverter.ToInt32(buffer, 13 + index);

            Data = new byte[FileSize];

            for (var i = 0; i < FileSize; i++)
            {
                Data[i] = buffer[i + index + 13 + 4];
            }
        }

     
    }
}
