using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
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
}
