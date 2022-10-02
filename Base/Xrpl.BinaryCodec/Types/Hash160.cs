using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Util;

using System.Diagnostics.Contracts;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash-160.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// Hash with a width of 160 bits
    /// </summary>
    public class Hash160: Hash
    {
        /// <inheritdoc />
        public Hash160(byte[] buffer) : base(buffer)
        {
            Contract.Assert(buffer.Length == 20, "buffer should be 20 bytes");
        }
        /// <summary> create instance from json object </summary>
        /// <param name="token">json object</param>
        public static Hash160 FromJson(JToken token)
        {
            return new Hash160(B16.Decode(token.ToString()));
        }
        /// <summary> create instance from binary parser</summary>
        /// <param name="parser">parser</param>
        /// <param name="hint"></param>
        public static Hash160 FromParser(BinaryParser parser, int? hint = null)
        {
            return new Hash160(parser.Read(20));
        }
    }
}