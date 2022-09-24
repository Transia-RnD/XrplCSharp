using System;
using Newtonsoft.Json;

namespace Xrpl.ClientLib.Json.Converters
{
    /// <summary>
    /// generic object json converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericStringConverter<T> : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<T>(reader);
            }

            T item = JsonConvert.DeserializeObject<T>(reader.Value.ToString());
            return item;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(T);
    }
}
