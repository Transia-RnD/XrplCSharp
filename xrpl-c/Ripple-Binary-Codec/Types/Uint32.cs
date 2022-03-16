using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Util;

namespace Ripple.Core.Types
{
    public class Uint32 : Uint<uint>
    {
        public Uint32(uint value) : base(value)
        {
        }
        public static Uint32 FromJson(JToken token)
        {
            return (uint)token;
        }
        public static implicit operator Uint32(uint v)
        {
            return new Uint32(v);
        }
        public static implicit operator uint(Uint32 v)
        {
            return v.Value;
        }
        public override byte[] ToBytes()
        {
            return Bits.GetBytes(Value);
        }

        public static Uint32 FromParser(BinaryParser parser, int? hint=null)
        {
            return Bits.ToUInt32(parser.Read(4), 0);
        }
    }
}