using Xrpl.Client.Model.Transaction.Interfaces;
using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class TicketCreateCreateTransactionResponse : TransactionResponseCommon, ITicketCreateTransaction
    {
        /// <inheritdoc/>
        public new TrustSetFlags? Flags { get; set; }
        /// <inheritdoc/>
        public LedgerEntryType LedgerEntryType { get; set; }

        /// <inheritdoc/>
        public string OwnerNode { get; set; }

        /// <inheritdoc/>
        public string PreviousTransactionId { get; set; }

        /// <inheritdoc/>
        public uint PreviousTransactionLedgerSequence { get; set; }

        /// <inheritdoc/>
        public uint TicketSequence { get; set; }
        /// <inheritdoc/>
        public uint TicketCount { get; set; }
    }
}