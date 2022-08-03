using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class PaymentChannelFundTransaction : TransactionCommon, IPaymentChannelFundTransaction
    {
        public PaymentChannelFundTransaction()
        {
            TransactionType = TransactionType.PaymentChannelFund;
        }

        public string Channel { get; set; }

        public string Amount { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }

    public interface IPaymentChannelFundTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        string Channel { get; set; }
        DateTime? Expiration { get; set; }
    }

    public class PaymentChannelFundTransactionResponse : TransactionResponseCommon, IPaymentChannelFundTransaction
    {
        public string Amount { get; set; }

        public string Channel { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}
