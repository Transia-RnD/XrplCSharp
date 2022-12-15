using Newtonsoft.Json.Linq;
using System.Diagnostics.Contracts;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash-160.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 160 bits
    /// </summary>
    public class Hash160 : Hash
    {
        /// <summary>
        /// Width of the hash
        /// </summary>
        public static readonly int Width = 20;

        /// <summary>
        /// Zero hash
        /// </summary>
        public static readonly Hash160 Zero160 = new Hash160(new byte[Width]);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bytes">Bytes</param>
        public Hash160(byte[] bytes) : base(bytes ?? Zero160.Bytes)
        {
            if (bytes != null && bytes.Length == 0)
            {
                bytes = Zero160.Bytes;
            }
        }
    }
}