using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Common;

namespace Xrpl.Client.Models.Transactions
{

    [Flags]
    public enum OfferCreateFlags : uint
    {
        tfPassive = 65536,
        tfImmediateOrCancel = 131072,
        tfFillOrKill = 262144,
        tfSell = 524288
    }
    public class OfferCreateTransaction : TransactionCommon, IOfferCreateTransaction
    {
        public OfferCreateTransaction()
        {
            TransactionType = TransactionType.OfferCreate;
        }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new OfferCreateFlags? Flags { get; set; }

        /// <summary>
        /// An offer to delete first, specified in the same way as OfferCancel.
        /// </summary>
        public uint? OfferSequence { get; set; }

        /// <summary>
        /// The amount and type of currency being provided by the offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }

        /// <summary>
        /// The amount and type of currency being requested by the offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
    }

    public interface IOfferCreateTransaction : ITransactionCommon
    {
        DateTime? Expiration { get; set; }
        new OfferCreateFlags? Flags { get; set; }
        uint? OfferSequence { get; set; }
        Currency TakerGets { get; set; }
        Currency TakerPays { get; set; }

    }

    public class OfferCreateTransactionResponse : TransactionResponseCommon, IOfferCreateTransaction
    {
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new OfferCreateFlags? Flags { get; set; }

        /// <summary>
        /// An offer to delete first, specified in the same way as OfferCancel.
        /// </summary>
        public uint? OfferSequence { get; set; }

        /// <summary>
        /// The amount and type of currency being provided by the offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }

        /// <summary>
        /// The amount and type of currency being requested by the offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
    }
}
