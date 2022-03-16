using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class NFTokenAcceptOfferTransaction : TransactionCommon, INFTokenAcceptOfferTransaction
    {
        public NFTokenAcceptOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenAcceptOffer;
        }

        //public string Account { get; set; } // INHEIRTED FROM COMMON

        public string TokenID { get; set; }
    }
}
