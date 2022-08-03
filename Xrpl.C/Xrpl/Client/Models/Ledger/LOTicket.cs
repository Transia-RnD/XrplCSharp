using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Ticket.ts

namespace Xrpl.Client.Models.Ledger
{
    // [JsonConverter(typeof(LOConverter))]
    // public class LODepositPreauth
    // {
    //     [JsonConverter(typeof(StringEnumConverter))]
    //     public LedgerEntryType LedgerEntryType { get; set; }

    //     [JsonProperty("index")]
    //     public string Index { get; set; }
    // }
}
