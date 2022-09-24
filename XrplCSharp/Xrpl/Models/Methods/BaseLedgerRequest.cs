using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Common;

namespace Xrpl.Models.Methods
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
