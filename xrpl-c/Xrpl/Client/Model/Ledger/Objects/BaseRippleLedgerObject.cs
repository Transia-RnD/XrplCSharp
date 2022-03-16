using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RippleDotNet.Json.Converters;

namespace RippleDotNet.Model.Ledger.Objects
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
