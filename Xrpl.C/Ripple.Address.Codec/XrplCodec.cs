using System;

namespace Ripple.Address.Codec
{
    public class XrplCodec
    {
        public const string Alphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        public static B58.Version AccountId = B58.Version.With(versionByte: 0, expectedLength: 20);

        public static B58.Version K256Seed = B58.Version.With(versionByte: 33, expectedLength: 16);
        public static B58.Version Ed25519Seed = B58.Version.With(versionBytes: new byte[] { 0x1, 0xe1, 0x4b }, expectedLength: 16);

        public static B58.Versions AnySeed = B58.Versions.With("secp256k1", K256Seed).And("ed25519", Ed25519Seed);

        private static readonly B58 B58;
        static XrplCodec()
        {
            B58 = new B58(Alphabet);
        }

        public class DecodedSeed
        {
            public readonly string Type;
            public readonly byte[] Bytes;

            public DecodedSeed(string type, byte[] payload)
            {
                Type = type;
                Bytes = payload;
            }
        }

        public static string EncodeSeed(byte[] bytes, string type)
        {
            return B58.Encode(bytes, type, AnySeed);
        }

        public static DecodedSeed DecodeSeed(string seed)
        {
            var decoded = B58.Decode(seed, AnySeed);
            return new DecodedSeed(decoded.Type, decoded.Payload);
        }

        public static string EncodeAccountID(byte[] bytes)
        {
            return B58.Encode(bytes, AccountId);
        }

        public static byte[] DecodeAccountID(string accountId)
        {
            return B58.Decode(accountId, AccountId);
        }
    }
}
