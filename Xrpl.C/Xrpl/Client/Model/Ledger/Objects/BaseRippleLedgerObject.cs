using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;

namespace Xrpl.Client.Model.Ledger.Objects
{
    [JsonConverter(typeof(LedgerObjectConverter))]
    public class BaseRippleLedgerObject
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}
