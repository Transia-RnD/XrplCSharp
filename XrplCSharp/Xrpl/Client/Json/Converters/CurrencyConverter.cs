using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Models.Common;

namespace Xrpl.ClientLib.Json.Converters
{
    /// <summary> currency json converter </summary>
    public class CurrencyConverter : JsonConverter
    {
        /// <summary>
        /// write  <see cref="Currency"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"> <see cref="Currency"/> value</param>
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
        /// <summary> read  <see cref="Currency"/>  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="Currency"/></returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            Debug.WriteLine("[CONVERTER] Read Json");
            Debug.WriteLine($"[CONVERTER] Token Type: {reader.TokenType}");
            return reader.TokenType switch
            {
                JsonToken.Null => null,
                JsonToken.String => new Currency
                {
                    CurrencyCode = "XRP",
                    Value = reader.Value?.ToString()
                },

                JsonToken.StartObject => serializer.Deserialize<Currency>(reader),
                _ => throw new NotSupportedException("Cannot convert value " + objectType)
            };
        }
        /// <summary> Can convert object to currency </summary>
        /// <param name="objectType">object type</param>
        /// <returns>bool result</returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(Currency);
    }
}
