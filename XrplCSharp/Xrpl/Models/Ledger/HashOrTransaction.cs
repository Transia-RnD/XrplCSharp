using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Ledger
{
    [JsonConverter(typeof(TransactionOrHashConverter))]
    public class HashOrTransaction
    {
        public string TransactionHash { get; set; }

        public TransactionResponseCommon Transaction { get; set; }
    }
}
