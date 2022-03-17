using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction;
using Xrpl.Client.Model.Transaction.TransactionTypes;
using Xrpl.Client.Responses.Transaction.TransactionTypes;

namespace Xrpl.Client.Model.Account
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
}
