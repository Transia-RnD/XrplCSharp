using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Model;

namespace Xrpl.Client.Requests.Ledger
{
    public class LedgerEntryRequest : BaseLedgerRequest
    {
        public LedgerEntryRequest()
        {
            Command = "ledger_entry";
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryRequestType LedgerEntryRequestType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("account_root")]
        public string AccountRoot { get; set; }

        [JsonProperty("ripple_state")]
        public RippleState RippleState { get; set; }

        [JsonProperty("binary")]
        public bool? Binary { get; set; }
    }

    public class RippleState
    {

        [JsonProperty("accounts")]
        public string[] Addresses { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }        
    }
}
