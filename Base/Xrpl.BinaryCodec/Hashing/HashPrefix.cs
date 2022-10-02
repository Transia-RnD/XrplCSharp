//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/hash-prefixes.ts

namespace Xrpl.BinaryCodecLib.Hashing
{
    /// <summary> hash prefix </summary>
    public enum HashPrefix : uint
    {
        /// <summary>
        /// transaction
        /// </summary>
        TransactionId = 0x54584E00u,
        /// <summary>
        /// transaction plus metadata
        /// </summary>
        TxNode = 0x534E4400u,
        /// <summary>
        /// account state
        /// </summary>
        LeafNode = 0x4D4C4E00u,
        /// <summary>
        /// inner node in tree
        /// </summary>
        InnerNode = 0x4D494E00u,
        /// <summary>
        /// ledger master data for signing
        /// </summary>
        LedgerMaster = 0x4C575200u,
        /// <summary>
        /// inner transaction to sign
        /// </summary>
        TxSign = 0x53545800u,
        /// <summary>
        /// Validation for signing
        /// </summary>
        Validation = 0x56414C00u,
        /// <summary>
        /// Proposal for signing
        /// </summary>
        Proposal = 0x50525000u
    }
}