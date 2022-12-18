using System;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;
using Xrpl.Keypairs.Utils;
using System.Globalization;

namespace Xrpl.Keypairs
{
    internal static class ExtensionHelpers
    {
        internal static string ConvertStringToHex(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Hex.ToHexString(bytes);
        }

        internal static string FromHexString(this string input)
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

        // <summary>
        /// from bytes array to hex row
        /// </summary>
        /// <param name="bytes">bytes array</param>
        /// <returns></returns>
        public static string ToHex(this byte[] input)
        {
            return Hex.ToHexString(input).ToUpper();
        }

        /// <summary>
        /// hex row to bytes array
        /// </summary>
        /// <param name="hex">hex row</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string input)
        {
            return Hex.Decode(input);
        }

        internal static byte[] Sha512HashHalf(this byte[] input)
        {
            return Sha512.Half(input: input);
        }
    }
}