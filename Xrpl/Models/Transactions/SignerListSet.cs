using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xrpl.Client.Exceptions;
using System.Threading.Tasks;

using Xrpl.Models.Ledger;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/signerListSet.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="ISignerListSet" />
    public class SignerListSet : TransactionCommon, ISignerListSet
    {

        public SignerListSet()
        {
            TransactionType = TransactionType.SignerListSet;
        }

        /// <inheritdoc />
        public uint SignerQuorum { get; set; }
        /// <inheritdoc />
        public List<SignerEntryWrapper> SignerEntries { get; set; }
    }

    /// <summary>
    /// The SignerListSet transaction creates, replaces, or removes a list of  signers that can be used to multi-sign a transaction.
    /// </summary>
    public interface ISignerListSet : ITransactionCommon
    {
        /// <summary>
        /// Array of SignerEntry objects, indicating the addresses and weights of signers in this list.<br/>
        /// This signer list must have at least 1 member and no more than 32 members.<br/>
        /// No address may appear more than once in the list, nor may the Account submitting the transaction appear in the list.
        /// </summary>
        List<SignerEntryWrapper> SignerEntries { get; set; }
        /// <summary>
        /// A target number for the signer weights.<br/>
        /// A multi-signature from this list is valid only if the sum weights of the signatures provided is greater than or equal to this value.<br/>
        /// To delete a signer list, use the value 0.
        /// </summary>
        uint SignerQuorum { get; set; }
    }

    /// <inheritdoc cref="ISignerListSet" />
    public class SignerListSetResponse : TransactionResponseCommon, ISignerListSet
    {
        /// <inheritdoc />
        public List<SignerEntryWrapper> SignerEntries { get; set; }
        /// <inheritdoc />
        public uint SignerQuorum { get; set; }
    }

    public partial class Validation
    {
        private const uint MAX_SIGNERS = 32;
        /// <summary>
        /// Verify the form and type of a SignerListSet at runtime.
        /// </summary>
        /// <param name="tx"> A SignerListSet Transaction.</param>
        /// <exception cref="ValidationException">When the SignerListSet is malformed.</exception>
        public static async Task ValidateSignerListSet(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("SignerQuorum", out var SignerQuorum) || SignerQuorum is null)
                throw new ValidationException("SignerListSet: missing field SignerQuorum");
            if (SignerQuorum is not uint)
                throw new ValidationException("SignerListSet: invalid SignerQuorum");


            if (!tx.TryGetValue("SignerEntries", out var SignerEntries) || SignerEntries is null)
                throw new ValidationException("SignerListSet: missing field SignerEntries");
            if (SignerEntries is not List<dynamic> entries)
                throw new ValidationException("SignerListSet: invalid SignerEntries");

            if(entries.Count==0)
                throw new ValidationException("SignerListSet: need at least 1 member in SignerEntries");

            if(entries.Count> MAX_SIGNERS)
                throw new ValidationException($"SignerListSet: maximum of {MAX_SIGNERS} members allowed in SignerEntries");


            foreach (dynamic entry_val in entries)
            {
                if (entry_val.TryGetValue("SignerEntry", out dynamic entry))
                {
                    if (entry.TryGetValue("WalletLocator", out dynamic val))
                    {
                        
                        if (val.ToString() is string { } WalletLocator && !Regex.IsMatch(WalletLocator, @"^[0-9A-Fa-f]{64}$"))
                            throw new ValidationException($"SignerListSet: WalletLocator in SignerEntry must be a 256-bit (32-byte) hexadecimal value");

                    }
                }
            }
        }
    }

}
