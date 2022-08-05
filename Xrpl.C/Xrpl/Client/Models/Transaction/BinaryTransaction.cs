using Newtonsoft.Json;

namespace Xrpl.Client.Models.Transactions
{
    public class BinaryTransaction
    {
        [JsonProperty("meta")]
        public string Meta { get; set; }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }
    }
}
