using System;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Transaction Flags for an NFTokenCreateOffer Transaction.
    /// </summary>
    [Flags]
    public enum NFTokenCreateOfferFlags : uint
    {
        /// <summary>
        /// If set, indicates that the offer is a sell offer.<br/>
        /// Otherwise, it is a buy offer.
        /// </summary>
        tfSellToken = 1
    }
    /// <inheritdoc cref="INFTokenCreateOffer" />
    public class NFTokenCreateOffer : TransactionCommon, INFTokenCreateOffer
    {
        public NFTokenCreateOffer()
        {
            TransactionType = TransactionType.NFTokenCreateOffer;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        /// <inheritdoc />
        public new NFTokenCreateOfferFlags? Flags { get; set; }

        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Owner { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }
    }

    /// <summary>
    /// The NFTokenCreateOffer transaction creates either an offer to buy an  NFT the submitting account does not own,
    /// or an offer to sell an NFT  the submitting account does own.
    /// </summary>
    public interface INFTokenCreateOffer : ITransactionCommon
    {
        /// <summary>
        /// Indicates the time after which the offer will no longer be valid.<br/>
        /// The value is the number of seconds since the Ripple Epoch.
        /// </summary>
        DateTime? Expiration { get; set; }
        /// <summary>
        /// If set, indicates that the offer is a sell offer.<br/>
        /// Otherwise, it is a buy offer.
        /// </summary>
        new NFTokenCreateOfferFlags? Flags { get; set; }
        /// <summary>
        /// Identifies the NFTokenID of the NFToken object that the offer references.
        /// </summary>
        string NFTokenID { get; set; }
        /// <summary>
        /// Indicates the amount expected or offered for the Token.<br/>
        /// The amount must be non-zero, except when this is a sell offer and the asset is XRP.<br/>
        /// This would indicate that the current owner of the token is giving it away free, either to anyone at all, or to the account identified by the Destination field.
        /// </summary>
        Currency Amount { get; set; }
        /// <summary>
        /// Indicates the AccountID of the account that owns the corresponding NFToken.<br/>
        /// If the offer is to buy a token, this field must be present and it must be different than Account (since an offer to buy a token one already holds is meaningless).<br/>
        /// If the offer is to sell a token, this field must not be present, as the owner is, implicitly, the same as Account (since an offer to sell a token one doesn't already hold is meaningless).
        /// </summary>
        string Owner { get; set; }
        /// <summary>
        /// If present, indicates that this offer may only be accepted by the specified account.<br/>
        /// Attempts by other accounts to accept this offer MUST fail.
        /// </summary>
        string Destination { get; set; }
    }

    /// <inheritdoc cref="INFTokenCreateOffer" />
    public class NFTokenCreateOfferResponse : TransactionResponseCommon, INFTokenCreateOffer
    {
        /// <inheritdoc />
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }

        /// <inheritdoc />
        public new NFTokenCreateOfferFlags? Flags { get; set; }

        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Owner { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

    }
}
