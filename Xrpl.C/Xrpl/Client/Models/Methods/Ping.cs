using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Client.Models.Methods
{
    public class PingRequest : RippleRequest
    {
        public PingRequest()
        {
            Command = "ping";
        }
    }
}
