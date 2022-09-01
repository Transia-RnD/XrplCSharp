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
        public static string ConvertStringToHex(this string input)
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
            if (string.IsNullOrWhiteSpace(input))
                return null;
            var buffer = new byte[input.Length / 2];
            for (var i = 0; i < input.Length; i += 2)
            {
                var hexadecimal = input.Substring(i, 2);
                buffer[i / 2] = byte.Parse(hexadecimal, NumberStyles.HexNumber);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
