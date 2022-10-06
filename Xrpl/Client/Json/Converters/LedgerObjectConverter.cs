using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Models;
using Xrpl.Models.Ledger;

namespace Xrpl.Client.Json.Converters
{
    /// <summary>
    /// <see cref="BaseLedgerEntry"/> json converter
    /// </summary>
    public class LOConverter : JsonConverter
    {
        /// <summary>
        /// Convert ledger entry json object to standard type
        /// </summary>
        /// <param name="type">field type</param>
        /// <param name="field">current json object</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BaseLedgerEntry GetBaseRippleLO(LedgerEntryType type, object field) =>
            type switch
            {
                LedgerEntryType.AccountRoot => JsonConvert.DeserializeObject<LOAccountRoot>($"{field}"),
                LedgerEntryType.Amendments => JsonConvert.DeserializeObject<LOAmendments>($"{field}"),
                LedgerEntryType.DirectoryNode => JsonConvert.DeserializeObject<LODirectoryNode>($"{field}"),
                LedgerEntryType.Escrow => JsonConvert.DeserializeObject<LOEscrow>($"{field}"),
                LedgerEntryType.FeeSettings => JsonConvert.DeserializeObject<LOFeeSettings>($"{field}"),
                LedgerEntryType.LedgerHashes => JsonConvert.DeserializeObject<LOLedgerHashes>($"{field}"),
                LedgerEntryType.Offer => JsonConvert.DeserializeObject<LOOffer>($"{field}"),
                LedgerEntryType.PayChannel => JsonConvert.DeserializeObject<LOPayChannel>($"{field}"),
                LedgerEntryType.RippleState => JsonConvert.DeserializeObject<LORippleState>($"{field}"),
                LedgerEntryType.SignerList => JsonConvert.DeserializeObject<LOSignerList>($"{field}"),
                //LedgerEntryType.NegativeUNL => expr,
                //LedgerEntryType.NFTokenOffer => expr,
                //LedgerEntryType.NFTokenPage => expr,
                //LedgerEntryType.Ticket => expr,
                //LedgerEntryType.Check => expr,
                //LedgerEntryType.DepositPreauth => expr,
                _ => throw new ArgumentOutOfRangeException()
            };

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
            switch (objectType.Name)
            {
                case "LOAccountRoot":
                    return new LOAccountRoot();
                case "LOAmendments":
                    return new LOAmendments();
                case "LODirectoryNode":
                    return new LODirectoryNode();
                case "LOEscrow":
                    return new LOEscrow();
                case "LOFeeSettings":
                    return new LOFeeSettings();
                case "LOLedgerHashes":
                    return new LOLedgerHashes();
                case "LOOffer":
                    return new LOOffer();
                case "LOPayChannel":
                    return new LOPayChannel();
                case "LORippleState":
                    return new LORippleState();
                case "LOSignerList":
                    return new LOSignerList();
                // case "Ticket":
                //     return new LOTicket();
            }

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
                //"NegativeUNL" => new NegativeUNL(),
                //"NFTokenOffer" => new NFTokenOffer(),
                //"NFTokenPage" => new NFTokenPage(),
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
