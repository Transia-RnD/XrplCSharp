using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class NFTokenAcceptOfferTransaction : TransactionCommon, INFTokenAcceptOfferTransaction
    {
        public NFTokenAcceptOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenAcceptOffer;
        }

        public string NFTokenID { get; set; }

        public string NFTokenSellOffer { get; set; }
        public string NFTokenBuyOffer { get; set; }
    }
}
