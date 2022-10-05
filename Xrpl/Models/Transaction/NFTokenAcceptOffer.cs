

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenAcceptOffer.ts

using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="INFTokenAcceptOffer" />
    public class NFTokenAcceptOffer : TransactionCommon, INFTokenAcceptOffer
    {
        public NFTokenAcceptOffer()
        {
            TransactionType = TransactionType.NFTokenAcceptOffer;
        }

        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        public string NFTokenSellOffer { get; set; }

        /// <inheritdoc />
        public string NFTokenBuyOffer { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency NFTokenBrokerFee { get; set; }
    }

    /// <summary>
    /// The NFTokenOfferAccept transaction is used to accept offers to buy or sell an NFToken.<br/>
    /// It can either:<br/>
    /// 1.<br/>
    /// Allow one offer to be accepted.<br/>
    /// This is called direct  mode.<br/>
    /// 2.<br/>
    /// Allow two distinct offers, one offering to buy a  given NFToken and the other offering to sell the same  NFToken, to be accepted in an atomic fashion.<br/>
    /// This is  called brokered mode.<br/>
    /// To indicate direct mode, use either the `sell_offer` or `buy_offer` fields, but not both.<br/>
    /// To indicate brokered mode, use both the `sell_offer` and `buy_offer` fields.<br/>
    /// If you use neither `sell_offer` nor `buy_offer`, the transaction is invalid.
    /// </summary>
    public interface INFTokenAcceptOffer : ITransactionCommon
    {
        string NFTokenID { get; set; } //todo unknown field
        /// <summary>
        /// Identifies the NFTokenOffer that offers to sell the NFToken.<br/>
        /// In direct mode this field is optional, but either NFTokenSellOffer or  NFTokenBuyOffer must be specified.<br/>
        /// In brokered mode, both NFTokenSellOffer  and NFTokenBuyOffer must be specified.
        /// </summary>
        public string NFTokenSellOffer { get; set; }
        /// <summary>
        /// Identifies the NFTokenOffer that offers to buy the NFToken.<br/>
        /// In direct mode this field is optional, but either NFTokenSellOffer or NFTokenBuyOffer must be specified.<br/>
        /// In brokered mode, both NFTokenSellOffer and NFTokenBuyOffer must be specified.
        /// </summary>
        public string NFTokenBuyOffer { get; set; }
        /// <summary>
        /// This field is only valid in brokered mode.<br/>
        /// It specifies the amount that the broker will keep as part of their fee for bringing the two offers together; the remaining amount will be sent to the seller of the NFToken being bought.<br/>
        /// If specified, the fee must be such that, prior to accounting for the transfer fee charged by the issuer, the amount that the seller would receive is at least as much as the amount indicated in the sell offer.<br/>
        /// This functionality is intended to allow the owner of an NFToken to offer their token for sale to a third party broker, who may then attempt to sell the NFToken on for a larger amount, without the broker having to own the NFToken or custody funds.<br/>
        /// Note: in brokered mode, the offers referenced by NFTokenBuyOffer and NFTokenSellOffer must both specify the same NFTokenID; that is, both must be for the same NFToken.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency NFTokenBrokerFee { get; set; }
    }

    /// <inheritdoc cref="INFTokenAcceptOffer" />
    public class NFTokenAcceptOfferResponse : TransactionResponseCommon, INFTokenAcceptOffer
    {
        /// <inheritdoc />
        public string NFTokenID { get; set; }

        /// <inheritdoc />
        public string NFTokenSellOffer { get; set; }

        /// <inheritdoc />
        public string NFTokenBuyOffer { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency NFTokenBrokerFee { get; set; }
    }
}
