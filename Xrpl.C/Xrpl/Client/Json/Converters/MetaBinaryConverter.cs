using System;
using Newtonsoft.Json;
using Xrpl.Client.Models.Transactions;

namespace Xrpl.Client.Json.Converters
{
    public class MetaBinaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            var meta = serializer.Deserialize<Meta>(reader);
            switch (reader.TokenType)
            {
                case JsonToken.String: meta.MetaBlob = reader.Value.ToString();
                    break;
                case JsonToken.StartObject: meta = serializer.Deserialize<Meta>(reader);
                    break;
            }
            return meta;
        }

        public override bool CanConvert(Type objectType) => true;
    }
}
