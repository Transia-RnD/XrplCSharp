using System;
using Newtonsoft.Json;

namespace Xrpl.Models.Methods
{
    public class RippleRequest
    {
        public RippleRequest()
        {
            Id = Guid.NewGuid();
        }

        public RippleRequest(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// request id
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// request command type
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; set; }
    }
}
