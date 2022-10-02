using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.Util;

namespace Xrpl.Wallet
{
    internal static class Extensions
    {
        internal static byte[] Bytes(this HashPrefix hp)
        {
            return Bits.GetBytes((uint)hp);
        }
    }
}