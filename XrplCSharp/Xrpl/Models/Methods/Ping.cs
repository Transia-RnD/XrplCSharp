using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Models.Methods
{
    public class PingRequest : RippleRequest
    {
        public PingRequest()
        {
            Command = "ping";
        }
    }
}
