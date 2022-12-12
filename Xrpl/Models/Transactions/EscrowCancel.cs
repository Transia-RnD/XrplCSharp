
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/escrowCancel.ts

using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="IEscrowCancel" />
    public class EscrowCancel : TransactionCommon, IEscrowCancel
    {
        public EscrowCancel()
        {
            TransactionType = TransactionType.EscrowCancel;
        }

        /// <inheritdoc />
        public string Owner { get; set; }

        /// <inheritdoc />
        public uint OfferSequence { get; set; }
    }

    /// <summary>
    /// Return escrowed XRP to the sender.
    /// </summary>
    public interface IEscrowCancel : ITransactionCommon
    {
        /// <summary>
        /// Transaction sequence (or Ticket number) of EscrowCreate transaction that.<br/>
        /// created the escrow to cancel.
        /// </summary>
        uint OfferSequence { get; set; }
        /// <summary>
        /// Address of the source account that funded the escrow payment.
        /// </summary>
        string Owner { get; set; }
    }

    /// <inheritdoc cref="IEscrowCancel" />
    public class EscrowCancelResponse : TransactionResponseCommon, IEscrowCancel
    {
        /// <inheritdoc />
        public uint OfferSequence { get; set; }
        /// <inheritdoc />
        public string Owner { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a EscrowCancel at runtime.
        /// </summary>
        /// <param name="tx"> A EscrowCancel Transaction.</param>
        /// <exception cref="ValidationError">When the EscrowCancel is malformed.</exception>
        public static async Task ValidateEscrowCancel(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("Owner", out var Owner) || Owner is null)
                throw new ValidationError("EscrowCancel: missing Owner");
            if(Owner is not string {})
                throw new ValidationError("EscrowCancel: Owner must be a string");

            if (!tx.TryGetValue("OfferSequence", out var OfferSequence) || OfferSequence is null)
                throw new ValidationError("EscrowCancel: missing OfferSequence");
            if (OfferSequence is not uint { })
                throw new ValidationError("EscrowCancel: OfferSequence must be a number");

        }
    }

}
