using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Transactions;

namespace Xrpl.Client.Models.Ledger
{
    [JsonConverter(typeof(TransactionOrHashConverter))]
    public class HashOrTransaction
    {
        public string TransactionHash { get; set; }

        public TransactionResponseCommon Transaction { get; set; }
    }
}
