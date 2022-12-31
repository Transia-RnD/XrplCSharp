// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/accountSet.ts

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// Enum for AccountSet Flags.
    /// </summary>
    public enum AccountSetTfFlags //todo rename to AccountSetAsfFlags https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/transactions/accountSet.ts#L11
    {
        /// <summary>
        /// Require a destination tag to send transactions to this account.
        /// </summary>
        asfRequireDest = 1,
        /// <summary>
        /// Require authorization for users to hold balances issued by this address can only be enabled if the address has no trust lines connected to it.
        /// </summary>
        asfRequireAuth = 2,
        /// <summary>
        /// XRP should not be sent to this account.
        /// </summary>
        asfDisallowXRP = 3,
        /// <summary>
        /// Disallow use of the master key pair.<br/>
        /// Can only be enabled if the account has configured another way to sign transactions, such as a Regular Key or a Signer List.
        /// </summary>
        asfDisableMaster = 4,
        /// <summary>
        /// Track the ID of this account's most recent transaction.<br/>
        /// Required for AccountTxnID.
        /// </summary>
        asfAccountTxnID = 5,
        /// <summary>
        /// Permanently give up the ability to freeze individual trust lines or disable Global Freeze.<br/>
        /// This flag can never be disabled after being enabled.
        /// </summary>
        asfNoFreeze = 6,
        /// <summary>
        /// Freeze all assets issued by this account.
        /// </summary>
        asfGlobalFreeze = 7,
        /// <summary>
        /// Enable rippling on this account's trust lines by default.
        /// </summary>
        asfDefaultRipple = 8,
        /// <summary>
        /// Enable Deposit Authorization on this account.
        /// </summary>
        asfDepositAuth = 9,
        /// <summary>
        /// Allow another account to mint and burn tokens on behalf of this account.
        /// </summary>
        asfAuthorizedNFTokenMinter = 10
    }

    //todo enum AccountSetTfFlags https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/transactions/accountSet.ts#L54
    //public interface IAccountSetFlags
    //{
    //    uint asfRequireDest { get; set; }
    //    uint asfRequireAuth { get; set; }
    //    uint asfDisallowXRP { get; set; }
    //    uint asfDisableMaster { get; set; }
    //    uint asfAccountTxnID { get; set; }
    //    uint asfNoFreeze { get; set; }
    //    uint asfGlobalFreeze { get; set; }
    //    uint asfDefaultRipple { get; set; }
    //}

    /// <inheritdoc cref="IAccountSet" />
    public class AccountSet : TransactionCommon, IAccountSet
    {
        public AccountSet()
        {
            TransactionType = TransactionType.AccountSet;
        }

        public AccountSet(string account) : this()
        {
            Account = account;
        }
        /// <inheritdoc />
        public uint? ClearFlag { get; set; }
        /// <inheritdoc />
        public string Domain { get; set; }
        /// <inheritdoc />
        public string EmailHash { get; set; }
        /// <inheritdoc />
        public string MessageKey { get; set; }
        /// <inheritdoc />
        public uint? SetFlag { get; set; }
        /// <inheritdoc />
        public uint? TransferRate { get; set; }
        /// <inheritdoc />
        public uint? TickSize { get; set; }
    }

    /// <summary>
    /// An AccountSet transaction modifies the properties of an account in the XRP  Ledger.
    /// </summary>
    public interface IAccountSet : ITransactionCommon
    {
        /// <summary>
        /// Unique identifier of a flag to disable for this account.
        /// </summary>
        uint? ClearFlag { get; set; }
        /// <summary>
        /// The domain that owns this account, as a string of hex representing the.<br/>
        /// ASCII for the domain in lowercase.
        /// </summary>
        string Domain { get; set; }
        /// <summary>
        /// Hash of an email address to be used for generating an avatar image.
        /// </summary>
        string EmailHash { get; set; }
        /// <summary>
        /// Public key for sending encrypted messages to this account.
        /// </summary>
        string MessageKey { get; set; }
        /// <summary>
        /// Integer flag to enable for this account.
        /// </summary>
        uint? SetFlag { get; set; } //todo change to enum AccountSetAsfFlags
        /// <summary>
        /// The fee to charge when users transfer this account's issued currencies, represented as billionths of a unit.<br/>
        /// Cannot be more than 2000000000 or less than 1000000000, except for the special case 0 meaning no fee.
        /// </summary>
        uint? TransferRate { get; set; }
        /// <summary>
        /// Tick size to use for offers involving a currency issued by this address.<br/>
        /// The exchange rates of those offers is rounded to this many significant digits.<br/>
        /// Valid values are 3 to 15 inclusive, or 0 to disable.
        /// </summary>
        uint? TickSize { get; set; }

        //todo not found field NFTokenMinter?: string
    }

    /// <inheritdoc cref="IAccountSet" />
    public class AccountSetResponse : TransactionResponseCommon, IAccountSet
    {
        /// <inheritdoc />
        public uint? ClearFlag { get; set; }
        /// <inheritdoc />
        public string Domain { get; set; }
        /// <inheritdoc />
        public string EmailHash { get; set; }
        /// <inheritdoc />
        public string MessageKey { get; set; }
        /// <inheritdoc />
        public uint? SetFlag { get; set; }
        /// <inheritdoc />
        public uint? TransferRate { get; set; }
        /// <inheritdoc />
        public uint? TickSize { get; set; }
    }

    public partial class Validation
    {
        private const uint MIN_TICK_SIZE = 3;

        private const uint MAX_TICK_SIZE = 15;

        /// <summary>
        /// Verify the form and type of a AccountSet at runtime.
        /// </summary>
        /// <param name="tx"> A AccountSet Transaction.</param>
        /// <exception cref="ValidationException">When the AccountSet is malformed.</exception>
        public static async Task ValidateAccountSet(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (tx.TryGetValue("ClearFlag", out var ClearFlag) && ClearFlag is not null)
            {
                if (ClearFlag is not uint { } flag )
                    throw new ValidationException("AccountSet: invalid ClearFlag");

                if (Enum.GetValues<AccountSetTfFlags>().All(c => (uint)c != flag))
                    throw new ValidationException("AccountSet: invalid ClearFlag");
            }
            if (tx.TryGetValue("Domain", out var Domain) && Domain is not string { })
                throw new ValidationException("AccountSet: invalid Domain");

            if (tx.TryGetValue("EmailHash", out var EmailHash) && EmailHash is not string { })
                throw new ValidationException("AccountSet: invalid EmailHash");

            if (tx.TryGetValue("MessageKey", out var MessageKey) && MessageKey is not string { })
                throw new ValidationException("AccountSet: invalid MessageKey");

            if (tx.TryGetValue("SetFlag", out var SetFlag) && SetFlag is not null)
            {
                if (SetFlag is not uint { })
                    throw new ValidationException("AccountSet: invalid SetFlag");

                if (Enum.GetValues<AccountSetTfFlags>().All(c => (uint)c != SetFlag))
                    throw new ValidationException("AccountSet: missing field Destination");
            }

            if (tx.TryGetValue("TransferRate", out var TransferRate) && TransferRate is not uint { })
                throw new ValidationException("AccountSet: invalid TransferRate");

            if (tx.TryGetValue("TickSize", out var TickSize) && TickSize is not null)
            {
                if (TickSize is not uint { } size)
                    throw new ValidationException("AccountSet: invalid TickSize");

                if (size is < MIN_TICK_SIZE or > MAX_TICK_SIZE)
                    throw new ValidationException("AccountSet: invalid TickSize");
            }

        }
    }

}
