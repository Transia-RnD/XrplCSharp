using Newtonsoft.Json;

using System.Collections.Generic;
using Xrpl.Models.Methods;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerData.ts
namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The response expected from a <see cref="LedgerDataRequest"/>.
    /// </summary>
    public class LOLedgerData //todo rename to LedgerDataResponse :BaseResponse
    {
        /// <summary>
        /// The ledger index of this ledger version.
        /// </summary>
        [JsonProperty("ledger_index")]
        public uint LedgerIndex { get; set; }
        /// <summary>
        /// Unique identifying hash of this ledger version.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// Array of JSON objects containing data from the ledger's state tree,  as defined below.
        /// </summary>
        [JsonProperty("state")]
        public List<LedgerDataBinaryOrJson> State { get; set; }
        /// <summary>
        /// Server-defined value indicating the response is paginated.<br/>
        /// Pass this to  the next call to resume where this call left off.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
        //todo non found field validated?: boolean
    }

    public class LedgerDataBinaryOrJson
    {
        [JsonProperty("marker")]
        public string Data { get; set; }

        public BaseLedgerEntry LedgerObject { get; set; }
    }
}
