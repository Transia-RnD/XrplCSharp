using Newtonsoft.Json;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerData.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The `ledger_data` method retrieves contents of the specified ledger.<br/>
    /// You can  iterate through several calls to retrieve the entire contents of a single  ledger version.
    /// </summary>
    /// <code>
    /// ``` ts  const ledgerData: LedgerDataRequest = {
    ///     "id": 2,
    ///     "ledger_hash": "842B57C1CC0613299A686D3E9F310EC0422C84D3911E5056389AA7E5808A93C8",
    ///     "command": "ledger_data",
    ///     "limit": 5,
    ///     "binary": true
    /// } ```
    /// </code>
    public class LedgerDataRequest : BaseLedgerRequest
    {
        public LedgerDataRequest()
        {
            Command = "ledger_data";
        }
        /// <summary>
        /// If set to true, return ledger objects as hashed hex strings instead of JSON.
        /// </summary>
        [JsonProperty("binary")]
        public bool? Binary { get; set; }
        /// <summary>
        /// Limit the number of ledger objects to retrieve.<br/>
        /// The server is not required to honor this value.
        /// </summary>
        [JsonProperty("limit")]
        public uint? Limit { get; set; }
        /// <summary>
        /// Value from a previous paginated response.<br/>
        /// Resume retrieving data where that response left off.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
