using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;

namespace Xrpl.Models.Methods
{
    public class GatewayBalances
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("assets")]
        public dynamic Assets { get; set; }

        [JsonProperty("balances")]
        public dynamic Balances { get; set; }

        [JsonProperty("obligations")]
        public dynamic Obligations { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
                
    }

    public class GatewayBalancesRequest : BaseLedgerRequest
    {
        public GatewayBalancesRequest(string account)
        {
            Account = account;
            Command = "gateway_balances";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("strict")]
        public bool? Strict { get; set; }

        [JsonProperty("hotwallet")]
        [JsonConverter(typeof(StringOrArrayConverter))]
        public object HotWallet { get; set; }
    }
}
