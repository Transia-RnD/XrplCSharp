

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/sha512Half.ts

using Xrpl.BinaryCodec.Hashing;

namespace Xrpl.Utils.Hashes
{
    public class Sha512HalfUtil
    {
        static public string Sha512Half(string hex)
        {
            return Sha512.Half(input: hex.FromHex()).ToHex();
        }
    }
}

