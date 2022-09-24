using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Models.Methods
{
    public class UnsubscribeRequest : RippleRequest
    {
        public UnsubscribeRequest()
        {
            Command = "unsubscribe";
        }
    }
}