using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Account
{
    public class AccountTransactionsRequest : BaseLedgerRequest
    {
        public AccountTransactionsRequest(string account)
        {
            Account = account;
            Command = "account_tx";
            LedgerIndexMin = -1;
            LedgerIndexMax = -1;
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("ledger_index_min")]
        public int LedgerIndexMin { get; set; }

        [JsonProperty("ledger_index_max")]
        public int LedgerIndexMax { get; set; }

        [JsonProperty("binary")]
        public bool? Binary { get; set; }

        [JsonProperty("forward")]
        public bool? Forward { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
