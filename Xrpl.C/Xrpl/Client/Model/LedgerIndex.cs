using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model
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
