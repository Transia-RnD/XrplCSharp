using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RippleDotNet.Model.Account
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
}
