using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Hash128 : Hash
    {
        public Hash128(byte[] buffer) : base(buffer) =>
            Contract.Assert(buffer.Length == 16, "buffer should be 16 bytes");

        public static Hash128 FromJson(JToken token) => new Hash128(B16.Decode(token.ToString()));

        public static Hash128 FromParser(BinaryParser parser, int? hint=null) => new Hash128(parser.Read(16));
    }
}
