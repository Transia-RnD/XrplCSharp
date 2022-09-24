using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Methods
{
    public class AccountTransactions
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("ledger_index_min")]
        public uint LedgerIndexMin { get; set; }

        [JsonProperty("ledger_index_max")]
        public uint LedgerIndexMax { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("transactions")]
        public List<TransactionSummary> Transactions { get; set; }

     
    }

    public class TransactionSummary
    {
        [JsonProperty("ledger_index")]
        public uint LedgerIndex { get; set; }

        [JsonProperty("meta")]
        [JsonConverter(typeof(MetaBinaryConverter))]
        public Meta Meta { get; set; }

        [JsonProperty("tx")]
        public TransactionResponseCommon Transaction { get; set; }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }

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
