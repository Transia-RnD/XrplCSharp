using Org.BouncyCastle.Utilities.Encoders;

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Xrpl.Client.Extensions
{
    public static class ExtensionHelpers
    {
        public static string ToHex(this string input)
        {
            char[] values = input.ToCharArray();

            StringBuilder sb = new StringBuilder();

            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                sb.Append($"{value:X}");                
            }

            return sb.ToString();
        }

        public static string FromHexString(this string input)
        {
            byte[] bytes = Enumerable.Range(0, input.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(input.Substring(x, 2), 16))
                .ToArray();
            return HttpUtility.HtmlEncode(Encoding.ASCII.GetString(bytes));
        }
        public static string FromHexUtf8String(this string hex)
        {
            var buffer = new byte[hex.Length / 2];
            for (var i = 0; i < hex.Length; i += 2)
            {
                var hexadecimal = hex.Substring(i, 2);
                buffer[i / 2] = byte.Parse(hexadecimal, NumberStyles.HexNumber);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
