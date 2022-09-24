using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Xrpl.Models.Methods
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
