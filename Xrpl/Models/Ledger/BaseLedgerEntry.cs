using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;

//https://github.com/XRPLF/xrpl.js/blob/76b73e16a97e1a371261b462ee1a24f1c01dbb0c/packages/xrpl/src/models/ledger/BaseLedgerEntry.ts

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