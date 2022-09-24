using Newtonsoft.Json;

namespace Xrpl.Models.Methods
{
    public class TxRequest : RippleRequest
    {
        public TxRequest(string hash)
        {
            Command = "tx";
            Transaction = hash;
        }

        [JsonProperty("transaction")]
        internal string Transaction { get; set; }

        [JsonProperty("binary")]
        internal bool? Binary { get; set; }
    }
}
