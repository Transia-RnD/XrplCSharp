namespace Ripple.Core.Hashing
{
    public enum HashPrefix : uint
    {
        TransactionId = 0x54584E00u,
        // transaction plus metadata
        TxNode = 0x534E4400u,
        // account state
        LeafNode = 0x4D4C4E00u,
        // inner node in tree
        InnerNode = 0x4D494E00u,
        // ledger master data for signing
        LedgerMaster = 0x4C575200u,
        // inner transaction to sign
        TxSign = 0x53545800u,
        // Validation for signing
        Validation = 0x56414C00u,
        // Proposal for signing
        Proposal = 0x50525000u
    }
}