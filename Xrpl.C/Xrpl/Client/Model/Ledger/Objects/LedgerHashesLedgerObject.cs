using System.Collections.Generic;

namespace Xrpl.Client.Model.Ledger.Objects
{
    public class LedgerHashesLedgerObject : BaseRippleLedgerObject
    {
        public LedgerHashesLedgerObject()
        {
            LedgerEntryType = LedgerEntryType.LedgerHashes;
        }

        public uint FirstLedgerSequence { get; set; }

        public uint LastLedgerSequence { get; set; }

        public List<string> Hashes { get; set; }

        public uint Flags { get; set; }
    }
}
