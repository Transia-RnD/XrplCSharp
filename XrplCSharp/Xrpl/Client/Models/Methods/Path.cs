using Newtonsoft.Json;

namespace Xrpl.Client.Models.Methods
{
    public class Path
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("currency")]
        public string CurrencyCode { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }
    }
}
