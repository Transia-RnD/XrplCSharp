using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;

namespace Ripple.Binary.Codec.Types
{
    public class Vector256 : List<Hash256>, ISerializedType
    {
        private Vector256(IEnumerable<Hash256> enumerable) : base (enumerable) {}

        public Vector256()
        {
        }

        public void ToBytes(IBytesSink sink)
        {
            foreach (var hash in this)
            {
                hash.ToBytes(sink);
            }
        }

        public JToken ToJson()
        {
            var arr = new JArray();
            foreach (var hash in this)
            {
                arr.Add(hash.ToJson());
            }
            return arr;
        }

        public static Vector256 FromJson(JToken token) => new Vector256(token.Select(Hash256.FromJson));

        public static Vector256 FromParser(BinaryParser parser, int? hint=null)
        {
            var vec = new Vector256();
            hint ??= parser.Size - parser.Pos();
            for (var i = 0; i < hint / 32; i++)
            {
                vec.Add(Hash256.FromParser(parser));
            }
            return vec;
        }
    }
}