using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Util;

namespace Ripple.Core.Types
{
    public class Hash128 : Hash
    {
        public Hash128(byte[] buffer) : base(buffer)
        {
            Contract.Assert(buffer.Length == 16, "buffer should be 16 bytes");
        }

        public static Hash128 FromJson(JToken token)
        {
            return new Hash128(B16.Decode(token.ToString()));
        }

        public static Hash128 FromParser(BinaryParser parser, int? hint=null)
        {
            return new Hash128(parser.Read(16));
        }
    }
}
