using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelCreate.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="IPaymentChannelCreate" />
    public class PaymentChannelCreate : TransactionCommon, IPaymentChannelCreate
    {
        public PaymentChannelCreate()
        {
            TransactionType = TransactionType.PaymentChannelCreate;
        }

        /// <inheritdoc />
        public string Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint SettleDelay { get; set; }

        /// <inheritdoc />
        public string PublicKey { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public uint? SourceTag { get; set; }
    }

    /// <summary>
    /// Create a unidirectional channel and fund it with XRP.<br/>
    /// The address sending  this transaction becomes the "source address" of the payment channel.
    /// </summary>
    public interface IPaymentChannelCreate : ITransactionCommon
    {
        /// <summary>
        /// Amount of XRP, in drops, to deduct from the sender's balance and set aside in this channel.<br/>
        /// While the channel is open, the XRP can only go to the Destination address.<br/>
        /// When the channel closes, any unclaimed XRP is returned to the source address's balance.
        /// </summary>
        string Amount { get; set; }
        /// <summary>
        /// The time, in seconds since the Ripple Epoch, when this channel expires.<br/>
        /// Any transaction that would modify the channel after this time closes the channel without otherwise affecting it.<br/>
        /// This value is immutable; the channel can be closed earlier than this time but cannot remain open after this time.
        /// </summary>
        DateTime? CancelAfter { get; set; }
        /// <summary>
        /// Address to receive XRP claims against this channel.<br/>
        /// This is also known as the "destination address" for the channel.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Arbitrary tag to further specify the destination for this payment channel, such as a hosted recipient at the destination address.
        /// </summary>
        uint? DestinationTag { get; set; }
        /// <summary>
        /// The public key of the key pair the source will use to sign claims against this channel in hexadecimal.<br/>
        /// This can be any secp256k1 or ed25519 public key.
        /// </summary>
        string PublicKey { get; set; }
        /// <summary>
        /// Amount of time the source address must wait before closing the channel if it has unclaimed XRP.
        /// </summary>
        uint SettleDelay { get; set; }
        /// <summary>
        /// (Optional) Arbitrary integer used to identify the reason for this payment, or a sender on whose behalf this transaction is made.<br/>
        /// Conventionally, a refund should specify the initial payment's SourceTag as the refund payment's DestinationTag.
        /// </summary>
        uint? SourceTag { get; set; }
    }

    /// <inheritdoc cref="IPaymentChannelCreate" />
    public class PaymentChannelCreateResponse : TransactionResponseCommon, IPaymentChannelCreate
    {
        /// <inheritdoc />
        public string Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint SettleDelay { get; set; }

        /// <inheritdoc />
        public string PublicKey { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? CancelAfter { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public uint? SourceTag { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a PaymentChannelCreate at runtime.
        /// </summary>
        /// <param name="tx"> A PaymentChannelCreate Transaction.</param>
        /// <exception cref="ValidationException">When the PaymentChannelCreate is malformed.</exception>
        public static async Task ValidatePaymentChannelCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);


            if (!tx.TryGetValue("Amount", out var Amount) || Amount is null)
                throw new ValidationException("PaymentChannelCreate: missing field Amount");
            if (Amount is not string)
                throw new ValidationException("PaymentChannelCreate: Amount must be a string");
            if (!tx.TryGetValue("Destination", out var Destination) || Destination is null)
                throw new ValidationException("PaymentChannelCreate: missing field Destination");
            if (Destination is not string)
                throw new ValidationException("PaymentChannelCreate: Destination must be a string");
            if (!tx.TryGetValue("SettleDelay", out var SettleDelay) || SettleDelay is null)
                throw new ValidationException("PaymentChannelCreate: missing field SettleDelay");
            if (SettleDelay is not uint)
                throw new ValidationException("PaymentChannelCreate: SettleDelay must be a number");
            if (!tx.TryGetValue("PublicKey", out var PublicKey) || PublicKey is null)
                throw new ValidationException("PaymentChannelCreate: missing field PublicKey");
            if (PublicKey is not string)
                throw new ValidationException("PaymentChannelCreate: PublicKey must be a string");
            if (tx.TryGetValue("CancelAfter", out var CancelAfter) && CancelAfter is not uint)
                throw new ValidationException("PaymentChannelCreate: CancelAfter must be a number");
            if (tx.TryGetValue("DestinationTag", out var DestinationTag) && DestinationTag is not uint)
                throw new ValidationException("PaymentChannelCreate: DestinationTag must be a number");

        }
    }

}
