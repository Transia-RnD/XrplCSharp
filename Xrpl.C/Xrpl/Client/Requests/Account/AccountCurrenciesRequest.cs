using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
    public class AccountCurrenciesRequest : BaseLedgerRequest
    {
        public AccountCurrenciesRequest(string account)
        {
            Command = "account_currencies";
            Account = account;
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("strict")]
        public bool? Strict { get; set; }
    }
}
