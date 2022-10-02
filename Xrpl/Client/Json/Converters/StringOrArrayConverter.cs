using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xrpl.Client.Json.Converters
{
    /// <summary> string or array json converter </summary>
    public class StringOrArrayConverter : JsonConverter
    {
        /// <summary>
        /// write  string or array to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"> string or array value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="Exception">Cannot write value</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }

            switch (value)
            {
                case string:
                    writer.WriteValue(value);
                    break;
                case List<string>: break;
                case Array: throw new Exception("Cannot write value");
            }
        }

        /// <summary> read  string or array  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns>string or array </returns>
        /// <exception cref="Exception">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.Null => null,
                JsonToken.String => reader.Value,
                JsonToken.StartObject => serializer.Deserialize<List<string>>(reader),
                _ => throw new Exception("Cannot convert value")
            };
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
            => objectType == typeof(string) || objectType == typeof(List<string>) || objectType == typeof(Array);
    }
}
