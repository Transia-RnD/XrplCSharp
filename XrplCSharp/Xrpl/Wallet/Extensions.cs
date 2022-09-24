using Xrpl.BinaryCodecLib.Hashing;
using Xrpl.BinaryCodecLib.Util;

namespace Xrpl.WalletLib
{
    internal static class Extensions
    {
        internal static byte[] Bytes(this HashPrefix hp)
        {
            return Bits.GetBytes((uint)hp);
        }
    }
}