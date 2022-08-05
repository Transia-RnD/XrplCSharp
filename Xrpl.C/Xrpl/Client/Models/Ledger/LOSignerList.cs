using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/SignerList.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LOSignerList : BaseRippleLO
    {
        public LOSignerList() => LedgerEntryType = LedgerEntryType.SignerList;

        public uint Flags { get; set; }

        public string OwnerNode { get; set; }

        public uint SignerQuorum { get; set; }

        public List<SignerEntry> SignerEntries { get; set; }

        public uint SignerListId { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }
    }

    public class SignerEntry
    {
        public string Account { get; set; }

        public ushort SignerWeight { get; set; }
    }
}
