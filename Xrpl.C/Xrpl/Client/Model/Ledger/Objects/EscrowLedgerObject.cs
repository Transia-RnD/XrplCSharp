using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class EscrowLedgerObject : BaseRippleLedgerObject
    {
        public EscrowLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.Escrow;
        }

        public string Account { get; set; }

        public string Destination { get; set; }

        public string Amount { get; set; }

        //https://tools.ietf.org/html/draft-thomas-crypto-conditions-02#section-8.1
        public string Condition { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        public uint? SourceTag { get; set; }

        public uint? DestinationTag { get; set; }

        public string OwnerNode { get; set; }

        public string DestinationNode { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }
    }
}
