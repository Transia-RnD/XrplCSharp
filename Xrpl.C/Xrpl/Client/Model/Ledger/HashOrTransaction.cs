using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.TransactionTypes;
using Xrpl.Client.Responses.Transaction.TransactionTypes;

namespace Xrpl.Client.Model.Ledger
{
    [JsonConverter(typeof(TransactionOrHashConverter))]
    public class HashOrTransaction
    {
        public string TransactionHash { get; set; }

        public TransactionResponseCommon Transaction { get; set; }
    }
}
