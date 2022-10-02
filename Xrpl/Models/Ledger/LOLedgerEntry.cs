using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerEntry.ts
namespace Xrpl.Models.Ledger
{
    [JsonConverter(typeof(LOConverter))]
    public class LOLedgerEntry //todo rename LedgerEntryResponse: BaseResponse
    {
        /// <summary>
        /// Object containing the data of this ledger object, according to the  ledger format.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }
        /// <summary>
        /// The unique ID of this ledger object.
        /// </summary>
        [JsonProperty("index")]
        public string Index { get; set; }

        //todo not found fields  - ledger_current_index: number, node?: LedgerEntry,  node_binary?: string,  validated?: boolean
    }
}