using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCreate.ts

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="ICheckCreate" />
    public class CheckCreate : TransactionCommon, ICheckCreate
    {
        public CheckCreate()
        {
            TransactionType = TransactionType.CheckCreate;
        }

        /// <inheritdoc />
        public string Destination { get; set; }
        /// <inheritdoc />
        public Currency SendMax { get; set; }
        /// <inheritdoc />
        public uint? DestinationTag { get; set; }
        /// <inheritdoc />
        public uint? Expiration { get; set; }
        /// <inheritdoc />
        public uint? InvoiceID { get; set; }
    }

    /// <summary>
    /// Create a Check object in the ledger, which is a deferred payment that can be  cashed by its intended destination.<br/>
    /// The sender of this transaction is the  sender of the Check.
    /// </summary>
    public interface ICheckCreate : ITransactionCommon
    {
        /// <summary>
        /// The unique address of the account that can cash the Check.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Maximum amount of source currency the Check is allowed to debit the sender, including transfer fees on non-XRP currencies.<br/>
        /// The Check can only credit the destination with the same currency (from the same issuer, for non-XRP currencies).<br/>
        /// For non-XRP amounts, the nested field names MUST be.<br/>
        /// lower-case.
        /// </summary>
        Currency SendMax { get; set; }
        /// <summary>
        /// Arbitrary tag that identifies the reason for the Check, or a hosted.<br/>
        /// recipient to pay.
        /// </summary>
        uint? DestinationTag { get; set; }
        /// <summary>
        /// Time after which the Check is no longer valid, in seconds since the Ripple.<br/>
        /// Epoch.
        /// </summary>
        uint? Expiration { get; set; }
        /// <summary>
        /// Arbitrary 256-bit hash representing a specific reason or identifier for.<br/>
        /// this Check.
        /// </summary>
        uint? InvoiceID { get; set; }
    }

    /// <inheritdoc cref="ICheckCreate" />
    public class CheckCreateResponse : TransactionResponseCommon, ICheckCreate
    {
        /// <inheritdoc />
        public string Destination { get; set; }
        /// <inheritdoc />
        public Currency SendMax { get; set; }
        /// <inheritdoc />
        public uint? DestinationTag { get; set; }
        /// <inheritdoc />
        public uint? Expiration { get; set; }
        /// <inheritdoc />
        public uint? InvoiceID { get; set; }
    }
    partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a CheckCreate at runtime.
        /// </summary>
        /// <param name="tx"> A CheckCreate Transaction.</param>
        /// <exception cref="ValidationError">When the CheckCreate is malformed.</exception>
        public async Task ValidateCheckCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("SendMax", out var SendMax) || SendMax is null)
                throw new ValidationError("CheckCreate: missing field SendMax");
            if (!tx.TryGetValue("Destination", out var Destination) || Destination is not { })
                throw new ValidationError("CheckCreate: missing field Destination");

            if (SendMax is not string { } || !Common.IsIssuedCurrency(SendMax))
                throw new ValidationError("CheckCreate: invalid SendMax");

            if (Destination is not string { })
                throw new ValidationError("CheckCreate: invalid Destination");

            if (tx.TryGetValue("DestinationTag", out var DestinationTag) && DestinationTag is not uint { })
                throw new ValidationError("CheckCreate: missing field DestinationTag");
            if (tx.TryGetValue("Expiration", out var Expiration) && Expiration is not uint { })
                throw new ValidationError("CheckCreate: missing field Expiration");
            if (tx.TryGetValue("InvoiceID", out var InvoiceID) && InvoiceID is not string { })
                throw new ValidationError("CheckCreate: missing field InvoiceID");


        }
    }

}
