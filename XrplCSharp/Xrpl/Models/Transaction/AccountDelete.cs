// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/accountDelete.ts

namespace Xrpl.Models.Transactions
{

    /// <summary>
    /// An AccountDelete transaction deletes an account and any objects it owns in  the XRP Ledger,
    /// if possible, sending the account's remaining XRP to a  specified destination account.
    /// </summary>
    public interface IAccountDelete : ITransactionCommon
    {
        /// <summary>
        /// The address of an account to receive any leftover XRP after deleting the sending account.<br/>
        /// Must be a funded account in the ledger, and must not be.<br/>
        /// the sending account.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Arbitrary destination tag that identifies a hosted recipient or other.<br/>
        /// information for the recipient of the deleted account's leftover XRP.
        /// </summary>
        uint? DestinationTag { get; set; }
    }

    /// <summary>
    /// An AccountDelete transaction deletes an account and any objects it owns in  the XRP Ledger,
    /// if possible, sending the account's remaining XRP to a  specified destination account.
    /// </summary>
    public class AccountDelete : TransactionCommon, IAccountDelete
    {
        public AccountDelete()
        {
            TransactionType = TransactionType.AccountDelete;
        }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }
    }

    public class AccountDeleteResponse : TransactionResponseCommon, IAccountDelete
    {
        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }
    }
}
