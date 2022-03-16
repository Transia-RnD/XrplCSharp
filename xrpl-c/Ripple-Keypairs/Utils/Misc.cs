using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

namespace Ripple.Signing.Utils
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
