using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
    public class AccountObjectsRequest : BaseLedgerRequest
    {
        public AccountObjectsRequest(string account)
        {
            Account = account;
            Command = "account_objects";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("limit")]
        public uint? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
