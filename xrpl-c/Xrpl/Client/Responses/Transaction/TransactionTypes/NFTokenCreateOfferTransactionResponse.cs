using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class NFTokenCreateOfferTransactionResponse : TransactionResponseCommon, INFTokenCreateOfferTransaction
    {
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        public new NFTokenCreateOfferFlags? Flags { get; set; }

        public string TokenID { get; set; }

        public string Amount { get; set; }

        public string Owner { get; set; }

        public string Destination { get; set; }

    }
}
