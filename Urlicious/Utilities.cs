using System.Diagnostics;
using System.Text;

namespace Urlicious
{
    public static class Utilities
    {
        public static string UrlEncode(string str)
        {
            if (str == null)
                return null;
            return UrlEncode(str, Encoding.UTF8);
        }

        private static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
                return null;
            return Encoding.UTF8.GetString(UrlEncodeToBytes(str, e));
        }

        private static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
                return null;
            byte[] bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int cSpaces = 0;
            int cUnsafe = 0;

            // count them first
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];

                if (ch == ' ')
                    cSpaces++;
                else if (!IsSafe(ch))
                    cUnsafe++;
            }

            // nothing to expand?
            if (!alwaysCreateReturnValue && cSpaces == 0 && cUnsafe == 0)
                return bytes;

            // expand not 'safe' characters into %XX, spaces to +s
            byte[] expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[offset + i];
                char ch = (char)b;

                if (IsSafe(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else
                {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;
        }

        private static char IntToHex(int n)
        {
            Debug.Assert(n < 0x10);

            if (n <= 9)
                return (char)(n + '0');

            return (char)(n - 10 + 'a');
        }

        private static bool IsSafe(char ch)
        {
            if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9')
                return true;

            switch (ch)
            {
                case '-':
                case '_':
                case '.':
                case '!':
                case '*':
                case '\'':
                case '(':
                case ')':
                    return true;
            }

            return false;
        }
    }
}
