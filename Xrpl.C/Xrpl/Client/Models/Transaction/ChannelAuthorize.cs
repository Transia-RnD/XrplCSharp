using Newtonsoft.Json;

namespace Xrpl.Client.Models.Transactions
{
    public class ChannelAuthorize
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
