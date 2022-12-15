// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/hash-prefixes.ts

namespace Xrpl.BinaryCodec
{
    public static class HashPrefix
    {
        public static readonly byte[] TransactionID = new byte[] { 0x54, 0x58, 0x4e, 0x00 };
        public static readonly byte[] Transaction = new byte[] { 0x53, 0x4e, 0x44, 0x00 };
        public static readonly byte[] AccountStateEntry = new byte[] { 0x4d, 0x4c, 0x4e, 0x00 };
        public static readonly byte[] InnerNode = new byte[] { 0x4d, 0x49, 0x4e, 0x00 };
        public static readonly byte[] LedgerHeader = new byte[] { 0x4c, 0x57, 0x52, 0x00 };
        public static readonly byte[] TransactionSig = new byte[] { 0x53, 0x54, 0x58, 0x00 };
        public static readonly byte[] TransactionMultiSig = new byte[] { 0x53, 0x4d, 0x54, 0x00 };
        public static readonly byte[] Validation = new byte[] { 0x56, 0x41, 0x4c, 0x00 };
        public static readonly byte[] Proposal = new byte[] { 0x50, 0x52, 0x50, 0x00 };
        public static readonly byte[] PaymentChannelClaim = new byte[] { 0x43, 0x4c, 0x4d, 0x00 };
    }
}