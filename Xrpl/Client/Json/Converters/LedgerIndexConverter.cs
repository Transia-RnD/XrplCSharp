using System;
using Newtonsoft.Json;
using Xrpl.Models.Common;

namespace Xrpl.Client.Json.Converters
{
    /// <summary>
    /// <see cref="LedgerIndex"/> json converter
    /// </summary>
    public class LedgerIndexConverter : JsonConverter
    {
        /// <summary>
        /// write <see cref="LedgerIndex"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"><see cref="LedgerIndex"/> object</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Cannot write this object type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is LedgerIndex ledger_index)
            {
                if (ledger_index.Index.HasValue)
                {
                    writer.WriteValue(ledger_index.Index.Value);
                }
                else
                {
                    writer.WriteValue(ledger_index.LedgerIndexType.ToString().ToLower());
                }
            }
            else
            {
                throw new Exception("Cannot convert this object type");
            }
        }

        /// <summary> read <see cref="LedgerIndex"/> from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="LedgerIndex"/> </returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(LedgerIndex);
    }
}
