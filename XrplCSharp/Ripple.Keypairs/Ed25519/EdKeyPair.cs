using System;

using Sha512 = Ripple.Keypairs.Utils.Sha512;

namespace Ripple.Keypairs.Ed25519
{
    //https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-keypairs/src/index.ts#L69
    public class EdKeyPair : IKeyPair
    {
        string prefix = "ED";
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

        internal static IKeyPair From128Seed(byte[] seed)
        {
            var edSecret = Sha512.Half(seed);
            byte[] publicKey;
            byte[] expandedPrivateKey;
            Chaos.NaCl.Ed25519.KeyPairFromSeed(out publicKey, out expandedPrivateKey, edSecret);
            return new EdKeyPair(publicKey, expandedPrivateKey);
        }

        public string Id() => prefix + Ripple.Address.Codec.Utils.FromBytesToHex(this._pubBytes);

        public string Pk() => prefix + Ripple.Address.Codec.Utils.FromBytesToHex(this._privBytes[0..32]);

        public static byte[] Sign(byte[] message, byte[] privateKey) => Chaos.NaCl.Ed25519.Sign(message, privateKey);

        public static bool Verify(byte[] signature, byte[] message, byte[] publicKey) => Chaos.NaCl.Ed25519.Verify(signature, message, publicKey);
    }
}