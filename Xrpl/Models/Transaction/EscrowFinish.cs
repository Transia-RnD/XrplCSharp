

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/escrowFinish.ts

using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="IEscrowFinish" />
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

    /// <inheritdoc cref="IEscrowFinish" />
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

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a EscrowFinish at runtime.
        /// </summary>
        /// <param name="tx"> A EscrowFinish Transaction.</param>
        /// <exception cref="ValidationError">When the EscrowFinish is malformed.</exception>
        public static async Task ValidateEscrowFinish(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Owner", out var Owner) || Owner is null)
                throw new ValidationError("EscrowFinish: missing field Owner");

            if (Owner is not string)
                throw new ValidationError("EscrowFinish: Owner must be a string");


            
            if (!tx.TryGetValue("OfferSequence", out var OfferSequence) || OfferSequence is null)
                throw new ValidationError("EscrowFinish: missing field OfferSequence");
            if (OfferSequence is not uint)
                throw new ValidationError("EscrowFinish: Destination must be a number");

            if (tx.TryGetValue("Condition", out var Condition) && Condition is not string)
                throw new ValidationError("EscrowFinish: Condition must be a string");
            if (tx.TryGetValue("Fulfillment", out var Fulfillment) && Fulfillment is not string)
                throw new ValidationError("EscrowFinish: Fulfillment must be a string");

        }
    }

}
