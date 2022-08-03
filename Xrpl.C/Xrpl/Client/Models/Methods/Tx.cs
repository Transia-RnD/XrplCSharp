using Newtonsoft.Json;

namespace Xrpl.Client.Models.Methods
{
    internal class TransactionRequest : RippleRequest
    {
        public TransactionRequest(string transaction)
        {
            Command = "tx";
            Transaction = transaction;
        }

        [JsonProperty("transaction")]
        internal string Transaction { get; set; }

        [JsonProperty("binary")]
        internal bool? Binary { get; set; }
    }
}
