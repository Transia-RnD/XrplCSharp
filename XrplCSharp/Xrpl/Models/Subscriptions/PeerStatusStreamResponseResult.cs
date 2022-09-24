using Newtonsoft.Json;

namespace Xrpl.Models.Subscriptions;

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
    public ResponseStreamType Type { get; set; }
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