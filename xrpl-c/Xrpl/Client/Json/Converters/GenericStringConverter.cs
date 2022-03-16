using System;
using Newtonsoft.Json;

namespace RippleDotNet.Json.Converters
{
    public class GenericStringConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<T>(reader);
            }

            T item = JsonConvert.DeserializeObject<T>(reader.Value.ToString());
            return item;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(T))
                return true;
            return false;
        }
    }
}
