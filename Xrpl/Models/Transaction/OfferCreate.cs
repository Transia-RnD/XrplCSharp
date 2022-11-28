using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/offerCreate.ts

namespace Xrpl.Models.Transaction
{

    /// <summary>
    /// Transaction Flags for an OfferCreate Transaction.
    /// </summary>
    [Flags]
    public enum OfferCreateFlags : uint
    {
        /// <summary>
        /// If enabled, the offer does not consume offers that exactly match it, and instead becomes an Offer object in the ledger.<br/>
        /// It still consumes offers that cross it.
        /// </summary>
        tfPassive = 65536,
        /// <summary>
        /// Treat the offer as an Immediate or Cancel order.<br/>
        /// If enabled, the offer never becomes a ledger object: it only tries to match existing offers in the ledger.<br/>
        /// If the offer cannot match any offers immediately, it executes "successfully" without trading any currency.<br/>
        /// In this case, the transaction has the result code tesSUCCESS, but creates no Offer objects in the ledger.
        /// </summary>
        tfImmediateOrCancel = 131072,
        /// <summary>
        /// Treat the offer as a Fill or Kill order.<br/>
        /// Only try to match existing offers in the ledger, and only do so if the entire TakerPays quantity can be obtained.<br/>
        /// If the fix1578 amendment is enabled and the offer cannot be executed when placed, the transaction has the result code tecKILLED;<br/>
        /// otherwise, the transaction uses the result code tesSUCCESS even when it was killed without trading any currency.
        /// </summary>
        tfFillOrKill = 262144,
        /// <summary>
        /// Exchange the entire TakerGets amount, even if it means obtaining more than the TakerPays amount in exchange.
        /// </summary>
        tfSell = 524288
    }

    /// <inheritdoc cref="IOfferCreate" />
    public class OfferCreate : TransactionCommon, IOfferCreate
    {
        public OfferCreate()
        {
            TransactionType = TransactionType.OfferCreate;
        }
        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
        /// <inheritdoc />
        public new OfferCreateFlags? Flags { get; set; }

        /// <inheritdoc />
        public uint? OfferSequence { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
    }

    /// <summary>
    /// An OfferCreate transaction is effectively a limit order.<br/>
    /// It defines an  intent to exchange currencies, and creates an Offer object if not completely.<br/>
    /// Fulfilled when placed.<br/>
    /// Offers can be partially fulfilled.
    /// </summary>
    public interface IOfferCreate : ITransactionCommon
    {
        /// <summary>
        /// Time after which the offer is no longer active, in seconds since the.<br/>
        /// Ripple Epoch.
        /// </summary>
        DateTime? Expiration { get; set; }
        /// <summary>
        /// Transaction Flags for an OfferCreate Transaction.
        /// </summary>
        new OfferCreateFlags? Flags { get; set; }
        /// <summary>
        /// An offer to delete first, specified in the same way as OfferCancel.
        /// </summary>
        uint? OfferSequence { get; set; }
        /// <summary>
        /// The amount and type of currency being provided by the offer creator.
        /// </summary>
        Currency TakerGets { get; set; }
        /// <summary>
        /// The amount and type of currency being requested by the offer creator.
        /// </summary>
        Currency TakerPays { get; set; }

    }

    /// <inheritdoc cref="IOfferCreate" />
    public class OfferCreateResponse : TransactionResponseCommon, IOfferCreate
    {
        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        /// <inheritdoc />
        public new OfferCreateFlags? Flags { get; set; }

        /// <inheritdoc />
        public uint? OfferSequence { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a OfferCreate at runtime.
        /// </summary>
        /// <param name="tx"> A OfferCreate Transaction.</param>
        /// <exception cref="ValidationError">When the OfferCreate is malformed.</exception>
        public static async Task ValidateOfferCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("TakerGets", out var TakerGets) || TakerGets is null)
                throw new ValidationError("OfferCreate: missing field TakerGets");
            if (!tx.TryGetValue("TakerPays", out var TakerPays) || TakerPays is null)
                throw new ValidationError("OfferCreate: missing field TakerPays");

            if (TakerGets is not string && !Common.IsAmount(TakerGets))
                throw new ValidationError("OfferCreate: invalid TakerGets");
            if (TakerPays is not string && !Common.IsAmount(TakerPays))
                throw new ValidationError("OfferCreate: invalid TakerGets");

            if (tx.TryGetValue("Expiration", out var Expiration) && Expiration is not uint { })
                throw new ValidationError("OfferCreate: invalid Expiration");
            if (tx.TryGetValue("OfferSequence", out var OfferSequence) && OfferSequence is not uint { })
                throw new ValidationError("OfferCreate: invalid OfferSequence");

        }
    }

}
