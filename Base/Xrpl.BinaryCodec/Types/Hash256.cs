using Newtonsoft.Json.Linq;
using System.Diagnostics.Contracts;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash-256.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 256 bits
    /// </summary>
    public class Hash256 : Hash
    {
        public static readonly Hash256 Zero = new Hash256(new byte[32]);

        /// <inheritdoc />
        public Hash256(byte[] buffer) : base(buffer)
        {
            Contract.Assert(buffer.Length == 32, "buffer should be 32 bytes");
        }   
        /// <summary> create instance from json object </summary>
        /// <param name="token">json object</param>
        public static Hash256 FromJson(JToken token) => FromHex((string) token);

        /// <summary> create instance from hex string</summary>
        /// <param name="token">string hex token</param>
        public static Hash256 FromHex(string token) => new Hash256(B16.Decode(token));

        /// <summary> create instance from binary parser</summary>
        /// <param name="parser">parser</param>
        /// <param name="hint"></param>
        public static Hash256 FromParser(BinaryParser parser, int? hint = null) => new Hash256(parser.Read(32));

        public int Nibblet(int depth)
        {
            var byteIx = depth > 0 ? depth / 2 : 0;
            int b = Buffer[byteIx];
            if (depth % 2 == 0)
            {
                b = (b & 0xF0) >> 4;
            }
            else
            {
                b = b & 0x0F;
            }
            return b;
        }
    }
}