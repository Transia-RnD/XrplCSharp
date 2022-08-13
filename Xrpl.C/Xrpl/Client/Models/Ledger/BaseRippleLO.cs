using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Ledger
{
    [JsonConverter(typeof(LOConverter))]
    public class BaseRippleLO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}