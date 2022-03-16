using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Util;

namespace Ripple.Core.Types
{
    public class Blob : ISerializedType
    {
        public readonly byte[] Buffer;
        private Blob(byte[] decode)
        {
            this.Buffer = decode;
        }
        public static Blob FromHex(string value)
        {
            return B16.Decode(value);
        }
        public static implicit operator Blob(byte[] value)
        {
            return new Blob(value);
        }
        public static implicit operator Blob(JToken token)
        {
            return FromJson(token);
        }

        public static Blob FromJson(JToken token)
        {
            return FromHex(token.ToString());
        }

        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Buffer);
        }

        public JToken ToJson()
        {
            return ToString();
        }

        public override string ToString()
        {
            return B16.Encode(Buffer);
        }

        public static Blob FromParser(BinaryParser parser, int? hint=null)
        {
            Debug.Assert(hint != null, "hint != null");
            return parser.Read((int) hint);
        }

        public static Blob FromString(string blob, System.Text.Encoding encoding)
        {
            return new Blob(encoding.GetBytes(blob));
        }

        public static Blob FromAscii(string blob)
        {
            return FromString(blob, System.Text.Encoding.ASCII);
        }
    }
}