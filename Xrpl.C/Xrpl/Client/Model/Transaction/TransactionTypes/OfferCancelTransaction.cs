using Xrpl.Client.Model.Transaction.Interfaces;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class OfferCancelTransaction : TransactionCommon, IOfferCancelTransaction
    {
        public OfferCancelTransaction()
        {
            TransactionType = TransactionType.OfferCancel;
        }

        public uint OfferSequence { get; set; }
    }
}
