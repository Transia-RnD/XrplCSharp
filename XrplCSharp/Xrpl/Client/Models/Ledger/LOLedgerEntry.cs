using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using System.Runtime.Serialization;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/LedgerEntry.ts

namespace Xrpl.Client.Models.Ledger
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
