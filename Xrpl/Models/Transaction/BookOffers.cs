using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Methods;

//https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/bookOffers.ts#L51

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// There are several options which can be either enabled or disabled when an OfferCreate transaction creates an offer object.<br/>
    /// In the ledger, flags are represented as binary values that can be combined with bitwise-or operations.<br/>
    /// The bit values for the flags in the ledger are different than the values used to enable or disable those flags in a transaction.<br/>
    /// Ledger flags have names that begin with lsf.
    /// </summary>
    [Flags]
    public enum OfferFlags
    {
        /// <summary>
        /// The object was placed as a passive offer. This has no effect on the object in the ledger.
        /// </summary>
        lsfPassive = 65536,
        /// <summary>
        /// The object was placed as a sell offer.<br/>
        /// This has no effect on the object in the ledger
        /// (because tfSell only matters if you get a better rate than you asked for, which cannot happen after the object enters the ledger).
        /// </summary>
        lsfSell = 131072
    }
    /// <summary>
    /// * Expected response from a <see cref="BookOffersRequest"/>.
    /// </summary>
    public class BookOffers //todo rename to response :BaseResponse
    {
        /// <summary>
        /// The ledger index of the current in-progress ledger version, which was  used to retrieve this information.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// The ledger index of the ledger version that was used when retrieving  this data, as requested.
        /// </summary>
        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }
        /// <summary>
        /// The identifying hash of the ledger version that was used when retrieving  this data, as requested.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// Array of offer objects, each of which has the fields of an Offer object.
        /// </summary>
        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; }

        //todo not found field  validated?: boolean

    }

    //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/ledger/Offer.ts
    /// <summary>
    /// The Offer object type describes an offer to exchange currencies, more traditionally known as an order, in the XRP Ledger's distributed exchange.<br/>
    /// An OfferCreate transaction only creates an Offer object in the ledger when the offer cannot be fully executed
    /// immediately by consuming other offers already in the ledger.
    /// </summary>
    public class Offer 
    {
        public Offer()
        {
            LedgerEntryType = LedgerEntryType.Offer;
        }
        /// <summary>
        /// The value 0x006F, mapped to the string Offer, indicates that this object describes an order to trade currency.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; } //todo unknown field
        /// <summary>
        /// The address of the account that placed this Offer.
        /// </summary>
        public string Account { get; set; }

        public decimal AmountEach
        {
            get
            {
                if ((TakerPays.ValueAsXrp ?? TakerPays.ValueAsNumber) != 0)
                {
                    return (TakerGets.ValueAsXrp ?? TakerGets.ValueAsNumber) /
                           (TakerPays.ValueAsXrp ?? TakerPays.ValueAsNumber);
                }
                return 0;
            }
        }
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
        /// The remaining amount and type of currency being provided by the Offer creator.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }
        /// <summary>
        /// The ID of the Offer Directory that links to this Offer.
        /// </summary>
        public string BookDirectory { get; set; }
        /// <summary>
        /// A hint indicating which page of the Offer Directory links to this object, in case the directory consists of multiple pages.
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


        //todo move this fields to BookOffer class
        //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/bookOffers.ts#L51

        /// <summary>
        /// Amount of the TakerGets currency the side placing the offer has available to be traded.<br/>
        /// (XRP is represented as drops; any other currency is represented as a decimal value.<br/>
        /// ) If a trader has multiple offers in the same book, only the highest-ranked offer includes this field.
        /// </summary>
        [JsonProperty("owner_funds")]
        public string OwnerFunds { get; set; }
        /// <summary>
        /// The maximum amount of currency that the taker can get, given the funding status of the offer.
        /// </summary>
        [JsonProperty("taker_gets_funded")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGetsFunded { get; set; }
        /// <summary>
        /// The maximum amount of currency that the taker would pay, given the funding status of the offer.
        /// </summary>
        [JsonProperty("taker_pays_funded")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPaysFunded { get; set; }
        /// <summary>
        /// The exchange rate, as the ratio taker_pays divided by taker_gets.<br/>
        /// For fairness, offers that have the same quality are automatically taken first-in, first-out.
        /// </summary>
        [JsonProperty("quality")]
        public double? Quality { get; set; }
    }
}
