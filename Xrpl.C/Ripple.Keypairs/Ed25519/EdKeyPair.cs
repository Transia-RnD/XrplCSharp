using System;
using Sha512 = Ripple.Keypairs.Utils.Sha512;

namespace Ripple.Keypairs.Ed25519
{
    public class EdKeyPair : IKeyPair
    {
        private byte[] _canonicalisedPubBytes;
        private readonly byte[] _pubBytes;
        private readonly byte[] _privBytes;

        private EdKeyPair(byte[] publicKey, byte[] expandedPrivateKey)
        {
            _pubBytes = publicKey;
            _privBytes = expandedPrivateKey;
            ComputeCanonicalPub();
        }

        public byte[] CanonicalPubBytes() => _canonicalisedPubBytes;

        private void ComputeCanonicalPub()
        {
            _canonicalisedPubBytes = new byte[33];
            _canonicalisedPubBytes[0] = 0xed;
            Array.Copy(_pubBytes, 0, _canonicalisedPubBytes,
                       1, _pubBytes.Length);
        }

        public byte[] Sign(byte[] message) => Chaos.NaCl.Ed25519.Sign(message, _privBytes);

        public bool Verify(byte[] message, byte[] signature) => Chaos.NaCl.Ed25519.Verify(signature, message, _pubBytes);

        internal static IKeyPair From128Seed(byte[] seed)
        {
            var edSecret = Sha512.Half(seed);
            Chaos.NaCl.Ed25519.KeyPairFromSeed(out var publicKey, out var expandedPrivateKey, edSecret);
            return new EdKeyPair(publicKey, expandedPrivateKey);
        }

        public string Id() => Address.Codec.AddressCodec.EncodeAddress(this.PubKeyHash());
    }
}