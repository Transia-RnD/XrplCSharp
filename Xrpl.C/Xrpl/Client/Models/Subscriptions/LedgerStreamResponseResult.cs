using Newtonsoft.Json;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

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