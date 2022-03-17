using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Requests.Account
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
