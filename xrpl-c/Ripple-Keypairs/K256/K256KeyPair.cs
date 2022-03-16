using Org.BouncyCastle.Math;
using Ripple.Signing.Utils;

namespace Ripple.Signing.K256
{
    using ECPrivateKeyParameters = Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters;
    using ECDSASigner = Org.BouncyCastle.Crypto.Signers.ECDsaSigner;
    using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;
    using HMacDsaKCalculator = Org.BouncyCastle.Crypto.Signers.HMacDsaKCalculator;
    using Sha256Digest = Org.BouncyCastle.Crypto.Digests.Sha256Digest;

    public class K256KeyPair : K256VerifyingKey, IKeyPair
    {
        private readonly BigInteger _privKey;
        private ECDSASigner _signer;
        private bool _isNodeKey;

        public K256KeyPair(BigInteger priv) : 
            this(priv, K256KeyGenerator.ComputePublicKey(priv))
        {
        }

        internal K256KeyPair(BigInteger priv, ECPoint pub) : base(pub)
        {
            _privKey = priv;
            InitSigner(priv);
        }

        private void InitSigner(BigInteger priv)
        {
            _signer = new ECDSASigner(new HMacDsaKCalculator(new Sha256Digest()));
            ECPrivateKeyParameters privKey = new ECPrivateKeyParameters(priv, Secp256K1.Parameters());
            _signer.Init(true, privKey);
        }

        internal K256KeyPair SetNodeKey()
        {
            _isNodeKey = true;
            return this;
        }

        public BigInteger Priv()
        {
            return _privKey;
        }

        public byte[] Sign(byte[] message)
        {
            return SignHash(Sha512.Half(message));
        }

        private byte[] SignHash(byte[] bytes)
        {
            return CreateEcdsaSignature(bytes).EncodeToDer();
        }

        private EcdsaSignature CreateEcdsaSignature(byte[] hash)
        {

            BigInteger[] sigs = _signer.GenerateSignature(hash);
            var r = sigs[0];
            var s = sigs[1];

            var otherS = Secp256K1.Order().Subtract(s);
            if (s.CompareTo(otherS) == 1)
            {
                s = otherS;
            }

            return new EcdsaSignature(r, s);
        }

        public string Id()
        {
            if (_isNodeKey)
            {
                return Address.AddressCodec.EncodeNodePublic(CanonicalPubBytes());
            }
            return Address.AddressCodec.EncodeAddress(this.PubKeyHash());
        }
    }

}