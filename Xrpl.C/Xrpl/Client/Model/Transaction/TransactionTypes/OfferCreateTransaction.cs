using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
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
}
