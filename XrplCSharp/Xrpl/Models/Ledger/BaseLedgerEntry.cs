using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;

namespace Xrpl.Models.Ledger
{
    [JsonConverter(typeof(LOConverter))]
    public class BaseLedgerEntry
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}