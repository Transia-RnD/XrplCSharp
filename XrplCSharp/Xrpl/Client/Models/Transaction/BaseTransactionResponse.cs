using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Models.Transactions
{

    public interface IBaseTransactionResponse
    {
        DateTime? Date { get; set; }

        string Hash { get; set; }

        uint? InLedger { get; set; }

        uint? LedgerIndex { get; set; }

        bool? Validated { get; set; }
    }
    
    public class BaseTransactionResponse : IBaseTransactionResponse
    {
        [JsonConverter(typeof(RippleDateTimeConverter))]
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("inLedger")]
        public uint? InLedger { get; set; }

        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }

        [JsonProperty("validated")]
        public bool? Validated { get; set; }
    }
}
