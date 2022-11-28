

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/offerCancel.ts

using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transaction
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

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a OfferCancel at runtime.
        /// </summary>
        /// <param name="tx"> A OfferCancel Transaction.</param>
        /// <exception cref="ValidationError">When the OfferCancel is malformed.</exception>
        public static async Task ValidateOfferCancel(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("OfferSequence", out var OfferSequence) || OfferSequence is null)
                throw new ValidationError("OfferCancel: missing field OfferSequence");

            if (OfferSequence is not uint { })
                throw new ValidationError("OfferCancel: OfferSequence must be a number");
        }
    }

}
