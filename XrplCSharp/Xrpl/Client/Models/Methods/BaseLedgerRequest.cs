using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Methods
{
    public class BaseLedgerRequest : RippleRequest
    {
        public BaseLedgerRequest() { }

        public BaseLedgerRequest(Guid requestId) : base(requestId){ }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        [JsonConverter(typeof(LedgerIndexConverter))]
        public LedgerIndex LedgerIndex { get; set; }
    }
}
