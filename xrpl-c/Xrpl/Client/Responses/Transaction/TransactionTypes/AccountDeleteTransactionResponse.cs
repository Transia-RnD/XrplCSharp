using System.Collections.Generic;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class AccountDeleteTransactionResponse : TransactionResponseCommon, IAccountDeleteTransaction
    {
        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }

        //public new AccountDeleteFlags? Flags { get; set; }

        public string InvoiceId { get; set; }

        public List<List<Path>> Paths { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }
}
