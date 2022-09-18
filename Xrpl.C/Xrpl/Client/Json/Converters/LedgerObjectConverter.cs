using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Common;



namespace Xrpl.Client.Json.Converters
{
    public class LOConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public BaseLedgerEntry Create(Type objectType, JObject jObject)
        {
            string ledgerEntryType = jObject.Property("LedgerEntryType").Value.ToString();
            switch (ledgerEntryType)
            {
                case "AccountRoot":
                    return new LOAccountRoot();
                case "Amendments":
                    return new LOAmendments();
                case "DirectoryNode":
                    return new LODirectoryNode();
                case "Escrow":
                    return new LOEscrow();
                case "FeeSettings":
                    return new LOFeeSettings();
                case "LedgerHashes":
                    return new LOLedgerHashes();
                case "Offer":
                    return new LOOffer();
                case "PayChannel":
                    return new LOPayChannel();
                case "RippleState":
                    return new LORippleState();
                case "SignerList":
                    return new LOSignerList();
                case "Ticket":
                    return new LOTicket();
                case "Check":
                    return new LOCheck();
                case "DepositPreauth":
                    return new LODepositPreauth();
            }
            throw new Exception("Can't create ledger type" + ledgerEntryType);
        }


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
