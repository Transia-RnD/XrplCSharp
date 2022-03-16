using Newtonsoft.Json;

namespace RippleDotNet.Model.Account
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
}
