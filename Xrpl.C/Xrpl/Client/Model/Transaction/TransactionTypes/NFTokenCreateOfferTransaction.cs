using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
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

        public string NFTokenID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        public string Owner { get; set; }

        public string Destination { get; set; }
    }
}
