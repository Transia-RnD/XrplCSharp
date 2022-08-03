using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class PaymentChannelCreateTransaction : TransactionCommon, IPaymentChannelCreateTransaction
    {
        public PaymentChannelCreateTransaction()
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

    public interface IPaymentChannelCreateTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        string PublicKey { get; set; }
        uint SettleDelay { get; set; }
        uint? SourceTag { get; set; }
    }

    public class PaymentChannelCreateTransactionResponse : TransactionResponseCommon, IPaymentChannelCreateTransaction
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
