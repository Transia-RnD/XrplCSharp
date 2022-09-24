using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    public class NFTokenAcceptOffer : TransactionCommon, INFTokenAcceptOffer
    {
        public NFTokenAcceptOffer()
        {
            TransactionType = TransactionType.NFTokenAcceptOffer;
        }

        public string NFTokenID { get; set; }

        public string NFTokenSellOffer { get; set; }
        public string NFTokenBuyOffer { get; set; }
    }

    public interface INFTokenAcceptOffer : ITransactionCommon
    {
        string NFTokenID { get; set; }
    }

    public class NFTokenAcceptOfferResponse : TransactionResponseCommon, INFTokenAcceptOffer
    {
        public string NFTokenID { get; set; }

    }
}
