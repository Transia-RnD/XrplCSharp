using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Transactions
{

    public interface IAccountDeleteTransaction : ITransactionCommon
    {
        Currency DeliverMin { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        //new AccountDeleteFlags? Flags { get; set; }
        string InvoiceId { get; set; }
        List<List<Path>> Paths { get; set; }
        Currency SendMax { get; set; }
    }
    
    public class AccountDeleteTransaction : TransactionCommon, IAccountDeleteTransaction
    {
        public AccountDeleteTransaction()
        {
            TransactionType = TransactionType.AccountDelete;
        }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

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
