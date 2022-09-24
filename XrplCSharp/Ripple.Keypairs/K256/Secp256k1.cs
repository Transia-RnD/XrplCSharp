using Org.BouncyCastle.Math;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-keypairs/src/secp256k1.ts

namespace Ripple.Keypairs.K256
{
    using SECNamedCurves = Org.BouncyCastle.Asn1.Sec.SecNamedCurves;
    using ECDomainParameters = Org.BouncyCastle.Crypto.Parameters.ECDomainParameters;
    using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

    public class Secp256K1
    {
        private static readonly ECDomainParameters EcParams;

        static Secp256K1()
        {
            var x9Params = SECNamedCurves.GetByName("secp256k1");
            EcParams = new ECDomainParameters(
                x9Params.Curve, x9Params.G, x9Params.N, x9Params.H);
        }

        public static ECDomainParameters Parameters() => EcParams;

        public static BigInteger Order() => EcParams.N;

        public static Org.BouncyCastle.Math.EC.ECCurve Curve() => EcParams.Curve;

        public static ECPoint BasePoint() => EcParams.G;
    }
}