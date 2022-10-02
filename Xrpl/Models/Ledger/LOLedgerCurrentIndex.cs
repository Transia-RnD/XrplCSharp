
using Newtonsoft.Json;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerCurrent.ts
namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// Response expected from a {@link LedgerCurrentRequest}.
    /// </summary>
    public class LOLedgerCurrentIndex //todo rename to LedgerCurrentResponse
    {
        /// <summary>
        /// The ledger index of this ledger version.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint CurrentIndex { get; set; }
    }
}
