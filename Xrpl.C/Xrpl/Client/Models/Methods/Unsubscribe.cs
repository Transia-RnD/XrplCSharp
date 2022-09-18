using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Client.Models.Methods
{
    public class UnsubscribeRequest : RippleRequest
    {
        public UnsubscribeRequest()
        {
            Command = "unsubscribe";
        }
    }
}