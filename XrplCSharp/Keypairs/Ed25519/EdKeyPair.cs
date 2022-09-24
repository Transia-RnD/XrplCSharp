using System;
using System.Security.Cryptography.X509Certificates;
using Xrpl.KeypairsLib;
using Sha512 = Xrpl.KeypairsLib.Utils.Sha512;
using Xrpl.AddressCodecLib;
using static Xrpl.AddressCodecLib.Utils;

namespace Xrpl.KeypairsLib.Ed25519
{
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

        public byte[] CanonicalPubBytes()
        {
            return _canonicalisedPubBytes;
        }

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

        public string Id()
        {
            return prefix + FromBytesToHex(this._pubBytes);
        }

        public string Pk()
        {
            return prefix + FromBytesToHex(this._privBytes[0..32]);
        }

        static public byte[] Sign(byte[] message, byte[] privateKey)
        {
            return Chaos.NaCl.Ed25519.Sign(message, privateKey);
        }

        static public bool Verify(byte[] signature, byte[] message, byte[] publicKey)
        {
            return Chaos.NaCl.Ed25519.Verify(signature, message, publicKey);
        }
    }
}