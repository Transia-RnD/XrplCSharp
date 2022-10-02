using System.Collections.Generic;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerHashes.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The LedgerHashes objects exist to make it possible to look up a previous ledger's hash
    /// with only the current ledger version and at most one lookup of a previous ledger version.
    /// </summary>
    public class LOLedgerHashes : BaseLedgerEntry
    {
        public LOLedgerHashes()
        {
            LedgerEntryType = LedgerEntryType.LedgerHashes;
        }
        
        public uint FirstLedgerSequence { get; set; } //todo unknown field
        /// <summary>
        /// The Ledger Index of the last entry in this object's Hashes array.
        /// </summary>
        public uint LastLedgerSequence { get; set; }
        /// <summary>
        /// An array of up to 256 ledger hashes. The contents depend on which sub-type of LedgerHashes object this is.
        /// </summary>
        public List<string> Hashes { get; set; }
        /// <summary>
        /// A bit-map of boolean flags for this object. No flags are defined for this type
        /// </summary>
        public uint Flags { get; set; }
    }
}
