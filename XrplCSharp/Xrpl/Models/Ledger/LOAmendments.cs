using Newtonsoft.Json;

using System;
using System.Collections.Generic;

using Xrpl.ClientLib.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Amendments.ts

namespace Xrpl.Models.Ledger
{
    public enum EnableAmendmentFlags
    {
        /// <summary>
        /// The tfGotMajority flag means that support for the amendment has increased to more than 80% of trusted validators.
        /// </summary>
        tfGotMajority = 65536,
        /// <summary>
        /// The tfLostMajority flag means that support for the amendment has decreased to 80% of trusted validators or less.
        /// </summary>
        tfLostMajority = 131072
    }

    /// <summary>
    /// The Amendments object type contains a list of Amendments that are currently active.
    /// </summary>
    public class LOAmendments : BaseLedgerEntry
    {
        public LOAmendments()
        {
            LedgerEntryType = LedgerEntryType.Amendments;
        }
        /// <summary>
        /// Array of objects describing the status of amendments that have majority support but are not yet enabled.<br/>
        /// If omitted, there are no pending amendments with majority support.
        /// </summary>
        public List<Majority> Majorities { get; set; }
        /// <summary>
        /// Array of 256-bit amendment IDs for all currently-enabled amendments.<br/>
        /// If omitted, there are no enabled amendments.
        /// </summary>
        public List<string> Amendments { get; set; }
        /// <summary>
        /// A bit-map of boolean flags.<br/>
        /// No flags are defined for the Amendments object type, so this value is always 0.
        /// </summary>
        public uint Flags { get; set; }
    }

    public class Majority
    {
        /// <summary>
        /// The Amendment ID of the pending amendment.
        /// </summary>
        public string Amendment { get; set; }
        /// <summary>
        /// The `close_time` field of the ledger version where this amendment most recently gained a majority.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CloseTime { get; set; }
    }
}
