using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Extensions;

namespace Xrpl.Client.Models.Methods
{
    public class AccountLines
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("lines")]
        public List<TrustLine> TrustLines { get; set; }

        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }

        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }

    public class TrustLine
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonIgnore]
        public decimal BalanceAsNumber => decimal.Parse(Balance);

        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonIgnore]
        public string CurrencyValidName => Currency is { Length: > 0 } row ? row.Length > 3 ? row.FromHexString().Trim('\0') : row : string.Empty;

        [JsonProperty("limit")]
        public string Limit { get; set; }

        [JsonIgnore]
        public decimal LimitAsNumber => decimal.Parse(Limit);

        [JsonProperty("limit_peer")]
        public string LimitPeer { get; set; }

        [JsonIgnore]
        public decimal LimitPeerAsNumber => decimal.Parse(LimitPeer);

        [JsonProperty("quality_in")]
        public uint QualityIn { get; set; }

        [JsonProperty("quality_out")]
        public uint QualityOut { get; set; }

        [JsonProperty("no_ripple")]
        public bool? NoRipple { get; set; }

        [JsonProperty("no_ripple_peer")]
        public bool? NoRipplePeer { get; set; }

        [JsonProperty("freeze")]
        public bool? Freeze { get; set; }

        [JsonProperty("freeze_peer")]
        public bool? FreezePeer { get; set; }
    }

    public class AccountLinesRequest : BaseLedgerRequest
    {
        public AccountLinesRequest(string account)
        {
            Account = account;
            Command = "account_lines";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("peer")]
        public string Peer { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
