

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    public class NFTokenCancelOffer : TransactionCommon, INFTokenCancelOffer
    {
        public NFTokenCancelOffer()
        {
            TransactionType = TransactionType.NFTokenCancelOffer;
        }

        public string[] NFTokenOffers { get; set; }
    }
    public interface INFTokenCancelOffer : ITransactionCommon
    {
        string[] NFTokenOffers { get; set; }
    }

    public class NFTokenCancelOfferResponse : TransactionResponseCommon, INFTokenCancelOffer
    {
        public string[] NFTokenOffers { get; set; }
    }
}
