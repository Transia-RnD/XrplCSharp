using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

using Xrpl.Models.Ledger;



namespace Xrpl.ClientLib.Json.Converters
{
    /// <summary>
    /// <see cref="BaseLedgerEntry"/> json converter
    /// </summary>
    public class LOConverter : JsonConverter
    {
        /// <summary>
        /// write <see cref="BaseLedgerEntry"/>  to json object
        /// </summary>
        /// <param name="writer">writer</param>
        /// <param name="value"> <see cref="BaseLedgerEntry"/>  value</param>
        /// <param name="serializer">json serializer</param>
        /// <exception cref="NotSupportedException">Can't create ledger type</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// create <see cref="BaseLedgerEntry"/> 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject">json object LedgerEntity</param>
        /// <returns></returns>
        public BaseLedgerEntry Create(Type objectType, JObject jObject)
        {
            string ledgerEntryType = jObject.Property("LedgerEntryType")?.Value.ToString();
            return ledgerEntryType switch
            {
                "AccountRoot" => new LOAccountRoot(),
                "Amendments" => new LOAmendments(),
                "DirectoryNode" => new LODirectoryNode(),
                "Escrow" => new LOEscrow(),
                "FeeSettings" => new LOFeeSettings(),
                "LedgerHashes" => new LOLedgerHashes(),
                "Offer" => new LOOffer(),
                "PayChannel" => new LOPayChannel(),
                "RippleState" => new LORippleState(),
                "SignerList" => new LOSignerList(),
                "Ticket" => new LOTicket(),
                "Check" => new LOCheck(),
                "DepositPreauth" => new LODepositPreauth(),
                _ => throw new Exception("Can't create ledger type" + ledgerEntryType)
            };
        }


        /// <summary> read <see cref="BaseLedgerEntry"/>  from json object </summary>
        /// <param name="reader">json reader</param>
        /// <param name="objectType">object type</param>
        /// <param name="existingValue">object value</param>
        /// <param name="serializer">json serializer</param>
        /// <returns><see cref="BaseLedgerEntry"/> </returns>
        /// <exception cref="NotSupportedException">Cannot convert value</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
