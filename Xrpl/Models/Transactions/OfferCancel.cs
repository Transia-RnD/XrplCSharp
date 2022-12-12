

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/offerCancel.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="IOfferCancel" />
    public class OfferCancel : TransactionCommon, IOfferCancel
    {
        public OfferCancel()
        {
            TransactionType = TransactionType.OfferCancel;
        }

        /// <inheritdoc />
        public uint OfferSequence { get; set; }
    }

    /// <summary>
    /// An OfferCancel transaction removes an Offer object from the XRP Ledger.
    /// </summary>
    public interface IOfferCancel : ITransactionCommon
    {
        /// <summary>
        /// The sequence number (or Ticket number) of a previous OfferCreate transaction.<br/>
        /// If specified, cancel any offer object in the ledger that was created by that transaction.<br/>
        /// It is not considered an error if the offer.<br/>
        /// specified does not exist.
        /// </summary>
        uint OfferSequence { get; set; }
    }

    /// <inheritdoc cref="IOfferCancel" />
    public class OfferCancelResponse : TransactionResponseCommon, IOfferCancel
    {
        /// <inheritdoc />
        public uint OfferSequence { get; set; }
    }
}
