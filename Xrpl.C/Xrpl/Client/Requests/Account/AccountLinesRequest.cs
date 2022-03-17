using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
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
        public int Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
