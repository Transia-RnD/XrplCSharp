using Newtonsoft.Json;

using System.Collections.Generic;
using System.Globalization;

using Xrpl.Client.Extensions;

namespace Xrpl.Client.Model.Account
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
        public decimal BalanceAsNumber => string.IsNullOrWhiteSpace(Balance) ? 0 : decimal.Parse(Balance, NumberStyles.Float, CultureInfo.InvariantCulture);

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonIgnore]
        public string CurrencyHexName => Currency is { Length: > 0 } row ? row.Length > 3 ? row.FromHexString().Trim('\0') : row : string.Empty;

        [JsonProperty("limit")]
        public string Limit { get; set; }

        [JsonIgnore]
        public decimal LimitAsNumber => string.IsNullOrWhiteSpace(Limit) ? 0 : decimal.Parse(Limit, NumberStyles.Float, CultureInfo.InvariantCulture);

        [JsonProperty("limit_peer")]
        public string LimitPeer { get; set; }

        [JsonIgnore]
        public decimal LimitPeerAsNumber => string.IsNullOrWhiteSpace(LimitPeer) ? 0 : decimal.Parse(LimitPeer, NumberStyles.Float, CultureInfo.InvariantCulture);

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
}
