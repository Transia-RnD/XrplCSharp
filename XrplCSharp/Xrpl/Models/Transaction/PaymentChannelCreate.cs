using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelCreate.ts

namespace Xrpl.Models.Transactions
{
    public class PaymentChannelCreate : TransactionCommon, IPaymentChannelCreate
    {
        public PaymentChannelCreate()
        {
            TransactionType = TransactionType.PaymentChannelCreate;
        }

        public string Amount { get; set; }

        public string Destination { get; set; }

        public uint SettleDelay { get; set; }

        public string PublicKey { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        public uint? DestinationTag { get; set; }

        public uint? SourceTag { get; set; }
    }

    public interface IPaymentChannelCreate : ITransactionCommon
    {
        string Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        string PublicKey { get; set; }
        uint SettleDelay { get; set; }
        uint? SourceTag { get; set; }
    }

    public class PaymentChannelCreateResponse : TransactionResponseCommon, IPaymentChannelCreate
    {
        public string Amount { get; set; }

        public string Destination { get; set; }

        public uint SettleDelay { get; set; }

        public string PublicKey { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        public uint? DestinationTag { get; set; }

        public uint? SourceTag { get; set; }
    }
}
