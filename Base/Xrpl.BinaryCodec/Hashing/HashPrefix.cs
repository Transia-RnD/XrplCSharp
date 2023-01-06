// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/hash-prefixes.ts

namespace Xrpl.BinaryCodec.Hashing
{
    /// <summary> hash prefix </summary>
    public enum HashPrefix : uint
    {
        /// <summary>
        /// TransactionId
        /// </summary>
        TransactionId = 0x54584E00u,
        /// <summary>
        /// Transaction
        /// </summary>
        Transaction = 0x534E4400u,
        /// <summary>
        /// AccountStateEntry
        /// </summary>
        AccountStateEntry = 0x4D4C4E00u,
        /// <summary>
        /// inner node in tree
        /// </summary>
        InnerNode = 0x4D494E00u,
        /// <summary>
        /// ledger master data for signing
        /// </summary>
        LedgerHeader = 0x4C575200u,
        /// <summary>
        /// TransactionSig
        /// </summary>
        TransactionSig = 0x53545800u,
        /// <summary>
        /// TransactionMultiSig
        /// </summary>
        TransactionMultiSig = 0x534D5400u,
        /// <summary>
        /// Validation
        /// </summary>
        Validation = 0x56414C00u,
        /// <summary>
        /// Proposal
        /// </summary>
        Proposal = 0x50525000u,
        /// <summary>
        /// PaymentChannelClaim
        /// </summary>
        PaymentChannelClaim = 0x434C4D00u,
    }
}