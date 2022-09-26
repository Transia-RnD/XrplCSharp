using Newtonsoft.Json;

using System;
using System.Collections.Generic;

using Xrpl.ClientLib.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Ledger.ts


namespace Xrpl.Models.Ledger
{
    public class LOLedger : LOBaseLedger
    {
        [JsonProperty("ledger")]
        [JsonConverter(typeof(LedgerBinaryConverter))]
        public object LedgerEntity { get; set; }

        [JsonProperty("queue_data")]
        public List<QueuedTransaction> QueueData { get; set; }
    }

    public abstract class BaseLedgerEntity
    {
        /// <summary> Whether or not this ledger has been closed. </summary>
        [JsonProperty("closed")]
        public bool Closed { get; set; }
    }

    public class LedgerBinaryEntity : BaseLedgerEntity
    {
        [JsonProperty("ledger_data")]
        public string LedgerData { get; set; }

        [JsonProperty("transactions")]
        public List<string> Transactions { get; set; }
    }
    /// <summary>
    /// A ledger is a block of transactions and shared state data.<br/>
    /// It has a unique header that describes its contents using cryptographic hashes.
    /// </summary>
    public class LedgerEntity : BaseLedgerEntity
    {
        /// <summary>
        /// The SHA-512Half of this ledger's state tree information.
        /// </summary>
        [JsonProperty("account_hash")]
        public string AccountHash { get; set; }

        //todo not found field accountState?: LedgerEntry[] All the state information in this ledger.
        //todo not found field close_flags: number A bit-map of flags relating to the closing of this ledger.

        [JsonProperty("accounts")]
        public dynamic[] Accounts { get; set; }
        /// <summary>
        /// The approximate time this ledger version closed,
        /// as the number of seconds since the Ripple Epoch of 2000-01-01 00:00:00.<br/>
        /// This value is rounded based on the close_time_resolution.
        /// </summary>
        [JsonProperty("close_time")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// The approximate time this ledger was closed, in human-readable format.<br/>
        /// Always uses the UTC time zone.
        /// </summary>
        [JsonProperty("close_time_human")]
        public string CloseTimeHuman { get; set; }
        /// <summary>
        /// An integer in the range [2,120] indicating the maximum number of seconds by which the close_time could be rounded.
        /// </summary>
        [JsonProperty("close_time_resolution")]
        public int CloseTimeResolution { get; set; }
        /// <summary>
        /// The SHA-512Half of this ledger version.<br/>
        /// This serves as a unique identifier for this ledger and all its contents.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger.<br/>
        /// Some API methods display this as a quoted integer; some display it as a native JSON number.
        /// </summary>
        [JsonProperty("ledger_index")]
        public string LedgerIndex { get; set; }

        //todo not found field parent_close_time: number The approximate time at which the previous ledger was closed.
        /// <summary>
        /// Unique identifying hash of the ledger that came immediately before this one
        /// </summary>
        [JsonProperty("parent_hash")]
        public string ParentHash { get; set; }
        /// <summary>
        /// Total number of XRP drops in the network, as a quoted integer.
        /// </summary>
        [JsonProperty("total_coins")]
        public string TotalCoins { get; set; }
        /// <summary>
        /// Hash of the transaction information included in this ledger, as hex.
        /// </summary>
        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }
        /// <summary>
        /// Transactions applied in this ledger version.<br/>
        /// By default, members are the transactions' identifying Hash strings.<br/>
        /// If the request specified expand as true,
        /// members are full representations of the transactions instead,
        /// in either JSON or binary depending on whether the request specified binary as true.
        /// </summary>
        [JsonProperty("transactions")]
        public List<HashOrTransaction> Transactions { get; set; }
    }

    public class QueuedTransaction
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("tx")]
        //TODO: This needs to be made into a JsonConverter for string or object
        public object Transaction { get; set; }

        [JsonProperty("retries_remaining")]
        public uint RetriesRemaining { get; set; }

        [JsonProperty("preflight_result")]
        public string PreflightResult { get; set; }

        [JsonProperty("last_result")]
        public string LastResult { get; set; }

        [JsonProperty("auth_change")]
        public bool? AuthChange { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("fee_level")]
        public string FeeLevel { get; set; }

        [JsonProperty("max_spend_drops")]
        public string MaxSpendDrops { get; set; }
    }
}
