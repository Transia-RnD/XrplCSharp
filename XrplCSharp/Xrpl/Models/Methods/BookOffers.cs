using Newtonsoft.Json;
using Xrpl.Models.Transactions;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/bookOffers.ts

namespace Xrpl.Models.Methods
{
    public class TakerAmount
    {
        /// <summary>
        /// The standard format for currency codes is a three-character string such as USD.<br/>
        /// This is intended for use with ISO 4217 Currency Codes <br/>
        /// As a 160-bit hexadecimal string, such as "0158415500000000C1F76FF6ECB0BAC600000000".<br/>
        /// The following characters are permitted:<br/>
        /// all uppercase and lowercase letters, digits, as well as the symbols ? ! @ # $ % ^ * ( ) { } [ ] | and symbols ampersand, less, greater<br/>
        /// Currency codes are case-sensitive.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        /// <summary>
        /// Generally, the account that issues this token.<br/>
        /// In special cases, this can refer to the account that holds the token instead.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
    /// <summary>
    /// The book_offers method retrieves a list of offers, also known as the order.<br/>
    /// Book, between two currencies.<br/>
    /// Returns an  <see cref="BookOffers"/>.
    /// </summary>
    /// <code>
    /// {
    /// 	"id": 4,
    /// 	"command": "book_offers",
    /// 	"taker": "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59",
    /// 	"taker_gets":{
    /// 	"currency": "XRP"},
    /// 	"taker_pays":{
    /// 	"currency": "USD",
    /// 	"issuer": "rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B"},
    /// 	"limit": 10}
    /// </code>
    public class BookOffersRequest : BaseLedgerRequest
    {
        public BookOffersRequest()
        {
            Command = "book_offers";
        }
        /// <summary>
        /// If provided, the server does not provide more than this many offers in the results.<br/>
        /// The total number of results returned may be fewer than the limit, because the server omits unfunded offers.
        /// </summary>
        [JsonProperty("limit")]
        public uint? Limit { get; set; }
        /// <summary>
        /// The Address of an account to use as a perspective.<br/>
        /// Unfunded offers placed by this account are always included in the response.
        /// </summary>
        [JsonProperty("taker")]
        public string Taker { get; set; }
        /// <summary>
        /// Specification of which currency the account taking the offer would receive,
        /// as an object with currency and issuer fields (omit issuer for XRP), like currency amounts.
        /// </summary>
        [JsonProperty("taker_gets")]
        public TakerAmount TakerGets { get; set; }
        /// <summary>
        /// Specification of which currency the account taking the offer would pay,
        /// as an object with currency and issuer fields (omit issuer for XRP), like currency amounts.
        /// </summary>
        [JsonProperty("taker_pays")]
        public TakerAmount TakerPays { get; set; }
    }
}
