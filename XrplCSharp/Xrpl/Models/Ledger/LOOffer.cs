using Newtonsoft.Json;

using System;

using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Transactions;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Offer.ts

namespace Xrpl.Models.Ledger
{
    public class LOOffer : BaseLedgerEntry
    {
        
        public LOOffer()
        {
            LedgerEntryType = LedgerEntryType.Offer;
        }
        /// <summary>
        /// The address of the account that placed this Offer. 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// A bit-map of boolean flags enabled for this Offer.
        /// </summary>
        public OfferFlags Flags { get; set; }
        /// <summary>
        /// The Sequence value of the OfferCreate transaction that created this Offer object.<br/>
        /// Used in combination with the Account to identify this Offer.
        /// </summary>
        public uint Sequence { get; set; }
        /// <summary>
        /// The remaining amount and type of currency requested by the Offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
        /// <summary>
        /// The remaining amount and type of currency being provided by the Offer creator
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }
        /// <summary>
        /// The ID of the Offer Directory that links to this Offer.
        /// </summary>
        public string BookDirectory { get; set; }
        /// <summary>
        /// A hint indicating which page of the Offer Directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string BookNode { get; set; }
        /// <summary>
        /// A hint indicating which page of the Owner Directory links to this object, in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }
        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }
        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTxnLgrSeq { get; set; }
        /// <summary>
        /// The time this Offer expires, in seconds since the Ripple Epoch.
        /// </summary>
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
}
