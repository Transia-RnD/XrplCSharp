using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RippleDotNet.Model;

namespace RippleDotNet.Requests.Account
{
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
