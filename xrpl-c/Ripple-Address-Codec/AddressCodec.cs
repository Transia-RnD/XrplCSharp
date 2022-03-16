using System;

namespace Ripple.Address
{
    public class AddressCodec
    {
        public const string Alphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        // ReSharper disable RedundantArgumentNameForLiteralExpression, RedundantArgumentName
        public static B58.Version AccountId = B58.Version.With(versionByte: 0, expectedLength: 20);

        public static B58.Version NodePublic = B58.Version.With(versionByte: 28, expectedLength: 33);
        public static B58.Version NodePrivate = B58.Version.With(versionByte: 32, expectedLength: 32);

        public static B58.Version AccountPublic = B58.Version.With(versionByte: 35, expectedLength: 33);
        public static B58.Version AccountPrivate = B58.Version.With(versionByte: 34, expectedLength: 32);

        public static B58.Version FamilyGenerator = B58.Version.With(versionByte: 41, expectedLength: 33);

        public static B58.Version K256Seed = B58.Version.With(versionByte: 33, expectedLength: 16);
        public static B58.Version Ed25519Seed = B58.Version.With(versionBytes: new byte[]{ 0x1, 0xe1, 0x4b }, expectedLength: 16);

        public static B58.Versions AnySeed = B58.Versions
                                                .With("secp256k1", K256Seed)
                                                .And("ed25519", Ed25519Seed);
        // ReSharper restore RedundantArgumentNameForLiteralExpression, RedundantArgumentName
        private static readonly B58 B58;
        static AddressCodec()
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

        public static DecodedSeed DecodeSeed(string seed)
        {
            var decoded = B58.Decode(seed, AnySeed);
            return new DecodedSeed(decoded.Type, decoded.Payload);
        }

        public static string EncodeSeed(byte[] bytes, string type)
        {
            return B58.Encode(bytes, type, AnySeed);
        }

        public static byte[] DecodeK256Seed(string seed)
        {
            return B58.Decode(seed, K256Seed);
        }

        public static string EncodeK256Seed(byte[] bytes)
        {
            return B58.Encode(bytes, K256Seed);
        }

        public static byte[] DecodeEdSeed(string base58)
        {
            return B58.Decode(base58, Ed25519Seed);
        }

        public static string EncodeEdSeed(byte[] bytes)
        {
            return B58.Encode(bytes, Ed25519Seed);
        }

        public static string EncodeAddress(byte[] bytes)
        {
            return B58.Encode(bytes, AccountId);
        }

        public static string EncodeNodePublic(byte[] bytes)
        {
            return B58.Encode(bytes, NodePublic);
        }

        public static byte[] DecodeNodePublic(string publicKey)
        {
            return B58.Decode(publicKey, NodePublic);
        }

        public static byte[] DecodeAddress(string address)
        {
            return B58.Decode(address, AccountId);
        }

        public static bool IsValidAddress(string address)
        {
            return B58.IsValid(address, AccountId);
        }

        public static bool IsValidNodePublic(string nodePublic)
        {
            return B58.IsValid(nodePublic, NodePublic);
        }

        public static bool IsValidSeed(string seed)
        {
            return B58.IsValid(seed, AnySeed);
        }

        public static bool IsValidEdSeed(string edSeed)
        {
            return B58.IsValid(edSeed, Ed25519Seed);
        }

        public static bool IsValidK256Seed(string k256Seed)
        {
            return B58.IsValid(k256Seed, K256Seed);
        }
    }
}
