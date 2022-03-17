using Newtonsoft.Json;

namespace Xrpl.Client.Requests.Ledger
{
    public class LedgerRequest : BaseLedgerRequest
    {
        public LedgerRequest()
        {
            Command = "ledger";
        }

        /// <summary>
        /// Admin is required for this property
        /// </summary>
        [JsonProperty("full")]
        public bool? Full { get; set; }

        /// <summary>
        /// Admin is required for this property
        /// </summary>
        [JsonProperty("accounts")]
        public bool? Accounts { get; set; }

        [JsonProperty("transactions")]
        public bool? Transactions { get; set; }

        [JsonProperty("expand")]
        public bool? Expand { get; set; }

        [JsonProperty("owner_funds")]
        public bool? OwnerFunds { get; set; }

        [JsonProperty("binary")]
        public bool? Binary { get; set; }

        [JsonProperty("queue")]
        public bool? Queue { get; set; }
    }
}
