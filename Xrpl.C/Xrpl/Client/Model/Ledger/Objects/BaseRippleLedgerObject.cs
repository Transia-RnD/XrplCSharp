using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Xrpl.Client.Json.Converters;

using xrpl_c.Xrpl.Client.Model.Enums;

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
