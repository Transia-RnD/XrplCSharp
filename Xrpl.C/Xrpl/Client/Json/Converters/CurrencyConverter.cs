using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Json.Converters
{
    public class CurrencyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Currency)
            {
                var currency = (Currency) value;
                if (currency.CurrencyCode == "XRP")
                {
                    writer.WriteValue(currency.Value);
                }
                else
                {
                    var t = JToken.FromObject(value);
                    t.WriteTo(writer);
                }
            }
            else
            {
                throw new NotSupportedException("Cannot write this object type");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null: return null;
                case JsonToken.String:
                {
                    var currency = new Currency
                    {
                        CurrencyCode = "XRP",
                        Value = reader.Value.ToString()
                    };
                    return currency;
                }
                case JsonToken.StartObject: return serializer.Deserialize<Currency>(reader);
                default: throw new NotSupportedException("Cannot convert value " + objectType);
            }
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Currency);
    }
}
