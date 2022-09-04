using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Client.Models.Transactions
{
    public class PaymentChannelFund : TransactionCommon, IPaymentChannelFund
    {
        public PaymentChannelFund()
        {
            TransactionType = TransactionType.PaymentChannelFund;
        }

        public string Channel { get; set; }

        public string Amount { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }

    public interface IPaymentChannelFund : ITransactionCommon
    {
        string Amount { get; set; }
        string Channel { get; set; }
        DateTime? Expiration { get; set; }
    }

    public class PaymentChannelFundResponse : TransactionResponseCommon, IPaymentChannelFund
    {
        public string Amount { get; set; }

        public string Channel { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}
