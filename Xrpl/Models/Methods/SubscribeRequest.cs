using Newtonsoft.Json;
using System.Collections.Generic;

//https://github.com/XRPLF/xrpl.js/blob/5fc1c795bc6fe4de34713fd8f0a3fde409378b30/packages/xrpl/src/models/methods/subscribe.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The subscribe method requests periodic notifications from the server when certain events happen.<br/>
    /// https://xrpl.org/subscribe.html
    /// </summary>
    public class SubscribeRequest : SubscribeBase
    {
        public SubscribeRequest()
        {
            Command = "subscribe";
        }
        /// <summary>
        /// (Optional for Websocket; Required otherwise) URL where the server sends a JSON-RPC callbacks for each event.
        /// Admin-only.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// (Optional) Username to provide for basic authentication at the callback URL.
        /// </summary>
        [JsonProperty("url_username")]
        public string UrlUsername { get; set; }
        /// <summary>
        /// (Optional) Password to provide for basic authentication at the callback URL.
        /// </summary>
        [JsonProperty("url_password")]
        public string UrlPassword { get; set; }

    }

    /// <summary>
    /// base subscribe / unsubscribe request fields
    /// </summary>
    public class SubscribeBase : RippleRequest
    {
        /// <summary>
        /// (Optional) Array of string names of generic streams to subscribe to, as explained below
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <item>
        /// <term>consensus</term>
        /// <description>
        /// Sends a message whenever the server changes phase in the consensus cycle (open, establish, accepted, and so forth)
        /// </description>
        /// </item>
        /// <item>
        /// <term>ledger</term>
        /// <description>
        /// Sends a message whenever the consensus process declares a new validated ledger
        /// </description>
        /// </item>
        /// <item>
        /// <term>manifests</term>
        /// <description>
        /// Sends a message whenever the server receives an update to a validator's ephemeral signing key.
        /// </description>
        /// </item>
        /// <item>
        /// <term>peer_status</term>
        /// <description>
        /// (Admin only) Information about connected peer rippled servers, especially with regards to the consensus process.
        /// </description>
        /// </item>
        /// <item>
        /// <term>transactions</term>
        /// <description>
        /// Sends a message whenever a transaction is included in a closed ledger
        /// </description>
        /// </item>
        /// <item>
        /// <term>transactions_proposed</term>
        /// <description>
        /// Sends a message whenever a transaction is included in a closed ledger,
        /// as well as some transactions that have not yet been included in a validated ledger and may never be.
        /// Not all proposed transactions appear before validation.
        /// Note: Even some transactions that don't succeed are included in validated ledgers, because they take the anti-spam transaction fee.
        /// </description>
        /// </item>
        /// <item>
        /// <term>server</term>
        /// <description>
        /// Sends a message whenever the status of the rippled server (for example, network connectivity) changes
        /// </description>
        /// </item>
        /// <item>
        /// <term>validations</term>
        /// <description>
        /// Sends a message whenever the server receives a validation message, regardless of if the server trusts the validator.
        /// (An individual rippled declares a ledger validated when the server receives validation messages from at least a quorum of trusted validators.)
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        [JsonProperty("streams")]
        public List<string> Streams { get; set; }

        /// <summary>
        /// (Optional) Array with the unique addresses of accounts to monitor for validated transactions.
        /// The addresses must be in the XRP Ledger's base58 format.
        /// The server sends a notification for any transaction that affects at least one of these accounts
        /// </summary>
        [JsonProperty("accounts")]
        public List<string> Accounts { get; set; }
        /// <summary>
        /// (Optional) Like accounts, but include transactions that are not yet finalized.
        /// </summary>
        [JsonProperty("accounts_proposed")]
        public List<string> AccountsProposed { get; set; }
        /// <summary>
        /// (Optional) Array of objects defining order books  to monitor for updates.
        /// </summary>
        [JsonProperty("books")]
        public List<SubscribeBook> Books { get; set; }

    }
    /// <summary>
    /// Defines the order book for monitoring updates
    /// </summary>
    public class SubscribeBook
    {
        /// <summary>
        /// Specification of which currency the account taking the Offer would receive, as a currency object with no amount.
        /// </summary>
        [JsonProperty("taker_gets")]
        public BookCurrency TakerGets { get; set; }
        /// <summary>
        /// Specification of which currency the account taking the Offer would pay, as a currency object with no amount.
        /// </summary>
        [JsonProperty("taker_pays")]
        public BookCurrency TakerPays { get; set; }
        /// <summary>
        /// Unique account address to use as a perspective for viewing offers, in the XRP Ledger's base58 format.
        /// (This affects the funding status and fees of Offers.)
        /// </summary>
        [JsonProperty("taker")]
        public string Taker { get; set; }
        /// <summary>
        /// (Optional) If true, return the current state of the order book once when you subscribe before sending updates.
        /// The default is false.
        /// </summary>
        [JsonProperty("snapshot")]
        public bool Snapshot { get; set; }
        /// <summary>
        /// (Optional) If true, return both sides of the order book. The default is false.
        /// </summary>
        [JsonProperty("both")]
        public bool Both { get; set; }
    }
    /// <summary>
    /// Order book currency
    /// </summary>
    public class BookCurrency
    {
        /// <summary>
        /// Currency code
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
        /// <summary>
        /// Currency Issuer
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
}
