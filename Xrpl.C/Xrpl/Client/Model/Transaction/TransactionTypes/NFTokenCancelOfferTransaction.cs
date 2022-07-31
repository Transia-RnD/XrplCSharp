using System;

using Newtonsoft.Json;

using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class NFTokenCancelOfferTransaction : TransactionCommon, INFTokenCancelOfferTransaction
    {
        public NFTokenCancelOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenCancelOffer;
        }

        public string[] NFTokenOffers { get; set; }
    }
}
