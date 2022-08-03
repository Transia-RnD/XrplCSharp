using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum PaymentFlags : uint
    {
        tfNoDirectRipple = 65536,
        tfPartialPayment = 131072,
        tfLimitQuality = 262144,
    }
    public class PaymentTransaction : TransactionCommon, IPaymentTransaction
    {
        public PaymentTransaction()
        {
            TransactionType = TransactionType.Payment;
        }

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

    public interface IPaymentTransaction : ITransactionCommon
    {
        Currency Amount { get; set; }
        Currency DeliverMin { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        new PaymentFlags? Flags { get; set; }
        string InvoiceId { get; set; }
        List<List<Path>> Paths { get; set; }
        Currency SendMax { get; set; }
    }

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
