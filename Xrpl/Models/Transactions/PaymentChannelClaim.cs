using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// Enum representing values for PaymentChannelClaim transaction flags.
    /// </summary>
    [Flags]
    public enum PaymentChannelClaimFlags : uint
    {
        /// <summary>
        /// Clear the channel's Expiration time.<br/>
        /// (Expiration is different from the channel's immutable CancelAfter time.)<br/>
        /// Only the source address of the payment channel can use this flag.
        /// </summary>
        tfRenew = 65536,
        /// <summary>
        /// Request to close the channel.<br/>
        /// Only the channel source and destination addresses can use this flag.<br/>
        /// This flag closes the channel immediately if it has no more XRP allocated to it after processing the current claim, or if the destination address uses it.<br/>
        /// If the source address uses this flag when the channel still holds XRP, this schedules the channel to close after SettleDelay seconds have passed.<br/>
        /// (Specifically, this sets the Expiration of the channel to the close time of the previous
        /// ledger plus the channel's SettleDelay time, unless the channel already has an earlier Expiration time.)<br/>
        /// If the destination address uses this flag when the channel still holds XRP, any XRP that remains after processing the claim is returned to the source address.
        /// </summary>
        tfClose = 131072
    }
    /// <inheritdoc cref="IPaymentChannelClaim" />
    public class PaymentChannelClaim : TransactionCommon, IPaymentChannelClaim
    {
        public PaymentChannelClaim()
        {
            TransactionType = TransactionType.PaymentChannelClaim;
        }

        /// <inheritdoc />
        public string Channel { get; set; }

        /// <inheritdoc />
        public string Balance { get; set; }

        /// <inheritdoc />
        public string Amount { get; set; }

        /// <inheritdoc />
        public new PaymentChannelClaimFlags? Flags { get; set; }

        /// <inheritdoc />
        public string Signature { get; set; }

        /// <inheritdoc />
        public string PublicKey { get; set; }
    }
    /// <summary>
    /// Claim XRP from a payment channel, adjust the payment channel's expiration,  or both.
    /// </summary>
    /// <code>
    /// ```typescript
    /// const paymentChannelClaim: PaymentChannelClaim =
    /// {
    /// 	Account: 'rMpxZpuy5RBSP47oK2hDWUtk3B5BNQHfGj,
    /// 	TransactionType: 'PaymentChannelClaim',
    /// 	Channel: hashes.hashPaymentChannel(  'rMpxZpuy5RBSP47oK2hDWUtk3B5BNQHfGj',
    /// 	'rQGYqiyH5Ue9J96p4E6Qt6AvqxK4sDhnS5',
    /// 	21970712,),
    /// 	Amount: '100',
    /// 	Flags:{
    /// 	tfClose: true
    ///     }
    /// }
    /// ```
    /// </code>
    public interface IPaymentChannelClaim : ITransactionCommon
    {
        /// <summary>
        /// The amount of XRP, in drops, authorized by the Signature.<br/>
        /// This must match the amount in the signed message.<br/>
        /// This is the cumulative amount of XRP that can be dispensed by the channel, including XRP previously redeemed.
        /// </summary>
        string Amount { get; set; }
        /// <summary>
        /// Total amount of XRP, in drops, delivered by this channel after processing this claim.<br/>
        /// Required to deliver XRP.<br/>
        /// Must be more than the total amount delivered by the channel so far, but not greater than the Amount of the signed claim.<br/>
        /// Must be provided except when closing the channel.
        /// </summary>
        string Balance { get; set; }
        /// <summary>
        /// The unique ID of the channel as a 64-character hexadecimal string.
        /// </summary>
        string Channel { get; set; }
        /// <summary>
        /// PaymentChannelClaim transaction flags
        /// </summary>
        new PaymentChannelClaimFlags? Flags { get; set; }
        /// <summary>
        /// The public key used for the signature, as hexadecimal.<br/>
        /// This must match the PublicKey stored in the ledger for the channel.<br/>
        /// Required unless the sender of the transaction is the source address of the channel and the Signature field is omitted.
        /// </summary>
        string PublicKey { get; set; }
        /// <summary>
        /// The signature of this claim, as hexadecimal.<br/>
        /// The signed message contains the channel ID and the amount of the claim.<br/>
        /// Required unless the sender of the transaction is the source address of the channel.
        /// </summary>
        string Signature { get; set; }
    }

    /// <inheritdoc cref="IPaymentChannelClaim" />
    public class PaymentChannelClaimResponse : TransactionResponseCommon, IPaymentChannelClaim
    {
        /// <inheritdoc />
        public string Amount { get; set; }
        /// <inheritdoc />
        public string Balance { get; set; }
        /// <inheritdoc />
        public string Channel { get; set; }
        /// <inheritdoc />
        public new PaymentChannelClaimFlags? Flags { get; set; }
        /// <inheritdoc />
        public string PublicKey { get; set; }
        /// <inheritdoc />
        public string Signature { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a PaymentChannelClaim at runtime.
        /// </summary>
        /// <param name="tx"> A PaymentChannelClaim Transaction.</param>
        /// <exception cref="ValidationError">When the PaymentChannelClaim is malformed.</exception>
        public static async Task ValidatePaymentChannelClaim(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);


            if (!tx.TryGetValue("Channel", out var Channel) || Channel is null)
                throw new ValidationError("PaymentChannelClaim: missing field Channel");
            if(Channel is not string)
                throw new ValidationError("PaymentChannelClaim: Channel must be a string");

            if (tx.TryGetValue("Balance", out var Balance) && Balance is not string)
                throw new ValidationError("PaymentChannelClaim: Balance must be a string");
            if (tx.TryGetValue("Amount", out var Amount) && Amount is not string)
                throw new ValidationError("PaymentChannelClaim: Amount must be a string");
            if (tx.TryGetValue("Signature", out var Signature) && Signature is not string)
                throw new ValidationError("PaymentChannelClaim: Signature must be a string");
            if (tx.TryGetValue("PublicKey", out var PublicKey) && PublicKey is not string)
                throw new ValidationError("PaymentChannelClaim: PublicKey must be a string");

        }
    }

}
