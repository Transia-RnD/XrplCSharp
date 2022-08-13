using System.Collections.Generic;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerHashes.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LOLedgerHashes : BaseRippleLO
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
