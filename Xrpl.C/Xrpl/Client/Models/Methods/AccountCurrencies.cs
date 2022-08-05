using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xrpl.Client.Models.Methods
{
    public class AccountCurrencies
    {
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public int LedgerIndex { get; set; }

        [JsonProperty("receive_currencies")]
        public List<string> ReceiveCurrencies { get; set; }

        [JsonProperty("send_currencies")]
        public List<string> SendCurrencies { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }

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
