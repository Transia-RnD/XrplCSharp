using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class NFTokenCancelOfferTransaction : TransactionCommon, INFTokenCancelOfferTransaction
    {
        public NFTokenCancelOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenCancelOffer;
        }

        public string[] TokenOffers { get; set; }
    }
}
