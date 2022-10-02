using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

namespace Xrpl.Models.Methods
{
    public class BaseLedgerRequest : RippleRequest
    {
        public BaseLedgerRequest() { }

        public BaseLedgerRequest(Guid requestId) : base(requestId){ }
        /// <summary>
        /// 20-byte hex string for the ledger version to use. 
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger to use, or a shortcut string to choose a ledger automatically.
        /// </summary>
        [JsonProperty("ledger_index")]
        [JsonConverter(typeof(LedgerIndexConverter))]
        public LedgerIndex LedgerIndex { get; set; }
    }
}
