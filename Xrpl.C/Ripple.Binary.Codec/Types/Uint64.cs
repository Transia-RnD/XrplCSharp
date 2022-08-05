using Newtonsoft.Json.Linq;

using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Uint64 : Uint<ulong>
    {
        public Uint64(ulong value) : base(value)
        {
        }
        public override byte[] ToBytes() => Bits.GetBytes(Value);

        public override string ToString() => B16.Encode(ToBytes());

        public static Uint64 FromJson(JToken token) => Bits.ToUInt64(B16.Decode(token.ToString()), 0);

        public static implicit operator Uint64(ulong v) => new Uint64(v);

        public override JToken ToJson() => ToString();

        public static Uint64 FromParser(BinaryParser parser, int? hint = null) => Bits.ToUInt64(parser.Read(8), 0); //todo hint for what?
    }
}