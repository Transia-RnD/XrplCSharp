using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Xrpl.KeypairsLib
{
    public static class ExtensionHelpers
    {
        public static string ToHex(this byte[] input)
        {
            return Hex.ToHexString(input).ToUpper();
        }

        public static byte[] FromHex(this string input)
        {
            return Hex.Decode(input);
        }
    }
}