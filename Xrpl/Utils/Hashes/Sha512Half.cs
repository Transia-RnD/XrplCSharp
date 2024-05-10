

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/sha512Half.ts

using Xrpl.BinaryCodec.Hashing;

namespace Xrpl.Utils.Hashes
{
    public static class Sha512HalfUtil
    {
        public const int HASH_SIZE = 64;
        /// <summary>
        /// Compute a sha512Half Hash of a hex string.
        /// </summary>
        /// <param name="hex"> Hex string to hash.</param>
        /// <returns>Hash of hex.</returns>
        public static string Sha512Half(this string hex)
        {
            return Sha512.Half(input: hex.FromHex()).ToHex();
        }
    }
}

