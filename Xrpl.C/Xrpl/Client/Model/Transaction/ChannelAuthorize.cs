using Newtonsoft.Json;

namespace Xrpl.Client.Model.Transaction
{
    public class ChannelAuthorize
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
