using Newtonsoft.Json;

namespace Xrpl.Client.Models.Transactions
{
    public class BinaryTransactionResponse : BaseTransactionResponse
    {
        [JsonProperty("meta")]
        public string Meta { get; set; }

        [JsonProperty("tx")]
        public string Transaction { get; set; }
    }
}
