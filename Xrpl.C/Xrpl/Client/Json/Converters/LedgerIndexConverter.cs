using System;
using Newtonsoft.Json;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Json.Converters
{
    public class LedgerIndexConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is LedgerIndex)
            {
                var ledgerIndex = (LedgerIndex) value;
                if (ledgerIndex.Index.HasValue)
                {
                    writer.WriteValue(ledgerIndex.Index.Value);
                }
                else
                {
                    writer.WriteValue(ledgerIndex.LedgerIndexType.ToString().ToLower());
                }
            }
            else
            {
                throw new Exception("Cannot convert this object type");
            }            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(LedgerIndex);
    }
}
