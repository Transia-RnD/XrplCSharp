using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Xrpl.Client.Model.Ledger
{
    public class BaseLedgerInfo
    {
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public uint LedgerIndex { get; set; }
    }
}
