using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountOffers.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an form an <see cref="AccountOffersRequest"/>.
    /// </summary>
    public class AccountOffers //todo rename to response
    {
        /// <summary>
        /// Unique Address identifying the account that made the offers.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Array of objects, where each object represents an offer made by this  account that is outstanding as of the requested ledger version.<br/>
        /// If the  number of offers is large, only returns up to limit at a time.
        /// </summary>
        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; }
        /// <summary>
        /// The ledger index of the current in-progress ledger version, which was  used when retrieving this data.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// The ledger index of the ledger version that was used when retrieving  this data, as requested.
        /// </summary>
        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }
        /// <summary>
        /// The identifying hash of the ledger version that was used when retrieving  this data.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// Server-defined value indicating the response is paginated.<br/>
        /// Pass this to  the next call to resume where this call left off.<br/>
        /// Omitted when there are  no pages of information after this one.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
    }
    /// <summary>
    /// offer made by account that is outstanding as of the requested ledger version.
    /// </summary>
    public class Offer
    {
        /// <summary>
        /// Options set for this offer entry as bit-flags.
        /// </summary>
        [JsonProperty("flags")]
        public uint Flags { get; set; }
        /// <summary>
        /// Sequence number of the transaction that created this entry.
        /// </summary>
        [JsonProperty("seq")]
        public uint Sequence { get; set; }
        /// <summary>
        /// The amount the account placing this Offer receives.
        /// </summary>
        [JsonProperty("taker_gets")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }
        /// <summary>
        /// The amount the account placing this Offer pays.
        /// </summary>
        [JsonProperty("taker_pays")]
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
        /// <summary>
        /// The exchange rate of the Offer, as the ratio of the original taker_pays divided by the original taker_gets.<br/>
        /// When executing offers, the offer with the most favorable (lowest) quality is consumed first; offers with the same quality are executed from oldest to newest.
        /// </summary>
        [JsonProperty("quality")]
        public string Quality { get; set; }
        /// <summary>
        /// A time after which this offer is considered unfunded, as the number of seconds since the Ripple Epoch.<br/>
        /// See also: Offer Expiration.
        /// </summary>
        [JsonProperty("expiration")]
        [JsonConverter(typeof(RippleDateTimeConverter))]
        public DateTime? Expiration { get; set; }
    }
    /// <summary>
    /// The account_offers method retrieves a list of offers made by a given account  that are outstanding as of a particular ledger version.<br/>
    /// Expects a response in  the form of a <see cref="AccountOffers"/>.
    /// </summary>
    /// <code>
    /// {
    /// 	"id": 9,
    /// 	"command": "account_offers",
    /// 	"account": "rpP2JgiMyTF5jR5hLG3xHCPi1knBb1v9cM"
    /// }
    /// </code>
    public class AccountOffersRequest : BaseLedgerRequest
    {
        public AccountOffersRequest(string account)
        {
            Account = account;
            Command = "account_offers";
        }
        /// <summary>
        /// A unique identifier for the account, most commonly the account's Address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Limit the number of transactions to retrieve.<br/>
        /// The server is not required to honor this value.<br/>
        /// Must be within the inclusive range 10 to 400.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        /// <summary>
        /// Value from a previous paginated response.<br/>
        /// Resume retrieving data where that response left off.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
        /// <summary>
        /// If true, then the account field only accepts a public key or XRP Ledger address.<br/>
        /// Otherwise, account can be a secret or passphrase (not recommended).<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("strict")]
        public bool Strict { get; set; }
    }
}
