using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Models;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/SignerList.ts

namespace Xrpl.Models.Ledger
{
    public class LOSignerList : BaseLedgerEntry
    {
        public LOSignerList()
        {
            LedgerEntryType = LedgerEntryType.SignerList;
        }

        public uint Flags { get; set; }

        public string OwnerNode { get; set; }

        public uint SignerQuorum { get; set; }

        public List<SignerEntry> SignerEntries { get; set; }

        public uint SignerListId { get; set; }

        public string PreviousTxnID { get; set; }

        public uint PreviousTxnLgrSeq { get; set; }
    }

    public class SignerEntry
    {
        public string Account { get; set; }

        public ushort SignerWeight { get; set; }
    }
}
