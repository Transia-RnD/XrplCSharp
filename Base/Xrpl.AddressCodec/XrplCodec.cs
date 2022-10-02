// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-address-codec/src/xrp-codec.ts

namespace Xrpl.AddressCodec
{
    public class XrplCodec
    {
        public const string Alphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        /// <summary>
        /// Account address (20 bytes)
        /// </summary>
        public static B58.Version AccountID = B58.Version.With(versionByte: 0, expectedLength: 20);
        /// <summary>
        /// Account public key (33 bytes)
        /// </summary>
        public static B58.Version PublicKey = B58.Version.With(versionByte: 35, expectedLength: 33);
        /// <summary>
        /// 33; Seed value (for secret keys) (16 bytes)
        /// </summary>
        public static B58.Version K256Seed = B58.Version.With(versionByte: 33, expectedLength: 16);
        /// <summary>
        /// [1, 225, 75]
        /// </summary>
        public static B58.Version Ed25519Seed = B58.Version.With(versionBytes: new byte[] { 0x1, 0xe1, 0x4b }, expectedLength: 16);
        /// <summary>
        /// 28; Validation public key (33 bytes)
        /// </summary>
        public static B58.Version NodePublic = B58.Version.With(versionByte: 28, expectedLength: 33);

        public static B58.Versions AnySeed = B58.Versions.With("secp256k1", K256Seed).And("ed25519", Ed25519Seed);

        private static readonly B58 B58;
        static XrplCodec()
        {
            B58 = new B58(Alphabet);
        }

        /// <summary> decoded seed </summary>
        public class DecodedSeed
        {
            /// <summary> seed type </summary>
            public readonly string Type;
            /// <summary> seed bytes </summary>
            public readonly byte[] Bytes;
            
            public DecodedSeed(string type, byte[] payload)
            {
                Type = type;
                Bytes = payload;
            }
        }

        /// <summary>
        /// Returns an encoded seed.
        /// </summary>
        /// <param name="bytes">Entropy bytes of SEED_LENGTH.</param>
        /// <param name="type">Either ED25519 or SECP256K1.</param>
        /// <returns>An encoded seed.</returns>
        /// <throws> AddressCodecError: If entropy is not of length SEED_LENGTH
        /// or the encoding type is not one of CryptoAlgorithm.</throws>
        public static string EncodeSeed(byte[] bytes, string type) => B58.Encode(bytes, type, AnySeed);

        /// <summary>
        /// Returns (decoded seed, its algorithm).
        /// </summary>
        /// <param name="seed">The b58 encoding of a seed.</param>
        /// <returns>A(decoded seed, its algorithm).</returns>
        /// <throws>SeedError: If the seed is invalid.</throws>
        public static DecodedSeed DecodeSeed(string seed)
        {
            var decoded = B58.Decode(seed, AnySeed);
            return new DecodedSeed(decoded.Type, decoded.Payload);
        }

        /// <summary>
        /// Returns the classic address encoding of these bytes as a base58 string.
        /// </summary>
        /// <param name="bytes">Bytes to be encoded.</param>
        /// <returns>The classic address encoding of these bytes as a base58 string.</returns>
        public static string EncodeAccountID(byte[] bytes) => B58.Encode(bytes, AccountID);

        /// <summary>
        /// Returns the decoded bytes of the classic address.
        /// </summary>
        /// <param name="accountId">Classic address to be decoded.</param>
        /// <returns>The decoded bytes of the classic address.</returns>
        public static byte[] DecodeAccountID(string accountId) => B58.Decode(accountId, AccountID);

        /// <summary>
        /// Returns the account public key encoding of these bytes as a base58 string.
        /// </summary>
        /// <param name="bytes">Bytes to be encoded.</param>
        /// <returns>The account public key encoding of these bytes as a base58 string.</returns>
        public static string EncodeAccountPublic(byte[] bytes) => B58.Encode(bytes, PublicKey);

        /// <summary>
        /// Returns the decoded bytes of the account public key.
        /// </summary>
        /// <param name="address">Account public key to be decoded.</param>
        /// <returns>The decoded bytes of the account public key.</returns>
        public static byte[] DecodeAccountPublic(string address) => B58.Decode(address, PublicKey);

        /// <summary>
        /// Returns the node public key encoding of these bytes as a base58 string.
        /// </summary>
        /// <param name="bytes">Bytes to be encoded.</param>
        /// <returns>The node public key encoding of these bytes as a base58 string.</returns>
        public static string EncodeNodePublic(byte[] bytes) => B58.Encode(bytes, NodePublic);

        /// <summary>
        /// Returns the decoded bytes of the node public key
        /// </summary>
        /// <param name="publicKey">Node public key to be decoded.</param>
        /// <returns>The decoded bytes of the node public key.</returns>
        public static byte[] DecodeNodePublic(string publicKey) => B58.Decode(publicKey, NodePublic);

        /// <summary>
        /// Returns a bool representing if the classic address is valid.
        /// </summary>
        /// <param name="address">Classic address to validate.</param>
        /// <returns>A bool representing if the classic address is valid.</returns>
        public static bool IsValidClassicAddress(string address) => B58.IsValid(address, AccountID);
    }
}
