using Newtonsoft.Json;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class RippleStateLedgerObject : BaseRippleLedgerObject
    {
        public RippleStateLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.RippleState;
        }

        public RippleStateFlags Flags { get; set; }

        public Currency Balance { get; set; }

        public Currency LowLimit { get; set; }

        public Currency HighLimit { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }

        public string LowNode { get; set; }

        public string HighNode { get; set; }

        public uint? LowQualityIn { get; set; }

        public uint? LowQualityOut { get; set; }

        public uint? HighQualityIn { get; set; }

        public uint? HighQualityOut { get; set; }
    }
}
