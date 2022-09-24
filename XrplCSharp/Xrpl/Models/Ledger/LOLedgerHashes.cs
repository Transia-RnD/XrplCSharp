using System.Collections.Generic;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerHashes.ts

namespace Xrpl.Models.Ledger
{
    public class LOLedgerHashes : BaseLedgerEntry
    {
        public LOLedgerHashes()
        {
            LedgerEntryType = LedgerEntryType.LedgerHashes;
        }

        public uint FirstLedgerSequence { get; set; }

        public uint LastLedgerSequence { get; set; }

        public List<string> Hashes { get; set; }

        public uint Flags { get; set; }
    }
}
