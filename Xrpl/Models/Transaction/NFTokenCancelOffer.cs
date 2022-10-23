

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenCancelOffer.ts

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;

namespace Xrpl.Models.Transaction
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
    partial class Validation
    {
        /// <summary>
        /// Verify the form and type of an NFTokenCancelOffer at runtime.
        /// </summary>
        /// <param name="tx">An NFTokenCancelOffer Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationError">When the NFTokenCancelOffer is Malformed.</exception>
        public async Task ValidateNFTokenCancelOffer(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);
            if (!tx.TryGetValue("NFTokenOffers ", out var NFTokenOffers) || NFTokenOffers is not List<dynamic> { } offers)
                throw new ValidationError("NFTokenCancelOffer : missing field NFTokenOffers ");

            if (offers.Count == 0)
                throw new ValidationError("NFTokenCancelOffer: empty field NFTokenOffers");
        }

    }

}
