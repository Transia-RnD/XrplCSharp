using System;
using Newtonsoft.Json;
using Xrpl.Models.Transactions;

namespace Xrpl.Client.Json.Converters
{
    /// <summary> <see cref="Meta"/> json converter </summary>
    public class MetaBinaryConverter : JsonConverter
    {
        /// <summary>
        /// write <see cref="Meta"/> to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"><see cref="Meta"/> value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Cannot write this object type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        /// <summary> read  <see cref="Meta"/> from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns> <see cref="Meta"/></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            Meta meta = serializer.Deserialize<Meta>(reader);
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    if (meta is not null) meta.MetaBlob = reader.Value?.ToString();
                    break;
                case JsonToken.StartObject:
                    meta = serializer.Deserialize<Meta>(reader);
                    break;
            }
            return meta;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
