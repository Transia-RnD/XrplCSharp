using Newtonsoft.Json;

namespace RippleDotNet.Model.Transaction
{
    public class ChannelAuthorize
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
