using Newtonsoft.Json;

namespace Xrpl.Client.Models.Methods
{
    public class LedgerDataRequest : BaseLedgerRequest
    {
        public LedgerDataRequest()
        {
            Command = "ledger_data";
        }

        [JsonProperty("binary")]
        public bool? Binary { get; set; }

        [JsonProperty("limit")]
        public uint? Limit { get; set; }

        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
}
