using System;
using Newtonsoft.Json;
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
}
