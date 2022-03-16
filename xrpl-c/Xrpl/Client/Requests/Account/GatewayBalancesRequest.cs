using Newtonsoft.Json;
using RippleDotNet.Json.Converters;

namespace RippleDotNet.Requests.Account
{
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
