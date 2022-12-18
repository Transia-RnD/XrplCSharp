using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Ledger
{
    [JsonConverter(typeof(TransactionOrHashConverter))]
    public class HashOrTransaction
    {
        /// <summary>
        /// Unique hash of the transaction you are looking up
        /// </summary>
        public string TransactionHash { get; set; }
        /// <summary>
        /// server transaction response
        /// </summary>
        public TransactionResponseCommon Transaction { get; set; }
    }
}
