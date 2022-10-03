using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Enums;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/st-array.ts
//https://xrpl.org/serialization.html#array-fields

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Class for serializing and deserializing Arrays of Objects
    /// </summary>
    public class StArray : List<StObject>, ISerializedType
    {
        /// <summary>
        ///  Construct an STArray from an Array of JSON Objects
        /// </summary>
        /// <param name="collection"> Array of JSON Objects</param>
        public StArray(IEnumerable<StObject> collection) : base(collection)
        {
        }
        /// <summary>
        ///  Construct an STArray 
        /// </summary>
        public StArray()
        {
        }

        /// <inheritdoc />
        public void ToBytes(BytesList sink)
        {
            foreach (var so in this)
            {
                so.ToBytes(sink);
            }
        }

        /// <inheritdoc />
        public JToken ToJson()
        {
            var arr = new JArray();
            foreach (var so in this)
            {
                arr.Add(so.ToJson());
            }
            return arr;
        }
        /// <summary> Deserialize StArray </summary>
        /// <param name="token">json token</param>
        /// <returns></returns>
        public static StArray FromJson(JToken token)
        {
            return new StArray(token.Select(StObject.FromJson));
        }
        /// <summary>
        /// Construct a StArray from a BinaryParser
        /// </summary>
        /// <param name="parser">A BinaryParser to read StArray from</param>
        /// <returns></returns>
        public static StArray FromParser(BinaryParser parser, int? hint = null)
        {
            var stArray = new StArray();
            while (!parser.End())
            {
                var field = parser.ReadField();
                if (field == Field.ArrayEndMarker)
                {
                    break;
                }
                var outer = new StObject {
                    [(StObjectField) field] =
                        StObject.FromParser(parser) };
                stArray.Add(outer);
            }
            return stArray;
        }
    }
}