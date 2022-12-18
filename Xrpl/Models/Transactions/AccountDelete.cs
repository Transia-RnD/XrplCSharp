// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/accountDelete.ts

using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

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

    /// <inheritdoc cref="IAccountDelete" />
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

    /// <inheritdoc cref="IAccountDelete" />
    public class AccountDeleteResponse : TransactionResponseCommon, IAccountDelete
    {
        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }
    }

    public partial class Validation
    {
        //https://github.com/XRPLF/xrpl.js/blob/b40a519a0d949679a85bf442be29026b76c63a22/packages/xrpl/src/models/transactions/accountDelete.ts#L33
        /// <summary>
        /// Verify the form and type of a AccountDelete at runtime.
        /// </summary>
        /// <param name="tx"> A AccountDelete Transaction.</param>
        /// <exception cref="ValidationException">When the AccountDelete is malformed.</exception>
        public static async Task ValidateAccountDelete(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("Destination", out var Destination) || Destination is null)
                throw new ValidationException("AccountDelete: missing field Destination");
            if (Destination is not string { })
                throw new ValidationException("AccountDelete: invalid Destination");

            if (tx.TryGetValue("DestinationTag", out var DestinationTag) && DestinationTag is not uint { })
                throw new ValidationException("AccountDelete: invalid DestinationTag");
        }
    }

}
