using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Xrpl.Client.Models.Transactions
{
    public class ChannelVerify
    {
        [JsonProperty("signature_verified")]
        public bool SignatureVerified { get; set; }
    }
}
