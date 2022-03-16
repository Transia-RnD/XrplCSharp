using System;
using Newtonsoft.Json;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class NFTokenAcceptOfferTransactionResponse : TransactionResponseCommon, INFTokenAcceptOfferTransaction
    {
        public string TokenID { get; set; }

    }
}
