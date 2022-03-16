using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Ledger;
using RippleDotNet.Model.Ledger.Objects;

namespace RippleDotNet.Model.Account
{
    public class AccountObjects
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("account_objects")]
        public List<BaseRippleLedgerObject> AccountObjectList { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }

        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
}
