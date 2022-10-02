using Org.BouncyCastle.Utilities.Encoders;

namespace Xrpl.Keypairs
{
    internal static class ExtensionHelpers
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