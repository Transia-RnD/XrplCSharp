using Newtonsoft.Json;
//https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/subscribe.ts#L366
namespace Xrpl.Models.Subscriptions;

/// <summary>
/// The consensus stream sends consensusPhase messages when the consensus process changes phase.<br/>
/// The message contains the new phase of consensus the server is in.
/// <see href="https://xrpl.org/subscribe.html#consensus-stream"/>
/// </summary>
public class ConsensusStreamResponseResult
{
    /// <summary>
    /// consensusPhase indicates this is from the consensus stream<br/>
    /// consensusPhase - type
    /// </summary>
    [JsonProperty("type")]
    public ResponseStreamType Type { get; set; }
    /// <summary>
    /// The new consensus phase the server is in. Possible values are open, establish, and accepted.
    /// </summary>
    [JsonProperty("consensus")]
    public string Consensus { get; set; }

}