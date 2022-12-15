using Newtonsoft.Json.Linq;
using System.Diagnostics.Contracts;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash-128.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 128 bits
    /// </summary>
    public class Hash128 : Hash
    {
        public static readonly int Width = 16;
        public static readonly Hash128 ZERO_128 = new Hash128(new byte[Width]);

        public Hash128(byte[] bytes) : base(bytes ?? ZERO_128.Bytes)
        {
        }
    }
}
