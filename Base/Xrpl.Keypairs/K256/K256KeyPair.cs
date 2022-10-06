using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Xrpl.Keypairs.Utils;
using static Xrpl.AddressCodec.Utils;

namespace Xrpl.Keypairs.K256
{
    using ECDSASigner = Org.BouncyCastle.Crypto.Signers.ECDsaSigner;
    using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;
    using ECPrivateKeyParameters = Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters;
    using HMacDsaKCalculator = Org.BouncyCastle.Crypto.Signers.HMacDsaKCalculator;
    using Sha256Digest = Org.BouncyCastle.Crypto.Digests.Sha256Digest;

    public class K256KeyPair : IXrplKeyPair
    {
        private readonly BigInteger _privKey;
        private bool _isNodeKey;

        public K256KeyPair(BigInteger priv, bool nodeKey = false)
        {
            _privKey = priv;
            _isNodeKey = nodeKey;
        }

        public string Id()
        {
            return K256KeyGenerator.ComputePublicKey(this._privKey).GetEncoded(true).ToHex();
        }

        public string Pk()
        {
            return $"00{FromBytesToHex(this._privKey.ToByteArrayUnsigned())}";
        }

        public static byte[] Sign(byte[] message, byte[] privateKey)
        {
            ECDSASigner signer = new ECDSASigner(new HMacDsaKCalculator(new Sha256Digest()));
            ECPrivateKeyParameters parameters = new ECPrivateKeyParameters("ECDSA", new BigInteger(privateKey), Secp256K1.Parameters());
            signer.Init(true, parameters);
            BigInteger[] sigs = signer.GenerateSignature(Sha512.Half(message));
            var r = sigs[0];
            var s = sigs[1];

            var otherS = Secp256K1.Order().Subtract(s);
            if (s.CompareTo(otherS) == 1)
            {
                s = otherS;
            }

            return new EcdsaSignature(r, s).EncodeToDer();
        }

        public static bool Verify(byte[] signature, byte[] message, byte[] publicKey)
        {
            ECDsaSigner verifier = new ECDsaSigner();
            ECPublicKeyParameters parameters = new ECPublicKeyParameters(Secp256K1.Curve().DecodePoint(publicKey), Secp256K1.Parameters());
            verifier.Init(false, parameters);
            EcdsaSignature sig = EcdsaSignature.DecodeFromDer(signature);
            return sig != null && verifier.VerifySignature(Sha512.Half(message), sig.R, sig.S);
        }
    }
}