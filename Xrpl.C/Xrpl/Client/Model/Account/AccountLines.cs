using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public decimal BalanceAsNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Balance))
                    return 0;
                var can_parse = decimal.TryParse(Balance, NumberStyles.Float, CultureInfo.InvariantCulture, out var result);
                return !can_parse ? 0 : result;
            }
        }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("limit")]
        public string Limit { get; set; }

        [JsonIgnore]
        public decimal LimitAsNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Limit))
                    return 0;
                var can_parse = decimal.TryParse(Limit, NumberStyles.Float, CultureInfo.InvariantCulture, out var result);
                return !can_parse ? 0 : result;
            }
        }

        [JsonProperty("limit_peer")]
        public string LimitPeer { get; set; }

        [JsonIgnore]
        public decimal LimitPeerAsNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LimitPeer))
                    return 0;
                var can_parse = decimal.TryParse(LimitPeer, NumberStyles.Float, CultureInfo.InvariantCulture, out var result);
                return !can_parse ? 0 : result;
            }
        }

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
