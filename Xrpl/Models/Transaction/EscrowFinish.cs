

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/escrowFinish.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Deliver XRP from a held payment to the recipient.
    /// </summary>
    public class EscrowFinish : TransactionCommon, IEscrowFinish
    {
        public EscrowFinish()
        {
            TransactionType = TransactionType.EscrowFinish;
        }

        public EscrowFinish(string owner, uint offerSequence, string condition, string fulfillment)
        {
            Owner = owner;
            OfferSequence = offerSequence;
            Condition = condition;
            Fulfillment = fulfillment;
        }

        /// <inheritdoc />
        public string Owner { get; set; }

        /// <inheritdoc />
        public uint OfferSequence { get; set; }

        /// <inheritdoc />
        public string Condition { get; set; }

        /// <inheritdoc />
        public string Fulfillment { get; set; }
    }

    /// <summary>
    /// Deliver XRP from a held payment to the recipient.
    /// </summary>
    public interface IEscrowFinish : ITransactionCommon
    {
        /// <summary>
        /// Hex value matching the previously-supplied PREIMAGE-SHA-256.<br/>
        /// crypto-condition of the held payment.
        /// </summary>
        string Condition { get; set; }
        /// <summary>
        /// Hex value of the PREIMAGE-SHA-256 crypto-condition fulfillment matching.<br/>
        /// the held payment's Condition.
        /// </summary>
        string Fulfillment { get; set; }
        /// <summary>
        /// Transaction sequence of EscrowCreate transaction that created the held.<br/>
        /// payment to finish.
        /// </summary>
        uint OfferSequence { get; set; }
        /// <summary>
        /// Address of the source account that funded the held payment.
        /// </summary>
        string Owner { get; set; }
    }

    /// <summary>
    /// Deliver XRP from a held payment to the recipient.
    /// </summary>
    public class EscrowFinishResponse : TransactionResponseCommon, IEscrowFinish
    {
        /// <inheritdoc />
        public string Condition { get; set; }
        /// <inheritdoc />
        public string Fulfillment { get; set; }
        /// <inheritdoc />
        public uint OfferSequence { get; set; }
        /// <inheritdoc />
        public string Owner { get; set; }
    }
}
