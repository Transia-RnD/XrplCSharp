using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xrpl.ClientLib.Json.Converters
{
    public class StringOrArrayConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                
            }

            if (value is string)
            {
                writer.WriteValue(value);
            } else if (value is List<string>)
            {
                
            } else if (value is Array)
            {
                
            }
            throw new Exception("Cannot write value");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                return reader.Value;
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<List<string>>(reader);
            }

            throw new Exception("Cannot convert value");
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(string) || objectType == typeof(List<string>) || objectType == typeof(Array))
                return true;
            return false;
        }
    }
}
