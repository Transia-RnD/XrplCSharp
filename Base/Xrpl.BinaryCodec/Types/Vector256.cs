using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/vector-256.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Class for serializing and deserializing vectors of Hash256
    /// </summary>
    public class Vector256 : List<Hash256>, ISerializedType
    {
        private Vector256(IEnumerable<Hash256> enumerable) : base (enumerable) {}
        /// <summary>
        /// Construct a Vector256
        /// </summary>
        public Vector256()
        {
        }

        /// <inheritdoc />
        public void ToBytes(IBytesSink sink)
        {
            foreach (var hash in this)
            {
                hash.ToBytes(sink);
            }
        }

        /// <inheritdoc />
        public JToken ToJson()
        {
            var arr = new JArray();
            foreach (var hash in this)
            {
                arr.Add(hash.ToJson());
            }
            return arr;
        }
        /// <summary> Deserialize Vector256 </summary>
        /// <param name="token">json token</param>
        /// <returns>Vector256 value</returns>
        public static Vector256 FromJson(JToken token) => new Vector256(token.Select(Hash256.FromJson));
        /// <summary>
        /// Construct a Vector256 from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read Vector256 from</param>
        /// <returns></returns>
        public static Vector256 FromParser(BinaryParser parser, int? hint=null)
        {
            var vec = new Vector256();
            hint ??= parser.Size - parser.Pos();
            for (int i = 0; i < hint / 32; i++)
            {
                vec.Add(Hash256.FromParser(parser));
            }
            return vec;
        }
    }
}