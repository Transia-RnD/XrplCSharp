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
}
