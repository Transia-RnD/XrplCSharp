//https://xrpl.org/enableamendment.html
namespace Xrpl.Models.Transactions
{
    public class EnableAmendment : TransactionCommon, IEnableAmendment
    {
        public EnableAmendment()
        {
            TransactionType = TransactionType.EnableAmendment;
        }

        /// <inheritdoc />
        public string Amendment { get; set; }

        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
    }

    public interface IEnableAmendment : ITransactionCommon
    {
        /// <summary>
        /// A unique identifier for the amendment.<br/>
        /// This is not intended to be a human-readable name.<br/>
        /// See Amendments for a list of known amendments.
        /// </summary>
        string Amendment { get; set; }
        /// <summary>
        /// The ledger index where this pseudo-transaction appears.<br/>
        /// This distinguishes the pseudo-transaction from other occurrences of the same change.
        /// </summary>
        uint LedgerSequence { get; set; }
    }

    public class EnableAmendmentResponse : TransactionResponseCommon, IEnableAmendment
    {
        /// <inheritdoc />
        public string Amendment { get; set; }
        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
    }
}
