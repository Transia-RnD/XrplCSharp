using System;
using Newtonsoft.Json;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/baseMethod.ts

namespace Xrpl.Models.Methods
{

    public class RippleRequest //todo rename to BaseRequest
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
        /// A unique value to identify this request.<br/>
        /// The response to this request uses the same id field.<br/>
        /// This way, even if responses arrive out of order, you know which request prompted which response.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }
        /** The name of the API method. */
        [JsonProperty("command")]
        public string Command { get; set; }

        //todo not found field  api_version?: number
    }
}
