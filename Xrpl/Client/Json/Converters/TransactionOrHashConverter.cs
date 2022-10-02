using System;
using Newtonsoft.Json;
using Xrpl.Models.Ledger;
using Xrpl.Models.Transaction;

namespace Xrpl.Client.Json.Converters
{
    /// <summary> Hash Or Transaction json converter </summary>
    public class TransactionOrHashConverter : JsonConverter
    {
        /// <summary>
        /// write  <see cref="HashOrTransaction"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"> <see cref="HashOrTransaction"/> value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Cannot write this object type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary> read  <see cref="HashOrTransaction"/>  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="HashOrTransaction"/></returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            HashOrTransaction hashOrTransaction = new HashOrTransaction();


            if (reader.TokenType == JsonToken.String)
            {
                hashOrTransaction.TransactionHash = reader.Value.ToString();
            }
            else
            {
                hashOrTransaction.Transaction = serializer.Deserialize<TransactionResponseCommon>(reader);
            }

            return hashOrTransaction;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
