using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class TrustSetTransactionResponse : TransactionResponseCommon, ITrustSetTransaction
    {
        public new TrustSetFlags? Flags { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LimitAmount { get; set; }
        public uint? QualityIn { get; set; }
        public uint? QualityOut { get; set; }
    }
}
