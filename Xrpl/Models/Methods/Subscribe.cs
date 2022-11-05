using Newtonsoft.Json;
using System.Collections.Generic;
using System.Transactions;
using Xrpl.Models.Subscriptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/subscribe.ts

namespace Xrpl.Models.Methods
{
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

    /// <summary>
    /// Defines the order book for monitoring updates
    /// </summary>
    public class Book
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
        public bool? Snapshot { get; set; }
        /// <summary>
        /// (Optional) If true, return both sides of the order book. The default is false.
        /// </summary>
        [JsonProperty("both")]
        public bool? Both { get; set; }
    }

    /// <summary>
    /// The subscribe method requests periodic notifications from the server when certain events happen.<br/>
    /// https://xrpl.org/subscribe.html
    /// </summary>
    public class SubscribeRequest : BaseRequest
    {
        public SubscribeRequest()
        {
            Command = "subscribe";
        }
        /// <summary>
        /// Array with the unique addresses of accounts to monitor for validated
        /// transactions.The addresses must be in the XRP Ledger's base58 format.
        /// The server sends a notification for any transaction that affects at least
        /// one of these accounts.
        /// </summary>
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
        public List<Book> Books { get; set; }

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
    /// The ledger stream only sends ledgerClosed messages when the consensus process declares a new validated ledger.<br/>
    /// The message identifies the ledger and provides some information about its contents.
    /// <see href="https://xrpl.org/subscribe.html#ledger-stream"/>
    /// </summary>
    public class LedgerStream
    {
        /// <summary>
        /// ledgerClosed indicates this is from the ledger stream
        /// </summary>
        [JsonProperty("type")]
        public ResponseStreamType Type = ResponseStreamType.ledgerClosed;
        /// <summary>
        /// The reference transaction cost as of this ledger version, in drops of XRP.<br/>
        /// If this ledger version includes a SetFee pseudo-transaction the new transaction cost applies starting with the following ledger version.
        /// </summary>
        [JsonProperty("fee_base")]
        public uint FeeBase { get; set; }
        /// <summary>
        /// The reference transaction cost in "fee units".
        /// </summary>
        [JsonProperty("fee_ref")]
        public uint FeeRef { get; set; }
        /// <summary>
        /// The identifying hash of the ledger version that was closed.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger that was closed.
        /// </summary>
        [JsonProperty("ledger_index")]
        public ulong LedgerIndex { get; set; }
        /// <summary>
        /// The time this ledger was closed, in seconds since the Ripple Epoch
        /// </summary>
        [JsonProperty("ledger_time")]
        public ulong LedgerTime { get; set; }
        /// <summary>
        /// The minimum reserve, in drops of XRP, that is required for an account.<br/>
        /// If this ledger version includes a SetFee pseudo-transaction the new base reserve applies starting with the following ledger version.
        /// </summary>
        [JsonProperty("reserve_base")]
        public uint ReserveBase { get; set; }
        /// <summary>
        /// The owner reserve for each object an account owns in the ledger, in drops of XRP.<br/>
        /// If the ledger includes a SetFee pseudo-transaction the new owner reserve applies after this ledger.
        /// </summary>
        [JsonProperty("reserve_inc")]
        public uint ReserveInc { get; set; }
        /// <summary>
        /// Number of new transactions included in this ledger version.
        /// </summary>
        [JsonProperty("txn_count")]
        public uint TxnCount { get; set; }
        /// <summary>
        /// (May be omitted) Range of ledgers that the server has available.<br/>
        /// This may be a disjoint sequence such as 24900901-24900984,24901116-24901158.<br/>
        /// This field is not returned if the server is not connected to the network, or if it is connected but has not yet obtained a ledger from the network.
        /// </summary>
        [JsonProperty("validated_ledgers")]
        public string ValidatedLedgers { get; set; }

    }

    /// <summary>
    /// This response mirrors the LedgerStream, except it does NOT include the 'type' nor 'txn_count' fields.
    /// </summary>
    public class LedgerStreamResponse : BaseResponse
    {
        /// <summary>
        /// The reference transaction cost as of this ledger version, in drops of XRP.<br/>
        /// If this ledger version includes a SetFee pseudo-transaction the new transaction cost applies starting with the following ledger version.
        /// </summary>
        [JsonProperty("fee_base")]
        public uint FeeBase { get; set; }
        /// <summary>
        /// The reference transaction cost in "fee units".
        /// </summary>
        [JsonProperty("fee_ref")]
        public uint FeeRef { get; set; }
        /// <summary>
        /// The identifying hash of the ledger version that was closed.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger that was closed.
        /// </summary>
        [JsonProperty("ledger_index")]
        public ulong LedgerIndex { get; set; }
        /// <summary>
        /// The time this ledger was closed, in seconds since the Ripple Epoch
        /// </summary>
        [JsonProperty("ledger_time")]
        public ulong LedgerTime { get; set; }
        /// <summary>
        /// The minimum reserve, in drops of XRP, that is required for an account.<br/>
        /// If this ledger version includes a SetFee pseudo-transaction the new base reserve applies starting with the following ledger version.
        /// </summary>
        [JsonProperty("reserve_base")]
        public uint ReserveBase { get; set; }
        /// <summary>
        /// The owner reserve for each object an account owns in the ledger, in drops of XRP.<br/>
        /// If the ledger includes a SetFee pseudo-transaction the new owner reserve applies after this ledger.
        /// </summary>
        [JsonProperty("reserve_inc")]
        public uint ReserveInc { get; set; }
        /// <summary>
        /// Number of new transactions included in this ledger version.
        /// </summary>
        [JsonProperty("txn_count")]
        public uint TxnCount { get; set; }
        /// <summary>
        /// (May be omitted) Range of ledgers that the server has available.<br/>
        /// This may be a disjoint sequence such as 24900901-24900984,24901116-24901158.<br/>
        /// This field is not returned if the server is not connected to the network, or if it is connected but has not yet obtained a ledger from the network.
        /// </summary>
        [JsonProperty("validated_ledgers")]
        public string ValidatedLedgers { get; set; }

    }

    /// <summary>
    /// The validations stream sends messages whenever it receives validation messages,
    /// also called validation votes, regardless of whether or not the validation message is from a trusted validator.
    /// <see href="https://xrpl.org/subscribe.html#validations-stream"/>
    /// </summary>
    public class ValidationStream : BaseStream
    {
        /// <summary>
        /// (May be omitted) The amendments this server wants to be added to the protocol.
        /// </summary>
        [JsonProperty("amendments")]
        public List<string> Amendments { get; set; }
        /// <summary>
        /// (May be omitted) The unscaled transaction cost (reference_fee value) this server wants to set by Fee Voting
        /// </summary>
        [JsonProperty("base_fee")]
        public uint? BaseFee { get; set; }
        /// <summary>
        /// (May be omitted) An arbitrary value chosen by the server at startup.<br/>
        /// If the same validation key pair signs validations with different cookies concurrently,
        /// that usually indicates that multiple servers are incorrectly configured to use the same validation key pair.
        /// </summary>
        [JsonProperty("cookie")]
        public string Cookie { get; set; }
        /// <summary>
        /// Bit-mask of flags added to this validation message.<br/>
        /// The flag 0x80000000 indicates that the validation signature is fully-canonical.<br/>
        /// The flag 0x00000001 indicates that this is a full validation; otherwise it's a partial validation.<br/>
        /// Partial validations are not meant to vote for any particular ledger.<br/>
        /// A partial validation indicates that the validator is still online but not keeping up with consensus.
        /// </summary>
        [JsonProperty("flags")]
        public uint Flags { get; set; }
        /// <summary>
        /// If true, this is a full validation. Otherwise, this is a partial validation.<br/>
        /// Partial validations are not meant to vote for any particular ledger.<br/>
        /// A partial validation indicates that the validator is still online but not keeping up with consensus.
        /// </summary>
        [JsonProperty("full")]
        public bool Full { get; set; }
        /// <summary>
        /// The identifying hash of the ledger version that was closed.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger that was closed.
        /// </summary>
        [JsonProperty("ledger_index")]
        public ulong LedgerIndex { get; set; }
        /// <summary>
        /// (May be omitted) The local load-scaled transaction cost this validator is currently enforcing, in fee units. 
        /// </summary>
        [JsonProperty("load_fee")]
        public uint? LoadFee { get; set; }
        /// <summary>
        /// (May be omitted) The validator's master public key, if the validator is using a validator token, in the XRP Ledger's base58 format.
        /// </summary>
        [JsonProperty("master_key")]
        public string MasterKey { get; set; }
        /// <summary>
        /// May be omitted) The minimum reserve requirement (account_reserve value) this validator wants to set by Fee Voting.
        /// </summary>
        [JsonProperty("reserve_base")]
        public uint? ReserveBase { get; set; }
        /// <summary>
        /// May be omitted) The increment in the reserve requirement (owner_reserve value) this validator wants to set by Fee Voting.
        /// </summary>
        [JsonProperty("reserve_inc")]
        public uint? ReserveInc { get; set; }
        /// <summary>
        /// (May be omitted) An 64-bit integer that encodes the version number of the validating server.<br/>
        /// For example, "1745990410175512576".<br/>
        /// Only provided once every 256 ledgers. 
        /// </summary>
        [JsonProperty("server_version")]
        public string ServerVersion { get; set; }
        /// <summary>
        /// The signature that the validator used to sign its vote for this ledger.
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
        /// <summary>
        /// When this validation vote was signed, in seconds since the Ripple Epoch. 
        /// </summary>
        [JsonProperty("signing_time")]
        public ulong SigningTime { get; set; }
        /// <summary>
        /// The unique hash of the proposed ledger this validation applies to.
        /// </summary>
        [JsonProperty("validated_hash")]
        public string ValidatedHash { get; set; }
        /// <summary>
        /// The public key from the key-pair that the validator used to sign the message, in the XRP Ledger's base58 format.<br/>
        /// This identifies the validator sending the message and can also be used to verify the signature.<br/>
        /// If the validator is using a token, this is an ephemeral public key.
        /// </summary>
        [JsonProperty("validation_public_key")]
        public string ValidationPublicKey { get; set; }

    }
}
