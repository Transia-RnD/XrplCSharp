using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Xrpl.Client.Models.Transactions
{
    public class RippleResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }
    /// <summary>
    /// The ledger stream only sends ledgerClosed messages when the consensus process declares a new validated ledger.<br/>
    /// The message identifies the ledger and provides some information about its contents.
    /// <see href="https://xrpl.org/subscribe.html#ledger-stream"/>
    /// </summary>
    public class LedgerStreamResponseResult
    {
        /// <summary>
        /// ledgerClosed indicates this is from the ledger stream
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
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
        public uint? ValidatedLedgers { get; set; }

    }
    public class BaseStreamResponseResult
    {
        /// <summary>
        /// String Transaction result code
        /// </summary>
        [JsonProperty("engine_result")]
        public string EngineResult { get; set; }
        /// <summary>
        /// Numeric transaction response code, if applicable.
        /// </summary>
        [JsonProperty("engine_result_code")]
        public int EngineResultCode { get; set; }
        /// <summary>
        /// Human-readable explanation for the transaction response
        /// </summary>
        [JsonProperty("engine_result_message")]
        public string EngineResultMessage { get; set; }
        /// <summary>
        /// (Validated transactions only) The identifying hash of the ledger version that includes this transaction
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// (Validated transactions only) The ledger index of the ledger version that includes this transaction.
        /// </summary>
        [JsonProperty("ledger_index")]
        public ulong? LedgerIndex { get; set; }

    }

    /// <summary>
    /// The validations stream sends messages whenever it receives validation messages,
    /// also called validation votes, regardless of whether or not the validation message is from a trusted validator.
    /// <see href="https://xrpl.org/subscribe.html#validations-stream"/>
    /// </summary>
    public class ValidationsStreamResponseResult
    {
        /// <summary>
        /// The value validationReceived indicates this is from the validations stream.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
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


    /// <summary>
    /// Many subscriptions result in messages about transactions, including the following:
    /// The transactions stream <br/>
    /// The transactions_proposed stream<br/>
    /// accounts subscriptions<br/>
    /// accounts_proposed subscriptions<br/>
    /// book (Order Book) subscriptions
    /// <see href="https://xrpl.org/subscribe.html#transaction-streams"/>
    /// </summary>
    public class TransactionStreamResponseResult : BaseStreamResponseResult
    {
        /// <summary>
        /// transaction indicates this is the notification of a transaction, which could come from several possible streams.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// (Unvalidated transactions only) The ledger index of the current in-progress ledger version for which this transaction is currently proposed.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// (Validated transactions only) The transaction metadata, which shows the exact outcome of the transaction in detail.
        /// </summary>
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        /// <summary>
        /// The definition of the transaction in JSON format
        /// </summary>
        [JsonProperty("transaction")]
        public dynamic TransactionJson { get; set; }

        [JsonIgnore]
        public ITransactionResponseCommon Transaction => JsonConvert.DeserializeObject<TransactionResponseCommon>(TransactionJson.ToString());

        /// <summary>
        /// If true, this transaction is included in a validated ledger and its outcome is final.<br/>
        /// Responses from the transaction stream should always be validated.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }

    }
    /// <summary>
    /// The admin-only peer_status stream reports a large amount of information on the activities of other rippled servers to which this server is connected,
    /// in particular their status in the consensus process.
    /// <see href="https://xrpl.org/subscribe.html#peer-status-stream"/>
    /// </summary>
    public class PeerStatusStreamResponseResult
    {
        /// <summary>
        /// peerStatusChange indicates this comes from the Peer Status stream.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The type of event that prompted this message. See Peer Status Events for possible values.<br/>
        /// possible values:<br/>
        /// CLOSING_LEDGER - The peer closed a ledger version with this Ledger Index, which usually means it is about to start consensus.<br/>
        /// ACCEPTED_LEDGER - The peer built this ledger version as the result of a consensus round. Note: This ledger is still not certain to become immutably validated.<br/>
        /// SWITCHED_LEDGER - The peer concluded it was not following the rest of the network and switched to a different ledger version.<br/>
        /// LOST_SYNC - The peer fell behind the rest of the network in tracking which ledger versions are validated and which are undergoing consensus.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }
        /// <summary>
        /// The time this event occurred, in seconds since the Ripple Epoch.
        /// </summary>
        [JsonProperty("date")]
        public ulong Date { get; set; }
        /// <summary>
        /// (May be omitted) The identifying Hash of a ledger version to which this message pertains.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// (May be omitted) The Ledger Index of a ledger version to which this message pertains.
        /// </summary>
        [JsonProperty("ledger_index")]
        public ulong? LedgerIndex { get; set; }
        /// <summary>
        /// (May be omitted) The largest Ledger Index the peer has currently available.
        /// </summary>
        [JsonProperty("ledger_index_max")]
        public ulong? ledger_index_max { get; set; }
        /// <summary>
        /// (May be omitted) The smallest Ledger Index the peer has currently available.
        /// </summary>
        [JsonProperty("ledger_index_min")]
        public ulong? ledger_index_min { get; set; }

    }
    /// <summary>
    /// When you subscribe to one or more order books with the books field, you get back any transactions that affect those order books.
    /// <see href="https://xrpl.org/subscribe.html#order-book-streams"/>
    /// </summary>
    public class OrderBookStreamResponseResult : BaseStreamResponseResult
    {   
        /// <summary>
        /// peerStatusChange indicates this comes from the Peer Status stream.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// (Validated transactions only) The transaction metadata, which shows the exact outcome of the transaction in detail.
        /// </summary>
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// The definition of the transaction in JSON format
        /// </summary>
        [JsonProperty("transaction")]
        public dynamic TransactionJson { get; set; }

        [JsonIgnore]
        public ITransactionResponseCommon Transaction => JsonConvert.DeserializeObject<TransactionResponseCommon>(TransactionJson.ToString());
        /// <summary>
        /// If true, this transaction is included in a validated ledger and its outcome is final.<br/>
        /// Responses from the transaction stream should always be validated.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
    /// <summary>
    /// The consensus stream sends consensusPhase messages when the consensus process changes phase.<br/>
    /// The message contains the new phase of consensus the server is in.
    /// <see href="https://xrpl.org/subscribe.html#consensus-stream"/>
    /// </summary>
    public class ConsensusStreamResponseResult
    {
        /// <summary>
        /// consensusPhase indicates this is from the consensus stream
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The new consensus phase the server is in. Possible values are open, establish, and accepted.
        /// </summary>
        [JsonProperty("consensus")]
        public string Consensus { get; set; }

    }
}
