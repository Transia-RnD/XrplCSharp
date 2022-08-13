using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xrpl.Client.Models.Transactions;

namespace Xrpl.Client.Models.Methods
{
    /// <summary>
    /// Per Ripple, this mode is intended to be used for testing.
    /// </summary>
    public class SubmitRequest : RippleRequest
    {
        public SubmitRequest()
        {
            Command = "submit";
            FeeMultMax = 1000;
        }

        [JsonProperty("tx_json")]
        public ITransactionCommon Transaction { get; set; }

        /// <summary>
        /// Do not send your secret to a server that you do not control or do not trust.
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("offline")]
        public bool? Offline { get; set; }

        [JsonProperty("build_path")]
        public bool? BuildPath { get; set; }

        [JsonProperty("fee_mult_max")]
        public uint? FeeMultMax { get; set; }

        [JsonProperty("fee_div_max")]
        public uint? FeeDivMax { get; set; }
    }

    public class SubmitBlobRequest : RippleRequest
    {
        public SubmitBlobRequest()
        {
            Command = "submit";
        }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }

        [JsonProperty("fail_hard")]
        public bool FailHard { get; set; }
    }
}
