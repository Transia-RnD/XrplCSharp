using Newtonsoft.Json;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

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