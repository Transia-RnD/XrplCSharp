using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;
using Ripple.Core.Enums;

namespace Ripple.Core.Types
{
    public class StArray : List<StObject>, ISerializedType
    {
        public StArray(IEnumerable<StObject> collection) : base(collection)
        {
        }

        public StArray()
        {
        }

        public void ToBytes(IBytesSink sink)
        {
            foreach (var so in this)
            {
                so.ToBytes(sink);
            }
        }

        public JToken ToJson()
        {
            var arr = new JArray();
            foreach (var so in this)
            {
                arr.Add(so.ToJson());
            }
            return arr;
        }

        public static StArray FromJson(JToken token)
        {
            return new StArray(token.Select(StObject.FromJson));
        }

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