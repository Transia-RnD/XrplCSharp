using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Client.Models.Transactions
{
    [Flags]
    public enum PaymentFlags : uint
    {
        tfNoDirectRipple = 65536,
        tfPartialPayment = 131072,
        tfLimitQuality = 262144,
    }
    public class Payment : TransactionCommon, IPayment
    {
        public Payment()
        {
            TransactionType = TransactionType.Payment;
        }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }

        public new PaymentFlags? Flags { get; set; }

        public string InvoiceID { get; set; }

        public List<List<Path>> Paths { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }

    public interface IPayment : ITransactionCommon
    {
        Currency Amount { get; set; }
        Currency DeliverMin { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        new PaymentFlags? Flags { get; set; }
        string InvoiceID { get; set; }
        List<List<Path>> Paths { get; set; }
        Currency SendMax { get; set; }
    }

    public class PaymentResponse : TransactionResponseCommon, IPayment
    {
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }

        public new PaymentFlags? Flags { get; set; }

        public string InvoiceID { get; set; }

        public List<List<Path>> Paths { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }
}
