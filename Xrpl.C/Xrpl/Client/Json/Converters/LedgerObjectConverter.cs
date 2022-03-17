using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client.Model.Ledger.Objects;

namespace Xrpl.Client.Json.Converters
{
    public class LedgerObjectConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public BaseRippleLedgerObject Create(Type objectType, JObject jObject)
        {
            string ledgerEntryType = jObject.Property("LedgerEntryType").Value.ToString();
            switch (ledgerEntryType)
            {
                case "AccountRoot":
                    return new AccountRootLedgerObject();
                case "Amendments":
                    return new AmendmentsLedgerObject();
                case "DirectoryNode":
                    return new DirectoryNodeLedgerObject();
                case "Escrow":
                    return new EscrowLedgerObject();
                case "FeeSettings":
                    return new FeeSettingsLedgerObject();
                case "LedgerHashes":
                    return new LedgerHashesLedgerObject();
                case "Offer":
                    return new OfferLedgerObject();
                case "PayChannel":
                    return new PayChannelLedgerObject();
                case "RippleState":
                    return new RippleStateLedgerObject();
                case "SignerList":
                    return new SignerListLedgerObject();
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
