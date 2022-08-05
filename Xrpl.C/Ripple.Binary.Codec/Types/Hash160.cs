using System.Diagnostics.Contracts;

using Newtonsoft.Json.Linq;

using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Hash160 : Hash
    {
        public Hash160(byte[] buffer) : base(buffer) =>
            Contract.Assert(buffer.Length == 20, "buffer should be 20 bytes");
        public static Hash160 FromJson(JToken token) => new Hash160(B16.Decode(token.ToString()));

        public static Hash160 FromParser(BinaryParser parser, int? hint = null) => new Hash160(parser.Read(20));
    }
}