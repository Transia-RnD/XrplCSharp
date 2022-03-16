using Newtonsoft.Json;
using RippleDotNet.Model.Transaction.TransactionTypes;

namespace RippleDotNet.Model.Transaction
{
    public class BinaryTransaction
    {
        [JsonProperty("meta")]
        public string Meta { get; set; }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }
    }
}
