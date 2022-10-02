using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-keypairs/src/utils.ts

namespace Xrpl.Keypairs.Utils
{
    internal class Misc
    {
        public static BigInteger UBigInt(byte[] bytes)
        {
            return new BigInteger(1, bytes);
        }

        public static string BigHex(BigInteger pub)
        {
            return Hex.ToHexString(pub.ToByteArrayUnsigned()).ToUpper();
        }

        public static string ToHex(byte[] bytes)
        {
            return Hex.ToHexString(bytes).ToUpper();
        }
    }
}
