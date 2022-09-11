using System.Collections.Generic;
using Newtonsoft.Json;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

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