using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xrpl.Client.Models.Transactions;

namespace Xrpl.Client.Models.Methods
{
    public class SubmitRequest : RippleRequest
    {
        public SubmitRequest()
        {
            Command = "submit";
        }

        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        [JsonProperty("fail_hard")]
        public bool FailHard { get; set; }
    }
}
