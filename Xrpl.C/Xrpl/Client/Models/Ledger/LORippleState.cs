using System;
using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/RippleState.ts

namespace Xrpl.Client.Models.Ledger
{
    [Flags]
    public enum RippleStateFlags
    {
        lsfLowReserve = 65536,
        lsfHighReserve = 131072,
        lsfLowAuth = 262144,
        lsfHighAuth = 524288,
        lsfLowNoRipple = 1048576,
        lsfHighNoRipple = 2097152,
        lsfLowFreeze = 4194304,
        lsfHighFreeze = 8388608
    }
    public class LORippleState : BaseRippleLO
    {
        public LORippleState()
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
