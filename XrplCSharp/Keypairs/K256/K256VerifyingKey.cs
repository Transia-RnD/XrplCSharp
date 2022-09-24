using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC;
using Xrpl.KeypairsLib.Utils;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Utilities.Encoders;
using System.Diagnostics;

namespace Xrpl.KeypairsLib.K256
{
    public class K256VerifyingKey
    {
        protected ECPoint PubKey;
        protected ECDsaSigner Verifier;
        protected byte[] PubKeyBytes;

        public K256VerifyingKey(ECPoint pub)
        {
            PubKey = pub;
            PubKeyBytes = pub.GetEncoded(true);
            SetVerifier(pub);
        }

        public byte[] CanonicalPubBytes()
        {
            return PubKeyBytes;
        }

        public bool Verify1(byte[] message, byte[] signature)
        {
            return VerifyHash(message, signature);
        }

        private bool VerifyHash(byte[] data, byte[] signature)
        {
            var sig = EcdsaSignature.DecodeFromDer(signature);
            return sig != null && Verifier.VerifySignature(data, sig.R, sig.S);
        }

        protected void SetVerifier(ECPoint pub)
        {
            Verifier = new ECDsaSigner();
            ECPublicKeyParameters parameters = new ECPublicKeyParameters(
                pub, Secp256K1.Parameters());
            Verifier.Init(false, parameters);
        }

        static public bool Verify(byte[] signature, byte[] message, byte[] publicKey)
        {
            ECDsaSigner verifier = new ECDsaSigner();
            ECPublicKeyParameters parameters = new ECPublicKeyParameters(Secp256K1.Curve().DecodePoint(publicKey), Secp256K1.Parameters());
            verifier.Init(false, parameters);
            EcdsaSignature sig = EcdsaSignature.DecodeFromDer(signature);
            return sig != null && verifier.VerifySignature(message, sig.R, sig.S);
        }
    }
}