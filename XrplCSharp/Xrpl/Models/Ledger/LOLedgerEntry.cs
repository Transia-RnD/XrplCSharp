using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerEntry.ts

namespace Xrpl.Models.Ledger
{
    [JsonConverter(typeof(LOConverter))]
    public class LOLedgerEntry
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}