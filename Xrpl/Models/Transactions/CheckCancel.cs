

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCancel.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="ICheckCancel" />
    public class CheckCancel : TransactionCommon, ICheckCancel
    {
        public CheckCancel()
        {
            TransactionType = TransactionType.CheckCancel;
        }

        /// <inheritdoc />
        public string CheckID { get; set; }
    }

    /// <summary>
    /// Cancels an unredeemed Check, removing it from the ledger without sending any  money.<br/>
    /// The source or the destination of the check can cancel a Check at any  time using this transaction type.<br/>
    /// If the Check has expired, any address can  cancel it.
    /// </summary>
    public interface ICheckCancel : ITransactionCommon
    {
        /// <summary>
        /// The ID of the Check ledger object to cancel as a 64-character hexadecimal string.
        /// </summary>
        string CheckID { get; set; }
    }

    /// <inheritdoc cref="ICheckCancel" />
    public class CheckCancelResponse : TransactionResponseCommon, ICheckCancel
    {
        /// <inheritdoc />
        public string CheckID { get; set; }    
    }
}
