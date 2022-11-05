

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/depositPreauth.ts

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transaction
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

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a DepositPreauth at runtime.
        /// </summary>
        /// <param name="tx"> A DepositPreauth Transaction.</param>
        /// <exception cref="ValidationError">When the DepositPreauth is malformed.</exception>
        public static async Task ValidateDepositPreauth(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            tx.TryGetValue("Authorize", out var Authorize);
            tx.TryGetValue("Unauthorize", out var Unauthorize);

            if (Authorize is null && Unauthorize is null)
                throw new ValidationError("DepositPreauth: must provide either Authorize or Unauthorize field ");

            if (Authorize is not null && Unauthorize is not null)
                throw new ValidationError("DepositPreauth: can't provide both Authorize and Unauthorize fields");
            if (Authorize is { } aut)
            {
                if (aut is not string { })
                    throw new ValidationError("DepositPreauth:  Authorize must be a string");
                if (tx["Account"] == aut)
                    throw new ValidationError("DepositPreauth:  Account can't preauthorize its own address");
            }
            if (Unauthorize is { } un_aut)
            {
                if (un_aut is not string { })
                    throw new ValidationError("DepositPreauth:  Unauthorize must be a string");
                if (tx["Account"] == un_aut)
                    throw new ValidationError("DepositPreauth:  Account can't unauthorize its own address");
            }


        }
    }

}
