using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Models.Ledger;

namespace Xrpl.Client.Json.Converters
{
    /// <summary>
    /// <see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/>  converter
    /// </summary>
    public class LedgerBinaryConverter : JsonConverter
    {
        /// <summary>
        /// write <see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"><see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/> </param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Cannot write this object type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// create <see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/> 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject">json object LedgerEntity</param>
        /// <returns></returns>
        public object Create(Type objectType, JObject jObject)
        {
            JToken ledgerDataToken = jObject.Property("ledger_data");
            return ledgerDataToken == null
                ? new LedgerEntity()
                : new LedgerBinaryEntity();
        }

        /// <summary> read <see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/>  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="LedgerEntity"/> or  <see cref="LedgerBinaryEntity"/> </returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
