using Newtonsoft.Json;

namespace Xrpl.Client.Responses.Transaction
{
    public class ChannelAuthorize
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
