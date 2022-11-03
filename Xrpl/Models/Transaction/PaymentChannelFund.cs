using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelFund.ts

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="IPaymentChannelFund" />
    public class PaymentChannelFund : TransactionCommon, IPaymentChannelFund
    {
        public PaymentChannelFund()
        {
            TransactionType = TransactionType.PaymentChannelFund;
        }

        /// <inheritdoc />
        public string Channel { get; set; }

        /// <inheritdoc />
        public string Amount { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }

    /// <summary>
    /// Add additional XRP to an open payment channel, and optionally update the  expiration time of the channel.<br/>
    /// Only the source address of the channel can  use this transaction.
    /// </summary>
    public interface IPaymentChannelFund : ITransactionCommon
    {
        /// <summary>
        /// Amount of XRP in drops to add to the channel.<br/>
        /// Must be a positive amount of XRP.
        /// </summary>
        string Amount { get; set; }
        /// <summary>
        /// The unique ID of the channel to fund as a 64-character hexadecimal string.
        /// </summary>
        string Channel { get; set; }
        /// <summary>
        /// New Expiration time to set for the channel in seconds since the Ripple Epoch.<br/>
        /// This must be later than either the current time plus the SettleDelay of the channel, or the existing Expiration of the channel.<br/>
        /// After the Expiration time, any transaction that would access the channel closes the channel without taking its normal action.<br/>
        /// Any unspent XRP is returned to the source address when the channel closes.<br/>
        /// (Expiration is separate from the channel's immutable CancelAfter time.)<br/>
        /// For more information, see the PayChannel ledger object type.
        /// </summary>
        DateTime? Expiration { get; set; }
    }

    /// <inheritdoc cref="IPaymentChannelFund" />
    public class PaymentChannelFundResponse : TransactionResponseCommon, IPaymentChannelFund
    {
        /// <inheritdoc />
        public string Amount { get; set; }

        /// <inheritdoc />
        public string Channel { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
    partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a PaymentChannelFund at runtime.
        /// </summary>
        /// <param name="tx"> A PaymentChannelFund Transaction.</param>
        /// <exception cref="ValidationError">When the PaymentChannelFund is malformed.</exception>
        public async Task ValidatePaymentChannelFund(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);


            if (!tx.TryGetValue("Channel", out var Channel) || Channel is null)
                throw new ValidationError("PaymentChannelFund: missing field Channel");
            if (Channel is not string)
                throw new ValidationError("PaymentChannelFund: Channel must be a string");
            if (!tx.TryGetValue("Amount", out var Amount) || Amount is null)
                throw new ValidationError("PaymentChannelFund: missing Amount");
            if (Amount is not string)
                throw new ValidationError("PaymentChannelFund: Amount must be a string");
            if (tx.TryGetValue("Expiration", out var Expiration) && Expiration is not uint)
                throw new ValidationError("PaymentChannelFund: Expiration must be a number");
        }
    }

}
