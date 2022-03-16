using System;
using System.Reflection;

namespace Ripple.Address.Tests
{
    internal class B16
    {
        public static string Encode(byte[] data)
        {
            if (data == null)
                return null;
            char[] c = new char[data.Length * 2];
            int b;
            for (int i = 0; i < data.Length; i++)
            {
                b = data[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = data[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }


        public static byte[] Decode(string hexString)
        {
            if (hexString == null)
                return null;
            if (hexString.Length % 2 != 0)
                throw new FormatException("The hex string is invalid because it has an odd length");
            var result = new byte[hexString.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return result;
        }
    }

    internal class Helpers
    {
        public static T Invoke<T>(MethodInfo info, params object[] objects)
        {
            return (T)info.Invoke(null, objects);
        }

        public static byte[] DecodeHex(string hex)
        {
            return B16.Decode(hex);
        }

        public static string EncodeHex(byte[] buffer)
        {
            return B16.Encode(buffer);
        }
    }
}
