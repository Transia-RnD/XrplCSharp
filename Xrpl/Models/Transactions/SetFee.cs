//https://xrpl.org/setfee.html
namespace Xrpl.Models.Transactions
{
    public class SetFee : TransactionCommon, ISetFee
    {
        public SetFee()
        {
            TransactionType = TransactionType.SetFee;
        }

        /// <inheritdoc />
        public string BaseFee { get; set; }

        /// <inheritdoc />
        public uint ReferenceFeeUnits { get; set; }

        /// <inheritdoc />
        public uint ReserveBase { get; set; }

        /// <inheritdoc />
        public uint ReserveIncrement { get; set; }

        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
    }

    public interface ISetFee : ITransactionCommon
    {
        /// <summary>
        /// The charge, in drops of XRP, for the reference transaction, as hex.<br/>
        /// (This is the transaction cost before scaling for load.)
        /// </summary>
        string BaseFee { get; set; }
        /// <summary>
        /// (Omitted for some historical SetFee pseudo-transactions)<br/>
        /// The index of the ledger version where this pseudo-transaction appears.<br/>
        /// This distinguishes the pseudo-transaction from other occurrences of the same change.
        /// </summary>
        uint LedgerSequence { get; set; }
        /// <summary>
        /// The cost, in fee units, of the reference transaction
        /// </summary>
        uint ReferenceFeeUnits { get; set; }
        /// <summary>
        /// The base reserve, in drops
        /// </summary>
        uint ReserveBase { get; set; }
        /// <summary>
        /// The incremental reserve, in drops
        /// </summary>
        uint ReserveIncrement { get; set; }
    }

    public class SetFeeResponse : TransactionResponseCommon, ISetFee
    {
        /// <inheritdoc />
        public string BaseFee { get; set; }
        /// <inheritdoc />
        public uint LedgerSequence { get; set; }
        /// <inheritdoc />
        public uint ReferenceFeeUnits { get; set; }
        /// <inheritdoc />
        public uint ReserveBase { get; set; }
        /// <inheritdoc />
        public uint ReserveIncrement { get; set; }
    }
}
