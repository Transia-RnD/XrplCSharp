using Xrpl.Models.Ledger;

namespace Xrpl.Models.Common
{
    public class LedgerIndex
    {
        public LedgerIndex(uint index)
        {
            Index = index;
        }

        public LedgerIndex(LedgerIndexType ledgerIndexType)
        {
            LedgerIndexType = ledgerIndexType;
        }

        public uint? Index { get; set; }

        public LedgerIndexType LedgerIndexType { get; set; }
    }
}
