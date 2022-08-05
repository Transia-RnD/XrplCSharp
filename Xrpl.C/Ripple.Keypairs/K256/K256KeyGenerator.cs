using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Ripple.Keypairs.Utils;

namespace Ripple.Keypairs.K256
{
    public class K256KeyGenerator
    {
        // See https://wiki.ripple.com/Account_Family
        public static K256KeyPair From128Seed(byte[] seedBytes, int keyIndex)
        {
            // The private generator (aka root private key, master private key)
            var privateGen = ComputePrivateGen(seedBytes);
            if (keyIndex == -1)
            {
                // The root keyPair
                return new K256KeyPair(privateGen);
            }
            var secret = ComputeSecretKey(privateGen, (uint) keyIndex);
            return new K256KeyPair(secret);
        }

        public static BigInteger ComputePrivateGen(byte[] seedBytes)
        {
            return ComputeScalar(seedBytes, null);
        }

        public static BigInteger ComputeSecretKey(
            BigInteger privateGen,
            uint accountNumber)
        {
            var publicGen = ComputePublicGenerator(privateGen);
            return ComputeScalar(publicGen.GetEncoded(true), accountNumber)
                            .Add(privateGen).Mod(Secp256K1.Order());
        }

        ///
        /// <param name="privateGen"> secret scalar</param>
        /// <returns> the corresponding public key is the public generator
        ///         (aka public root key, master public key).
        /// </returns>
        public static ECPoint ComputePublicGenerator(BigInteger privateGen) => ComputePublicKey(privateGen);

        public static byte[] ComputePublicKey(byte[] publicGenBytes, uint accountNumber)
        {
            var rootPubPoint = Secp256K1.Curve().DecodePoint(publicGenBytes);
            var scalar = ComputeScalar(publicGenBytes, accountNumber);
            var point = Secp256K1.BasePoint().Multiply(scalar);
            var offset = rootPubPoint.Add(point);
            return offset.GetEncoded(true);
        }

        /// <param name="seedBytes"> - a bytes sequence of arbitrary length which will be hashed </param>
        /// <param name="discriminator"> - nullable optional uint32 to hash </param>
        /// <returns> a number between [1, order -1] suitable as a private key
        ///  </returns>
        public static BigInteger ComputeScalar(byte[] seedBytes, uint? discriminator)
        {
            BigInteger key = null;
            for (uint i = 0; i <= 0xFFFFFFFFL; i++)
            {
                var sha512 = new Sha512(seedBytes);
                if (discriminator != null)
                {
                    sha512.AddU32(discriminator.Value);
                }
                sha512.AddU32(i);
                var keyBytes = sha512.Finish256();
                key = Misc.UBigInt(keyBytes);
                if (key.CompareTo(BigInteger.Zero) == 1 &&
                    key.CompareTo(Secp256K1.Order()) == -1)
                {
                    break;
                }
            }
            return key;
        }

        ///
        /// <param name="secretKey"> secret point on the curve as BigInteger </param>
        /// <returns> corresponding public point </returns>
        public static ECPoint ComputePublicKey(BigInteger secretKey) => Secp256K1.BasePoint().Multiply(secretKey);
    }
}