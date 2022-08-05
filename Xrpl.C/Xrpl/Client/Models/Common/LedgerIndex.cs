using Xrpl.Client.Models.Ledger;

namespace Xrpl.Client.Models.Common
{
    public class LedgerIndex
    {
        public LedgerIndex(uint index) => Index = index;

        public LedgerIndex(LedgerIndexType ledgerIndexType) => LedgerIndexType = ledgerIndexType;

        public uint? Index { get; set; }

        public LedgerIndexType LedgerIndexType { get; set; }
    }
}
