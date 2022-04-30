using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class NFTokenCancelOfferTransactionResponse : TransactionResponseCommon, INFTokenCancelOfferTransaction
    {
        public string[] NFTokenOffers { get; set; }
    }
}
