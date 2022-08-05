using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Models.Ledger;


namespace Xrpl.Client.Json.Converters
{
    public class LOConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public BaseRippleLO Create(Type objectType, JObject jObject)
        {
            var ledgerEntryType = jObject.Property("LedgerEntryType").Value.ToString();
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
                _ => throw new Exception("Can't create ledger type" + ledgerEntryType)
            };
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
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
