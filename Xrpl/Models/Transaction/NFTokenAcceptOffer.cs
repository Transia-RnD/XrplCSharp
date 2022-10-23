

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenAcceptOffer.ts

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
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

    partial class Validation
    {
        //https://github.com/XRPLF/xrpl.js/blob/b40a519a0d949679a85bf442be29026b76c63a22/packages/xrpl/src/models/transactions/NFTokenAcceptOffer.ts#L67
        public Task ValidateNFTokenBrokerFee(Dictionary<string, dynamic> tx)
        {
            if (!tx.TryGetValue("NFTokenBrokerFee", out var NFTokenBrokerFee) || NFTokenBrokerFee is null)
                throw new ValidationError("NFTokenAcceptOffer: invalid NFTokenBrokerFee");

            var value = Common.ParseAmountValue(NFTokenBrokerFee);
            if (double.IsNaN(value))
                throw new ValidationError("NFTokenAcceptOffer: invalid NFTokenBrokerFee");
            if (value <= 0)
                throw new ValidationError("NFTokenAcceptOffer: NFTokenBrokerFee must be greater than 0; omit if there is no fee");

            if (!tx.TryGetValue("NFTokenSellOffer", out var NFTokenSellOffer) ||
                !tx.TryGetValue("NFTokenBuyOffer", out var NFTokenBuyOffer) ||
                NFTokenSellOffer is null || NFTokenBuyOffer is null)
                throw new ValidationError("NFTokenAcceptOffer: both NFTokenSellOffer and NFTokenBuyOffer must be set if using brokered mode");

            return Task.CompletedTask;
        }
        //https://github.com/XRPLF/xrpl.js/blob/b40a519a0d949679a85bf442be29026b76c63a22/packages/xrpl/src/models/transactions/NFTokenAcceptOffer.ts#L92
        /// <summary>
        /// Verify the form and type of an NFTokenAcceptOffer at runtime.
        /// </summary>
        /// <param name="tx">An NFTokenAcceptOffer Transaction.</param>
        /// <exception cref="ValidationError">When the NFTokenAcceptOffer is Malformed.</exception>
        public async Task ValidateNFTokenAcceptOffer(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            var can_get_value_NFTokenSellOffer = tx.TryGetValue("NFTokenSellOffer", out var NFTokenSellOffer);
            var can_get_value_NFTokenBuyOffer = tx.TryGetValue("NFTokenBuyOffer", out var NFTokenBuyOffer);

            if (tx.TryGetValue("NFTokenBrokerFee", out var NFTokenBrokerFee) && NFTokenBrokerFee is not null)
                await ValidateNFTokenBrokerFee(tx);

            if ((!can_get_value_NFTokenSellOffer && !can_get_value_NFTokenBuyOffer) || (NFTokenSellOffer is null && NFTokenBuyOffer is null))
                throw new ValidationError("NFTokenAcceptOffer: must set either NFTokenSellOffer or NFTokenBuyOffer");
        }

    }

}
