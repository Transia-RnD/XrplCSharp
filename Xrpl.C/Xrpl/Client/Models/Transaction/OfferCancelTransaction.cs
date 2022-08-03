using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class OfferCancelTransaction : TransactionCommon, IOfferCancelTransaction
    {
        public OfferCancelTransaction()
        {
            TransactionType = TransactionType.OfferCancel;
        }

        public uint OfferSequence { get; set; }
    }

    public interface IOfferCancelTransaction : ITransactionCommon
    {
        uint OfferSequence { get; set; }
    }

    public class OfferCancelTransactionResponse : TransactionResponseCommon, IOfferCancelTransaction
    {
        public uint OfferSequence { get; set; }
    }
}
