using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

using System.Diagnostics.Contracts;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash-128.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// Hash with a width of 128 bits
    /// </summary>
    public class Hash128 : Hash
    {
        /// <inheritdoc />
        public Hash128(byte[] buffer) : base(buffer)
        {
            Contract.Assert(buffer.Length == 16, "buffer should be 16 bytes");
        }
        /// <summary> create instance from json object </summary>
        /// <param name="token">json object</param>
        public static Hash128 FromJson(JToken token) => new Hash128(B16.Decode(token.ToString()));

        /// <summary> create instance from binary parser</summary>
        /// <param name="parser">parser</param>
        /// <param name="hint"></param>
        public static Hash128 FromParser(BinaryParser parser, int? hint=null) => new Hash128(parser.Read(16));
    }
}
