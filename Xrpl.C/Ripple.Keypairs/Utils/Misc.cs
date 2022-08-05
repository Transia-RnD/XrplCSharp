using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

namespace Ripple.Keypairs.Utils
{
    internal class Misc
    {
        public static BigInteger UBigInt(byte[] bytes) => new BigInteger(1, bytes);

        public static string BigHex(BigInteger pub) => Hex.ToHexString(pub.ToByteArrayUnsigned()).ToUpper();

        public static string ToHex(byte[] bytes) => Hex.ToHexString(bytes).ToUpper();
    }
}
