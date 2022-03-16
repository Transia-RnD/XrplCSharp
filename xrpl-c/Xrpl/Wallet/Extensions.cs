using Ripple.Core.Hashing;
using Ripple.Core.Util;

namespace Ripple.TxSigning
{
    internal static class Extensions
    {
        internal static byte[] Bytes(this HashPrefix hp)
        {
            return Bits.GetBytes((uint)hp);
        }
    }
}