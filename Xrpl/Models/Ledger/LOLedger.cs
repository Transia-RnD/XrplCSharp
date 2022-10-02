using Newtonsoft.Json;

using System;
using System.Collections.Generic;

using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Ledger.ts


namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// Response expected from a <see cref="LedgerRequest"/>.
    /// https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/ledger.ts#L113
    /// </summary>
    public class LOLedger : LOBaseLedger //todo rename to LedgerResponse : BaseResponse
    {
        /// <summary>
        /// The complete header data of this {@link Ledger}.
        /// </summary>
        [JsonProperty("ledger")]
        [JsonConverter(typeof(LedgerBinaryConverter))]
        public object LedgerEntity { get; set; }
        /// <summary>
        /// Array of objects describing queued transactions, in the same order as  the queue.<br/>
        /// If the request specified expand as true, members contain full  representations of the transactions,
        /// in either JSON or binary depending  on whether the request specified binary as true.
        /// </summary>
        [JsonProperty("queue_data")]
        public List<QueuedTransaction> QueueData { get; set; }
        //todo not found field  validated?: boolean
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
    public class LedgerEntity : BaseLedgerEntity //todo rename to Ledger https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/ledger/Ledger.ts#L11
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
    /// <summary>
    /// Each member of the queue_data array represents one transaction in the queue.<br/>
    /// Some fields of this object may be omitted because they have not yet been calculated.<br/>
    /// <a>https://xrpl.org/ledger.html#response-format</a>
    /// </summary>
    public class QueuedTransaction 
        //todo Rename to LedgerQueueData https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/ledger.ts#L87
    {
        /// <summary>
        /// The Address of the sender for this queued transaction.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// By default, this is a String containing the identifying hash of the transaction.<br/>
        /// If transactions are expanded in binary format, this is an object whose only field is tx_blob,
        /// containing the binary form of the transaction as a decimal string.<br/>
        /// If transactions are expanded in JSON format, this is an object containing the
        /// transaction object including the transaction's identifying hash in the hash field.
        /// </summary>
        [JsonProperty("tx")]
        //TODO: This needs to be made into a JsonConverter for string or object
        public object Transaction { get; set; }
        /// <summary>
        /// How many times this transaction can be retried before being dropped.
        /// </summary>
        [JsonProperty("retries_remaining")]
        public uint RetriesRemaining { get; set; }
        /// <summary>
        /// The tentative result from preliminary transaction checking.<br/>
        /// This is always tesSUCCESS.
        /// </summary>
        [JsonProperty("preflight_result")]
        public string PreflightResult { get; set; }
        /// <summary>
        /// May be omitted) If this transaction was left in the queue after getting a retriable (ter) result, this is the exact ter result code it got.
        /// </summary>
        [JsonProperty("last_result")]
        public string LastResult { get; set; }
        /// <summary>
        /// (May be omitted) Whether this transaction changes this address's ways of authorizing transactions.
        /// </summary>
        [JsonProperty("auth_change")]
        public bool? AuthChange { get; set; }
        /// <summary>
        /// (May be omitted) The Transaction Cost of this transaction, in drops of XRP.
        /// </summary>
        [JsonProperty("fee")]
        public string Fee { get; set; }
        /// <summary>
        /// (May be omitted) The transaction cost of this transaction, relative to the minimum cost for this type of transaction, in fee levels.
        /// </summary>
        [JsonProperty("fee_level")]
        public string FeeLevel { get; set; }
        /// <summary>
        /// (May be omitted) The maximum amount of XRP, in drops, this transaction could potentially send or destroy.
        /// </summary>
        [JsonProperty("max_spend_drops")]
        public string MaxSpendDrops { get; set; }
    }
}
