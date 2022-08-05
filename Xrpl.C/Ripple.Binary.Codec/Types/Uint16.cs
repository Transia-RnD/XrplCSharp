using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Uint16 : Uint<ushort>
    {
        public Uint16(ushort value) : base(value)
        {
        }
        public static Uint16 FromJson(JToken token) => (ushort) token;

        public static implicit operator Uint16(ushort v) => new Uint16(v);

        public override byte[] ToBytes() => Bits.GetBytes(Value);

        public static Uint16 FromParser(BinaryParser parser, int? hint=null) => Bits.ToUInt16(parser.Read(2), 0);
    }
}