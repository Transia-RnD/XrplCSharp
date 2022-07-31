using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class PayChannelLedgerObject : BaseRippleLedgerObject
    {
        public PayChannelLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.PayChannel;
        }

        public uint Flags { get; set; }

        public string Account { get; set; }

        public string Destination { get; set; }

        public string Amount { get; set; }

        public string Balance { get; set; }

        public string PublicKey { get; set; }

        public uint SettleDelay { get; set; }

        public string OwnerNode { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        public uint SourceTag { get; set; }

        public uint DestinationTag { get; set; }
    }
}
