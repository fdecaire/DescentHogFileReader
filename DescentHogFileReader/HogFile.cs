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
