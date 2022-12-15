using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/st-array.ts

namespace Xrpl.BinaryCodec.Types
{
    public class STArray : SerializedType
    {
        private static readonly byte[] ARRAY_END_MARKER = new byte[] { 0xf1 };
        private const string ARRAY_END_MARKER_NAME = "ArrayEndMarker";
        private static readonly byte[] OBJECT_END_MARKER = new byte[] { 0xe1 };

        public static STArray FromParser(BinaryParser parser)
        {
            var bytes = new List<byte[]>();

            while (!parser.End())
            {
                var field = parser.ReadField();
                if (field.Name == ARRAY_END_MARKER_NAME)
                {
                    break;
                }

                bytes.Add(field.Header);
                bytes.Add(parser.ReadFieldValue(field).ToBytes());
                bytes.Add(OBJECT_END_MARKER);
            }

            bytes.Add(ARRAY_END_MARKER);
            return new STArray(bytes.SelectMany(x => x).ToArray());
        }

        public static STArray From<T>(T value) where T : STArray, IEnumerable<JsonObject>
        {
            if (value is STArray)
            {
                return value;
            }

            if (value.Any() && value.First() is JsonObject)
            {
                var bytes = new List<byte[]>();
                foreach (var obj in value)
                {
                    bytes.Add(STObject.From(obj).ToBytes());
                }

                bytes.Add(ARRAY_END_MARKER);
                return new STArray(bytes.SelectMany(x => x).ToArray());
            }

            throw new Exception("Cannot construct STArray from value given");
        }

        public JsonObject[] ToJson()
        {
            var result = new List<JsonObject>();

            var arrayParser = new BinaryParser(this.ToString());

            while (!arrayParser.End())
            {
                var field = arrayParser.ReadField();
                if (field.Name == ARRAY_END_MARKER_NAME)
                {
                    break;
                }

                var outer = new JsonObject();
                outer[field.Name] = STObject.FromParser(arrayParser).ToJson();
                result.Add(outer);
            }

            return result.ToArray();
        }
    }
}