using System.Collections.Generic;
using Newtonsoft.Json;
using RippleDotNet.Model.Transaction;
using RippleDotNet.Model.Transaction.TransactionTypes;

namespace RippleDotNet.Model.Account
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
