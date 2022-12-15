using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Enums;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/st-object.ts

namespace Xrpl.BinaryCodec.Types
{
    public class STObject : SerializedType
    {
        const byte OBJECT_END_MARKER_BYTE = 0xe1;
        const string OBJECT_END_MARKER = "ObjectEndMarker";
        const string ST_OBJECT = "STObject";
        const string DESTINATION = "Destination";
        const string ACCOUNT = "Account";
        const string SOURCE_TAG = "SourceTag";
        const string DEST_TAG = "DestinationTag";

        private static JObject HandleXAddress(string field, string xAddress)
        {
            var decoded = XAddressToClassicAddress(xAddress);

            string tagName;
            if (field == DESTINATION) tagName = DEST_TAG;
            else if (field == ACCOUNT) tagName = SOURCE_TAG;
            else if (decoded.Tag != false)
                throw new Exception($"{field} cannot have an associated tag");

            return decoded.Tag != false
                ? new JObject { { field, decoded.ClassicAddress }, { tagName, decoded.Tag } }
                : new JObject { { field, decoded.ClassicAddress } };
        }

        private static void CheckForDuplicateTags(JObject obj1, JObject obj2)
        {
            if (!(obj1[SOURCE_TAG] == null || obj2[SOURCE_TAG] == null))
                throw new Exception("Cannot have Account X-Address and SourceTag");
            if (!(obj1[DEST_TAG] == null || obj2[DEST_TAG] == null))
                throw new Exception("Cannot have Destination X-Address and DestinationTag");
        }

        public static STObject FromParser(BinaryParser parser)
        {
            var list = new BytesList();
            var bytes = new BinarySerializer(list);

            while (!parser.End())
            {
                var field = parser.ReadField();
                if (field.Name == OBJECT_END_MARKER)
                {
                    break;
                }

                var associatedValue = parser.ReadFieldValue(field);

                bytes.WriteFieldAndValue(field, associatedValue);
                if (field.Type.Name == ST_OBJECT)
                {
                    bytes.Put(OBJECT_END_MARKER_BYTE);
                }
            }

            return new STObject(list.ToBytes());
        }

        public static STObject From<T>(T value, Func<object, bool> filter = null) where T : JsonObject
        {
            var list = new BytesList();
            var bytes = new BinarySerializer(list);

            var isUnlModify = false;

            var xAddressDecoded = value.ToDictionary().ToDictionary(kvp => kvp.Key, kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var sorted = xAddressDecoded.Keys.Select(f => Field.GetField(f)).Where(f => f != null && xAddressDecoded[f.Name] != null && f.IsSerialized).OrderBy(f => f.Ordinal).ToList();

            if (filter != null)
            {
                sorted = sorted.Where(filter).ToList();
            }

            foreach (var field in sorted)
            {
                var associatedValue = field.AssociatedType.From(xAddressDecoded[field.Name]);

                if (associatedValue == null)
                {
                    throw new TypeError($"Unable to interpret \"{field.Name}: {xAddressDecoded[field.Name]}\".");
                }

                if ((associatedValue as Bytes).Name == "UNLModify")
                {
                    isUnlModify = true;
                }

                var isUnlModifyWorkaround = field.Name == "Account" && isUnlModify;
                bytes.WriteFieldAndValue(field, associatedValue, isUnlModifyWorkaround);
                if (field.Type.Name == ST_OBJECT)
                {
                    bytes.Put(OBJECT_END_MARKER_BYTE);
                }
            }

            return new STObject(list.ToBytes());
        }

        public static STObject FromST<T>(T value, Func<object, bool> filter = null) where T : STObject
        {
            return value;
        }

        public JsonObject ToJson()
        {
            var objectParser = new BinaryParser(this.ToString());
            var accumulator = new JsonObject();

            while (!objectParser.End())
            {
                var field = objectParser.ReadField();
                if (field.Name == OBJECT_END_MARKER)
                {
                    break;
                }

                accumulator[field.Name] = objectParser.ReadFieldValue(field).ToJson();
            }

            return accumulator;
        }
    }
}