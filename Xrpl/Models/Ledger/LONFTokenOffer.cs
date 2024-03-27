using Newtonsoft.Json;

using System;

using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;

namespace Xrpl.Models.Methods
{
    public enum NFTokenOffer
    {
        /// <summary>
        /// If enabled, the offer is a sell offer.
        /// Otherwise, the offer is a buy offer.
        /// </summary>
        lsfSellNFToken = 0x00000001
    }
    public class LONFTokenOffer : BaseLedgerEntry
    {

        public LONFTokenOffer()
        {
            //The type of ledger object (0x0074).
            LedgerEntryType = LedgerEntryType.NFTokenOffer;
        }
        /// <summary>
        /// A set of flags associated with this object, used to specify various options or settings. Flags are listed in the table below.
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// Amount expected or offered for the NFToken. If the token has the lsfOnlyXRP flag set, the amount must be specified in XRP.<br/>
        /// Sell offers that specify assets other than XRP must specify a non-zero amount.<br/>
        /// Sell offers that specify XRP can be 'free' (that is, the Amount field can be equal to "0").
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <summary>
        /// The AccountID for which this offer is intended. If present, only that account can accept the offer.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// The time after which the offer is no longer active. The value is the number of seconds since the Ripple Epoch.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// Internal bookkeeping, indicating the page inside the token buy or sell offer directory, as appropriate, where this token is being tracked.
        /// This field allows the efficient deletion of offers.
        /// </summary>
        public string NFTokenOfferNode { get; set; }
        /// <summary>
        /// NFTokenID of the NFToken object referenced by this offer.
        /// </summary>
        public string NFTokenID { get; set; }
        /// <summary>
        /// Owner of the account that is creating and owns the offer.
        /// Only the current Owner of an NFToken can create an offer to sell an NFToken,
        /// but any account can create an offer to buy an NFToken.
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// Internal bookkeeping, indicating the page inside the owner directory where this token is being tracked.
        /// This field allows the efficient deletion of offers.
        /// </summary>
        public string OwnerNode { get; set; }
        /// <summary>
        /// Identifying hash of the transaction that most recently modified this object.
        /// </summary>
        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }
        /// <summary>
        /// Index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }

    }
}
