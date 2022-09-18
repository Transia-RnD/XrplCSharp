using System;
using System.Diagnostics;
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
                Currency currency = (Currency) value;
                if (currency.CurrencyCode == "XRP")
                {
                    writer.WriteValue(currency.Value);
                }
                else
                {
                    JToken t = JToken.FromObject(value);
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
            Debug.WriteLine("[CONVERTER] Read Json");
            Debug.WriteLine($"[CONVERTER] Token Type: {reader.TokenType}");
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                Currency currency = new Currency();
                currency.CurrencyCode = "XRP";
                currency.Value = reader.Value.ToString();
                return currency;
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<Currency>(reader);
            }

            throw new NotSupportedException("Cannot convert value " + objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Currency))
                return true;
            return false;
        }
    }
}
