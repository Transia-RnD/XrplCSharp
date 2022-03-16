using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class NFTokenCancelOfferTransactionResponse : TransactionResponseCommon, INFTokenCancelOfferTransaction
    {
        public string[] TokenOffers { get; set; }
    }
}
