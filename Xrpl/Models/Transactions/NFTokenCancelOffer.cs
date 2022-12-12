

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenCancelOffer.ts

namespace Xrpl.Models.Transactions
{
    /// <inheritdoc cref="INFTokenCancelOffer" />
    public class NFTokenCancelOffer : TransactionCommon, INFTokenCancelOffer
    {
        public NFTokenCancelOffer()
        {
            TransactionType = TransactionType.NFTokenCancelOffer;
        }

        /// <inheritdoc />
        public string[] NFTokenOffers { get; set; }
    }
    /// <summary>
    /// The NFTokenCancelOffer transaction deletes existing NFTokenOffer objects.<br/>
    /// It is useful if you want to free up space on your account to lower your  reserve requirement.<br/>
    /// The transaction can be executed by the account that originally created  the NFTokenOffer, the account in the `Recipient` field of the NFTokenOffer  (if present), or any account if the NFTokenOffer has an `Expiration` and  the NFTokenOffer has already expired.
    /// </summary>
    public interface INFTokenCancelOffer : ITransactionCommon
    {
        /// <summary>
        /// An array of identifiers of NFTokenOffer objects that should be cancelled by this transaction.<br/>
        /// It is an error if an entry in this list points to an object that is not an NFTokenOffer object.<br/>
        /// It is not an error if an entry in this list points to an object that does not exist.<br/>
        /// This field is required.
        /// </summary>
        string[] NFTokenOffers { get; set; }
    }

    /// <inheritdoc cref="INFTokenCancelOffer" />
    public class NFTokenCancelOfferResponse : TransactionResponseCommon, INFTokenCancelOffer
    {
        /// <inheritdoc />
        public string[] NFTokenOffers { get; set; }
    }
}
