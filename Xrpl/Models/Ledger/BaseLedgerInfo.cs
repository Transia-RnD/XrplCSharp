using Newtonsoft.Json;

//https://github.com/XRPLF/xrpl.js/blob/76b73e16a97e1a371261b462ee1a24f1c01dbb0c/packages/xrpl/src/models/ledger/Ledger.ts

namespace Xrpl.Models.Ledger
{
    /// <summary> base ledger fields </summary>
    public class LOBaseLedger
    {
        /// <summary>
        /// The SHA-512Half of this ledger version.<br/>
        /// This serves as a unique identifier for this ledger and all its contents.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        /// <summary>
        /// The ledger index of the ledger.<br/>
        /// Some API methods display this as a quoted integer; some display it as a native JSON number.
        /// </summary>
        [JsonProperty("ledger_index")] 
        public uint LedgerIndex { get; set; }
    }
}
