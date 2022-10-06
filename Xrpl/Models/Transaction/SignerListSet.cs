using System.Collections.Generic;
using Xrpl.Models.Ledger;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/signerListSet.ts

namespace Xrpl.Models.Transaction
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
        public List<SignerEntry> SignerEntries { get; set; }
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
        List<SignerEntry> SignerEntries { get; set; }
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
        public List<SignerEntry> SignerEntries { get; set; }
        /// <inheritdoc />
        public uint SignerQuorum { get; set; }
    }
}
