using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Uint32 : Uint<uint>
    {
        public Uint32(uint value) : base(value)
        {
        }
        public static Uint32 FromJson(JToken token) => (uint)token;

        public static implicit operator Uint32(uint v) => new Uint32(v);

        public static implicit operator uint(Uint32 v) => v.Value;

        public override byte[] ToBytes() => Bits.GetBytes(Value);

        public static Uint32 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt32(parser.Read(4), 0);
    }
}