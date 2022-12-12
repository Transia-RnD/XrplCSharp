using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/escrowCreate.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="IEscrowCreate" />
    public class EscrowCreate : TransactionCommon, IEscrowCreate
    {
        public EscrowCreate()
        {
            TransactionType = TransactionType.EscrowCreate;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        /// <inheritdoc />
        public string Condition { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public uint? SourceTag { get; set; }
    }

    /// <summary>
    /// Sequester XRP until the escrow process either finishes or is canceled.
    /// </summary>
    public interface IEscrowCreate : ITransactionCommon
    {
        /// <summary>
        /// Amount of XRP, in drops, to deduct from the sender's balance and escrow.<br/>
        /// Once escrowed, the XRP can either go to the Destination address (after the.<br/>
        /// FinishAfter time) or returned to the sender (after the CancelAfter time).
        /// </summary>
        Currency Amount { get; set; }
        /// <summary>
        /// The time, in seconds since the Ripple Epoch, when this escrow expires.<br/>
        /// This value is immutable; the funds can only be returned the sender after.<br/>
        /// this time.
        /// </summary>
        DateTime? CancelAfter { get; set; }
        /// <summary>
        /// Hex value representing a PREIMAGE-SHA-256 crypto-condition.<br/>
        /// The funds can.<br/>
        /// only be delivered to the recipient if this condition is fulfilled.
        /// </summary>
        string Condition { get; set; }
        /// <summary>
        /// Address to receive escrowed XRP.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Arbitrary tag to further specify the destination for this escrowed.<br/>
        /// payment, such as a hosted recipient at the destination address.
        /// </summary>
        uint? DestinationTag { get; set; }
        /// <summary>
        /// The time, in seconds since the Ripple Epoch, when the escrowed XRP can be released to the recipient.<br/>
        /// This value is immutable; the funds cannot move.<br/>
        /// until this time is reached.
        /// </summary>
        DateTime? FinishAfter { get; set; }
        uint? SourceTag { get; set; } //todo unknown field
    }

    /// <inheritdoc cref="IEscrowCreate" />
    public class EscrowCreateResponse : TransactionResponseCommon, IEscrowCreate
    {
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? FinishAfter { get; set; }

        /// <inheritdoc />
        public string Condition { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public uint? SourceTag { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a EscrowCreate at runtime.
        /// </summary>
        /// <param name="tx"> A EscrowCreate Transaction.</param>
        /// <exception cref="ValidationException">When the EscrowCreate is malformed.</exception>
        public static async Task ValidateEscrowCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            tx.TryGetValue("Amount", out var Amount);

            if (Amount is null)
                throw new ValidationException("EscrowCreate: missing field Amount");

            if (Amount is not string)
                throw new ValidationException("EscrowCreate: Amount must be a string");


            tx.TryGetValue("Destination", out var Destination);
            if (Destination is null)
                throw new ValidationException("EscrowCreate: missing field Destination");
            if (Destination is not string)
                throw new ValidationException("EscrowCreate: Destination must be a string");

            tx.TryGetValue("CancelAfter", out var CancelAfter);
            tx.TryGetValue("FinishAfter", out var FinishAfter);
            tx.TryGetValue("Condition", out var Condition);

            if (CancelAfter is null && FinishAfter is null)
                throw new ValidationException("EscrowCreate: Either CancelAfter or FinishAfter must be specified");

            if (FinishAfter is null && Condition is null)
                throw new ValidationException("EscrowCreate: Either Condition or FinishAfter must be specified");

            if (CancelAfter is not null && CancelAfter is not uint)
                throw new ValidationException("EscrowCreate: CancelAfter must be a number");
            if (FinishAfter is not null && FinishAfter is not uint)
                throw new ValidationException("EscrowCreate: FinishAfter must be a number");
            if (Condition is not null && Condition is not string)
                throw new ValidationException("EscrowCreate: Condition must be a string");

            tx.TryGetValue("DestinationTag", out var DestinationTag);
            if (Destination is not null && DestinationTag is not uint)
                throw new ValidationException("EscrowCreate: DestinationTag must be a number");
        }
    }

}
