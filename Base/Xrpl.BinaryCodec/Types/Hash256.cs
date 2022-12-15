using Newtonsoft.Json.Linq;
using System.Diagnostics.Contracts;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/hash-256.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 256 bits
    /// </summary>
    public class Hash256 : Hash
    {
        public static readonly int Width = 32;
        public static readonly Hash256 Zero256 = new Hash256(new byte[Width]);

        public Hash256(byte[] bytes) : base(bytes ?? Zero256.Bytes)
        {
        }
    }
}