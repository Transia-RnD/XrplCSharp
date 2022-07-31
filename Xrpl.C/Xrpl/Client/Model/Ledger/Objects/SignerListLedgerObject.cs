using System.Collections.Generic;

using Newtonsoft.Json;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class SignerListLedgerObject : BaseRippleLedgerObject
    {
        public SignerListLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.SignerList;
        }

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
