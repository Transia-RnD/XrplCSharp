using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC;
using Ripple.Signing.Utils;

namespace Ripple.Signing.K256
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

        public bool Verify(byte[] message, byte[] signature)
        {
            byte[] hash = Sha512.Half(message);
            return VerifyHash(hash, signature);
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
    }
}