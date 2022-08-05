using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
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

    public interface INFTokenAcceptOfferTransaction : ITransactionCommon
    {
        string NFTokenID { get; set; }
    }

    public class NFTokenAcceptOfferTransactionResponse : TransactionResponseCommon, INFTokenAcceptOfferTransaction
    {
        public string NFTokenID { get; set; }

    }
}
