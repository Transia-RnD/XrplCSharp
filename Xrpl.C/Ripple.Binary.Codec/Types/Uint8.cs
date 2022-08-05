using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;

namespace Ripple.Binary.Codec.Types
{
    public class Uint8 : Uint<byte>
    {
        public Uint8(byte value) : base(value)
        {
        }

        public override byte[] ToBytes() => new [] {Value};

        public static Uint8 FromJson(JToken token) => (byte) token;

        public static implicit operator Uint8(byte v) => new Uint8(v);

        public static Uint8 FromParser(BinaryParser parser, int? hint=null) => parser.ReadOne();
    }
}