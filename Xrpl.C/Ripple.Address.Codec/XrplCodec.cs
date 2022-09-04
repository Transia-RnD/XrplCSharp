using System;
using System.Diagnostics;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/xrp-codec.ts

namespace Ripple.Address.Codec
{
    public class XrplCodec
    {
        public const string Alphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        // Account address (20 bytes)
        public static B58.Version AccountID = B58.Version.With(versionByte: 0, expectedLength: 20);
        // Account public key (33 bytes)
        public static B58.Version PublicKey = B58.Version.With(versionByte: 35, expectedLength: 33);
        // 33; Seed value (for secret keys) (16 bytes)
        public static B58.Version K256Seed = B58.Version.With(versionByte: 33, expectedLength: 16);
        // [1, 225, 75]
        public static B58.Version Ed25519Seed = B58.Version.With(versionBytes: new byte[] { 0x1, 0xe1, 0x4b }, expectedLength: 16);
        // 28; Validation public key (33 bytes)
        public static B58.Version NodePublic = B58.Version.With(versionByte: 28, expectedLength: 33);

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
            return B58.Encode(bytes, AccountID);
        }

        public static byte[] DecodeAccountID(string accountId)
        {
            return B58.Decode(accountId, AccountID);
        }

        public static string EncodeAccountPublic(byte[] bytes)
        {
            return B58.Encode(bytes, PublicKey);
        }

        public static byte[] DecodeAccountPublic(string address)
        {

            return B58.Decode(address, PublicKey);
        }

        public static string EncodeNodePublic(byte[] bytes)
        {
            return B58.Encode(bytes, NodePublic);
        }

        public static byte[] DecodeNodePublic(string publicKey)
        {
            return B58.Decode(publicKey, NodePublic);
        }

        public static bool IsValidClassicAddress(string address)
        {
            return B58.IsValid(address, AccountID);
        }
    }
}
