using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Model.Transaction;
using Xrpl.Client.Model.Transaction.TransactionTypes;

namespace Xrpl.Client.Model.Account
{
    public class NoRippleCheck
    {
        [JsonProperty("ledger_current_index")]
        public uint LedgerCurrentIndex { get; set; }

        [JsonProperty("problems")]
        public List<string> Problems { get; set; }

        [JsonProperty("transactions")]
        public List<TransactionCommon> Transactions { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
}
