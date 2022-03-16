using System.Collections.Generic;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction;
using RippleDotNet.Model.Transaction.TransactionTypes;
using RippleDotNet.Responses.Transaction.TransactionTypes;

namespace RippleDotNet.Model.Account
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
