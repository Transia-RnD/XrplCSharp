using Newtonsoft.Json;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Transactions
{
    public class BinaryTransaction
    {
        [JsonProperty("meta")]
        public string Meta { get; set; }

        [JsonProperty("tx_blob")]
        public string TransactionBlob { get; set; }
    }
}
