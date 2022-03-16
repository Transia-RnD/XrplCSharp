using System;
using System.Security.Cryptography;
using System.Text;
using Ripple.Address;
using Ripple.Signing.Ed25519;
using Ripple.Signing.K256;
using Ripple.Signing.Utils;

namespace Ripple.Signing
{
    using static AddressCodec;

    public class Seed
    {
        private readonly byte[] _seedBytes;
        private string _keyType = "secp256k1";
        private bool _isNodeKey;

        public Seed(byte[] seedBytes) : this("secp256k1", seedBytes)
        {
        }
        public Seed(string version, byte[] seedBytes)
        {
            _seedBytes = seedBytes;
            _keyType = version;
        }

        public override string ToString()
        {
            return EncodeSeed(_seedBytes, _keyType);
        }

        public byte[] Bytes()
        {
            return _seedBytes;
        }

        public Seed SetEd25519()
        {
            _keyType = "ed25519";
            return this;
        }
        public Seed SetNodeKey()
        {
            _isNodeKey = true;
            return this;
        }

        public IKeyPair KeyPair()
        {
            return KeyPair(0);
        }

        public IKeyPair RootKeyPair()
        {
            return KeyPair(-1);
        }

        public IKeyPair KeyPair(int pairIndex)
        {
            if (_keyType == "ed25519")
            {
                if (pairIndex != 0)
                {
                    throw new ArgumentException("`account` is ignored for ed25519");
                }
                return EdKeyPair.From128Seed(_seedBytes);
            }
            pairIndex = _isNodeKey ? -1 : pairIndex;
            var pair = K256KeyGenerator.From128Seed(_seedBytes, pairIndex);
            if (_isNodeKey)
            {
                pair.SetNodeKey();
            }
            return pair;
        }

        public static Seed FromBase58(string b58)
        {
            var seed = DecodeSeed(b58);
            return new Seed(seed.Type, seed.Bytes);
        }

        public static Seed FromPassPhrase(string passPhrase)
        {
            return new Seed(PassPhraseToSeedBytes(passPhrase));
        }

        public static Seed FromRandom()
        {
            using (var rng = RandomNumberGenerator.Create())
            { 
                var seed = new byte[16];
                rng.GetBytes(seed);
                return new Seed(seed);
            }
        }

        private static byte[] PassPhraseToSeedBytes(string phrase)
        {
            var phraseBytes = Encoding.UTF8.GetBytes(phrase.ToCharArray());
            return (new Sha512(phraseBytes).Finish128());
        }
    }
}