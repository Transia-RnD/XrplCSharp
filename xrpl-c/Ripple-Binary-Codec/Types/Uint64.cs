using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Util;

namespace Ripple.Core.Types
{
    public class Uint64 : Uint<ulong>
    {
        public Uint64(ulong value) : base(value)
        {
        }
        public override byte[] ToBytes()
        {
            return Bits.GetBytes(Value);
        }

        public override string ToString()
        {
            return B16.Encode(ToBytes());
        }

        public static Uint64 FromJson(JToken token)
        {
            return Bits.ToUInt64(B16.Decode(token.ToString()), 0);
        }
        public static implicit operator Uint64(ulong v)
        {
            return new Uint64(v);
        }

        public override JToken ToJson()
        {
            return ToString();
        }

        public static Uint64 FromParser(BinaryParser parser, int? hint=null)
        {
            return Bits.ToUInt64(parser.Read(8), 0);
        }
    }
}