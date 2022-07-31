using System.Collections.Generic;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class PaymentTransactionResponse : TransactionResponseCommon, IPaymentTransaction
    {
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }

        public new PaymentFlags? Flags { get; set; }

        public string InvoiceId { get; set; }

        public List<List<Path>> Paths { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }
}
