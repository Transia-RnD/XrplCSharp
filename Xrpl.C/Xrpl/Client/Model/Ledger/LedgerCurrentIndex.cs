
using Newtonsoft.Json;

namespace Xrpl.Client.Model.Ledger
{
    public class LedgerCurrentIndex
    {
        [JsonProperty("ledger_current_index")]
        public uint CurrentIndex { get; set; }
    }
}
