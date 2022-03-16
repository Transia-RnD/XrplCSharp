using Newtonsoft.Json;

namespace RippleDotNet.Responses.Transaction
{
    public class ChannelAuthorize
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
