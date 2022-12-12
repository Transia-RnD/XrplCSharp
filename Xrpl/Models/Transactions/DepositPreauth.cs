

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/depositPreauth.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="IDepositPreauth" />
    public class DepositPreauth : TransactionCommon, IDepositPreauth
    {
        public DepositPreauth()
        {
            TransactionType = TransactionType.DepositPreauth;
        }

        /// <inheritdoc />
        public string Authorize { get; set; }
        /// <inheritdoc />
        public string Unauthorize { get; set; }
    }

    /// <summary>
    /// A DepositPreauth transaction gives another account pre-approval to deliver  payments to the sender of this transaction.<br/>
    /// This is only useful if the sender  of this transaction is using (or plans to use) Deposit Authorization.
    /// </summary>
    public interface IDepositPreauth : ITransactionCommon
    {
        /// <summary>
        /// The XRP Ledger address of the sender to preauthorize.
        /// </summary>
        string Authorize { get; set; }
        /// <summary>
        /// The XRP Ledger address of a sender whose preauthorization should be.<br/>
        /// revoked.
        /// </summary>
        string Unauthorize { get; set; }
    }

    /// <inheritdoc cref="IDepositPreauth" />
    public class DepositPreauthResponse : TransactionResponseCommon, IDepositPreauth
    {
        /// <inheritdoc />
        public string Authorize { get; set; }
        /// <inheritdoc />
        public string Unauthorize { get; set; }    
    }
}
