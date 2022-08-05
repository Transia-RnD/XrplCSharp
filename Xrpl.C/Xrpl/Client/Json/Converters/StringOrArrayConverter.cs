using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xrpl.Client.Json.Converters
{
    public class StringOrArrayConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                
            }


            switch (value)
            {
                case string: writer.WriteValue(value);
                    break;
                case List<string>: break;
                case Array: break;
            }
            throw new Exception("Cannot write value");
        }

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

        public override bool CanConvert(Type objectType) => objectType == typeof(string) || objectType == typeof(List<string>) || objectType == typeof(Array);
    }
}
