using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Json.Converters
{
    /// <summary> currency json converter </summary>
    public class CurrencyConverter : JsonConverter
    {
        /// <summary>
        /// write currency to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value">currency value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Cannot write this object type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Currency currency)
            {
                if (currency.CurrencyCode == "XRP")
                {
                    writer.WriteValue(currency.Value);
                }
                else
                {
                    JToken t = JToken.FromObject(currency);
                    t.WriteTo(writer);
                }
            }
            else
            {
                throw new NotSupportedException("Cannot write this object type");
            }
        }
        /// <summary> read currency from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns>Currency<see cref="Xrpl.Client.Models.Common.Currency"/> </returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            Debug.WriteLine("[CONVERTER] Read Json");
            Debug.WriteLine($"[CONVERTER] Token Type: {reader.TokenType}");
            switch (reader.TokenType)
            {
                case JsonToken.Null: return null;
                case JsonToken.String:
                {
                    Currency currency = new Currency
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
        /// <summary> Can convert object to currency </summary>
        /// <param name="objectType">object type</param>
        /// <returns>bool result</returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(Currency);
    }
}
