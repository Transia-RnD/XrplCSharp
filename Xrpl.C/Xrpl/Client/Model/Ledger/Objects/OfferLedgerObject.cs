using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class OfferLedgerObject : BaseRippleLedgerObject
    {
        public OfferLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.Offer;
        }

        public string Account { get; set; }

        public OfferFlags Flags { get; set; }
        
        public uint Sequence { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }

        public string BookDirectory { get; set; }

        public string BookNode { get; set; }

        public string OwnerNode { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}
