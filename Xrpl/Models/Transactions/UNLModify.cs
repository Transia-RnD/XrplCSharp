//https://xrpl.org/unlmodify.html
namespace Xrpl.Models.Transactions
{
    public class UNLModify : TransactionCommon, IUNLModify
    {
        public UNLModify()
        {
            //The value 0x0066, mapped to the string UNLModify, indicates that this object is an UNLModify pseudo-transaction
            TransactionType = TransactionType.UNLModify;
        }

        /// <inheritdoc />
        public string UNLModifyValidator { get; set; }

        /// <inheritdoc />
        public uint UNLModifyDisabling { get; set; }

        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
    }

    public interface IUNLModify : ITransactionCommon
    {
        /// <summary>
        /// The validator to add or remove, as identified by its master public key.
        /// </summary>
        string UNLModifyValidator { get; set; }

        /// <summary>
        /// If 1, this change represents adding a validator to the Negative UNL.<br/>
        /// If 0, this change represents removing a validator from the Negative UNL.<br/>
        /// (No other values are allowed.)
        /// </summary>
        uint UNLModifyDisabling { get; set; }

        /// <summary>
        /// The ledger index where this pseudo-transaction appears.<br/>
        /// This distinguishes the pseudo-transaction from other occurrences of the same change.
        /// </summary>
        uint LedgerSequence { get; set; }
    }

    public class UNLModifyResponse : TransactionResponseCommon, IUNLModify
    {
        /// <inheritdoc />
        public string UNLModifyValidator { get; set; }

        /// <inheritdoc />
        public uint UNLModifyDisabling { get; set; }

        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
    }

}
