using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Ledger;

namespace Xrpl.Client.Models.Methods
{
    public class AccountObjects
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("account_objects")]
        public List<BaseRippleLO> AccountObjectList { get; set; }

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

    public class AccountObjectsRequest : BaseLedgerRequest
    {
        public AccountObjectsRequest(string account)
        {
            Account = account;
            Command = "account_objects";
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
