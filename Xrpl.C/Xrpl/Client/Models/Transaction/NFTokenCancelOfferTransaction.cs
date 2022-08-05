using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class NFTokenCancelOfferTransaction : TransactionCommon, INFTokenCancelOfferTransaction
    {
        public NFTokenCancelOfferTransaction()
        {
            TransactionType = TransactionType.NFTokenCancelOffer;
        }

        public string[] NFTokenOffers { get; set; }
    }
    public interface INFTokenCancelOfferTransaction : ITransactionCommon
    {
        string[] NFTokenOffers { get; set; }
    }

    public class NFTokenCancelOfferTransactionResponse : TransactionResponseCommon, INFTokenCancelOfferTransaction
    {
        public string[] NFTokenOffers { get; set; }
    }
}
