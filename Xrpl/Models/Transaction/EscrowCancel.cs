
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/escrowCancel.ts

namespace Xrpl.Models.Transaction
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
}
