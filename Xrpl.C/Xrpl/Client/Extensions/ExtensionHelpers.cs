using System;
using System.Globalization;
using System.Text;

namespace Xrpl.Client.Extensions
{
    public static class ExtensionHelpers
    {
        public static string ToHex(this string input)
        {
            var values = input.ToCharArray();

            var sb = new StringBuilder();

            foreach (var letter in values)
            {
                var value = Convert.ToInt32(letter);
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
