using Newtonsoft.Json;
using Xrpl.Models.Transaction;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/submit.ts
namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The submit method applies a transaction and sends it to the network to be  confirmed and included in future ledgers.<br/>
    /// Expects a response in the form of a  <see cref="Submit"/> .
    /// </summary>
    public class SubmitRequest : RippleRequest
    {
        public SubmitRequest()
        {
            Command = "submit";
        }

        /// <summary>
        /// The complete transaction in hex string format.
        /// </summary>
        [JsonProperty("tx_blob")]
        public string TxBlob { get; set; }

        /// <summary>
        /// If true, and the transaction fails locally, do not retry or relay the transaction to other servers.<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("fail_hard")]
        public bool? FailHard { get; set; }
    }
}
