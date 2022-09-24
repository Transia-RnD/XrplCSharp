using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Methods
{
    public enum RoleType
    {
        [EnumMember(Value = "gateway")]
        Gateway,
        [EnumMember(Value = "user")]
        User
    }
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
    public class NoRippleCheckRequest : BaseLedgerRequest
    {
        public NoRippleCheckRequest(string account)
        {
            Account = account;
            Command = "noripple_check";
            Role = RoleType.User;
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoleType Role { get; set; }

        [JsonProperty("transactions")]
        public bool? Transactions { get; set; }

        [JsonProperty("limit")]
        public uint? Limit { get; set; }
    }
}
