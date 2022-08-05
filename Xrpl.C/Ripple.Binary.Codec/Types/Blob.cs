using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class Blob : ISerializedType
    {
        public readonly byte[] Buffer;
        private Blob(byte[] decode) => Buffer = decode;
        public static Blob FromHex(string value) => B16.Decode(value);

        public static implicit operator Blob(byte[] value) => new(value);

        public static implicit operator Blob(JToken token) => FromJson(token);

        public static Blob FromJson(JToken token) => FromHex(token.ToString());

        public void ToBytes(IBytesSink sink) => sink.Put(Buffer);

        public JToken ToJson() => ToString();

        public override string ToString() => B16.Encode(Buffer);

        public static Blob FromParser(BinaryParser parser, int? hint=null)
        {
            Debug.Assert(hint != null, "hint != null");
            return parser.Read((int) hint);
        }

        public static Blob FromString(string blob, System.Text.Encoding encoding) => new Blob(encoding.GetBytes(blob));

        public static Blob FromAscii(string blob) => FromString(blob, System.Text.Encoding.ASCII);
    }
}