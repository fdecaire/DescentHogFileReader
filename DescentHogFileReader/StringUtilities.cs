namespace DescentHogFileReader
{
    public static class StringUtilities
    {
        public static string ByteArrayToString(this byte[] buffer, int offset, int length)
        {
            var name = new char[length];
            var lastChar = false;
            for (var i = 0; i < length; i++)
            {
                if (!lastChar)
                    name[i] = (char)buffer[i + offset];

                if (name[i] == 0 || lastChar)
                {
                    name[i] = ' ';
                    lastChar = true;
                }
            }

            return new string(name);
        }
    }
}
