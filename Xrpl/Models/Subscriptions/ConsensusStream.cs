using Newtonsoft.Json;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/subscribe.ts

namespace Xrpl.Models.Subscriptions
{
    /// <summary>
    /// The consensus stream sends consensusPhase messages when the consensus process changes phase.<br/>
    /// The message contains the new phase of consensus the server is in.
    /// <see href="https://xrpl.org/subscribe.html#consensus-stream"/>
    /// </summary>
    public class ConsensusStream : BaseStream
    {
        /// <summary>
        /// The new consensus phase the server is in. Possible values are open, establish, and accepted.
        /// </summary>
        [JsonProperty("consensus")]
        public string Consensus { get; set; }

    }
}