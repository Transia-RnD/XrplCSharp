using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Transactions
{
    public class EscrowCreateTransaction : TransactionCommon, IEscrowCreateTransaction
    {
        public EscrowCreateTransaction()
        {
            TransactionType = TransactionType.EscrowCreate;
        }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Destination { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        public string Condition { get; set; }

        public uint? DestinationTag { get; set; }

        public uint? SourceTag { get; set; }
    }

    public interface IEscrowCreateTransaction : ITransactionCommon
    {
        Currency Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Condition { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        DateTime? FinishAfter { get; set; }
        uint? SourceTag { get; set; }
    }

    public class EscrowCreateTransactionResponse : TransactionResponseCommon, IEscrowCreateTransaction
    {
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Destination { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        public string Condition { get; set; }

        public uint? DestinationTag { get; set; }

        public uint? SourceTag { get; set; }
    }
}
