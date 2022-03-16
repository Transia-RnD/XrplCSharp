using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class NFTokenCreateOfferTransaction : TransactionCommon, INFTokenCreateOfferTransaction
    {
        public NFTokenCreateOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenCreateOffer;
        }

        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new NFTokenCreateOfferFlags? Flags { get; set; }

        public string TokenID { get; set; }

        public string Amount { get; set; }

        public string Owner { get; set; }

        public string Destination { get; set; }
    }
}
