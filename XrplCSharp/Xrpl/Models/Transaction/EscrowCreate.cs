using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    public class EscrowCreate : TransactionCommon, IEscrowCreate
    {
        public EscrowCreate()
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

    public interface IEscrowCreate : ITransactionCommon
    {
        Currency Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Condition { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        DateTime? FinishAfter { get; set; }
        uint? SourceTag { get; set; }
    }

    public class EscrowCreateResponse : TransactionResponseCommon, IEscrowCreate
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
