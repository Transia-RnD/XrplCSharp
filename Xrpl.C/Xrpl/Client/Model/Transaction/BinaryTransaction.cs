using Newtonsoft.Json;
using Xrpl.Client.Model.Transaction.TransactionTypes;

namespace Xrpl.Client.Model.Transaction
{
    public class BinaryTransaction
    {
        [JsonProperty("meta")]
        public string Meta { get; set; }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }
    }
}
