using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;
using System.Runtime.Serialization;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerEntry.ts

namespace Xrpl.Models.Ledger
{
    public enum LedgerIndexType
    {
        [EnumMember(Value = "current")]
        Current,
        [EnumMember(Value = "closed")]
        Closed,
        [EnumMember(Value = "validated")]
        Validated
    }
    
    [JsonConverter(typeof(LOConverter))]
    public class LOLedgerEntry
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }
    }
}
