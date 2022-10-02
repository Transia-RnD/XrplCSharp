

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transaction
{
    public class OfferCancel : TransactionCommon, IOfferCancel
    {
        public OfferCancel()
        {
            TransactionType = TransactionType.OfferCancel;
        }

        public uint OfferSequence { get; set; }
    }

    public interface IOfferCancel : ITransactionCommon
    {
        uint OfferSequence { get; set; }
    }

    public class OfferCancelResponse : TransactionResponseCommon, IOfferCancel
    {
        public uint OfferSequence { get; set; }
    }
}
