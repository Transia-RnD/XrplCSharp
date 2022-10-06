

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/setRegularKey.ts

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="ISetRegularKey" />
    public class SetRegularKey : TransactionCommon, ISetRegularKey
    {
        public SetRegularKey()
        {
            TransactionType = TransactionType.SetRegularKey;
        }


        /// <inheritdoc />
        public string RegularKey { get; set; }
    }

    /// <summary>
    /// A SetRegularKey transaction assigns, changes, or removes the regular key  pair associated with an account.
    /// </summary>
    public interface ISetRegularKey : ITransactionCommon
    {
        /// <summary>
        /// A base-58-encoded Address that indicates the regular key pair to be assigned to the account.<br/>
        /// If omitted, removes any existing regular key pair from the account.<br/>
        /// Must not match the master key pair for the address.
        /// </summary>
        string RegularKey { get; set; }
    }

    /// <inheritdoc cref="ISetRegularKey" />
    public class SetRegularKeyResponse : TransactionResponseCommon, ISetRegularKey
    {
        /// <inheritdoc />
        public string RegularKey { get; set; }
    }
}
