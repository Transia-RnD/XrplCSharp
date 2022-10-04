using Xrpl.Models.Common;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCash.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Attempts to redeem a Check object in the ledger to receive up to the amount  authorized by the corresponding CheckCreate transaction.<br/>
    /// Only the Destination  address of a Check can cash it with a CheckCash transaction.
    /// </summary>
    public class CheckCash : TransactionCommon, ICheckCash
    {
        public CheckCash()
        {
            TransactionType = TransactionType.CheckCash;
        }
        /// <inheritdoc />
        public string CheckID { get; set; }
        /// <inheritdoc />
        public Currency? Amount { get; set; }
        /// <inheritdoc />
        public Currency? DeliverMin { get; set; }
    }

    /// <summary>
    /// Attempts to redeem a Check object in the ledger to receive up to the amount  authorized by the corresponding CheckCreate transaction.<br/>
    /// Only the Destination  address of a Check can cash it with a CheckCash transaction.
    /// </summary>
    public interface ICheckCash : ITransactionCommon
    {

        /// <summary>
        /// The ID of the Check ledger object to cash as a 64-character hexadecimal string.
        /// </summary>
        string CheckID { get; set; }
        /// <summary>
        /// Redeem the Check for exactly this amount, if possible.<br/>
        /// The currency must match that of the SendMax of the corresponding CheckCreate transaction.<br/>
        /// You.<br/>
        /// must provide either this field or DeliverMin.
        /// </summary>
        Currency? Amount { get; set; }
        /// <summary>
        /// Redeem the Check for at least this amount and for as much as possible.<br/>
        /// The currency must match that of the SendMax of the corresponding CheckCreate.<br/>
        /// transaction.<br/>
        /// You must provide either this field or Amount.
        /// </summary>
        Currency? DeliverMin { get; set; }
    }

    public class CheckCashResponse : TransactionResponseCommon, ICheckCash
    {
        /// <inheritdoc />
        public string CheckID { get; set; }
        /// <inheritdoc />
        public Currency? Amount { get; set; }

        /// <inheritdoc />
        public Currency? DeliverMin { get; set; }  
    }
}
