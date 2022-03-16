using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction;


namespace RippleDotNet.Model.Ledger
{
    public class Ledger : BaseLedgerInfo
    {
        [JsonProperty("ledger")]
        [JsonConverter(typeof(LedgerBinaryConverter))]
        public LedgerEntity LedgerEntity { get; set; }

        [JsonProperty("queue_data")]
        public List<QueuedTransaction> QueueData { get; set; }
    }

    public abstract class BaseLedgerEntity
    {
        [JsonProperty("closed")]
        public bool Closed { get; set; }
    }

    public class LedgerBinaryEntity : BaseLedgerEntity
    {
        [JsonProperty("ledger_data")]
        public string LedgerData { get; set; }

        [JsonProperty("transactions")]
        public List<BinaryTransaction> Transactions { get; set; }
    }

    public class LedgerEntity : BaseLedgerEntity
    {
        [JsonProperty("account_hash")]
        public string AccountHash { get; set; }

        [JsonProperty("accounts")]
        public dynamic[] Accounts { get; set; }

        [JsonProperty("close_time")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime CloseTime { get; set; }

        [JsonProperty("close_time_human")]
        public string CloseTimeHuman { get; set; }

        [JsonProperty("close_time_resolution")]
        public int CloseTimeResolution { get; set; }
        
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public string LedgerIndex { get; set; }

        [JsonProperty("parent_hash")]
        public string ParentHash { get; set; }

        [JsonProperty("total_coins")]
        public string TotalCoins { get; set; }

        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }

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
