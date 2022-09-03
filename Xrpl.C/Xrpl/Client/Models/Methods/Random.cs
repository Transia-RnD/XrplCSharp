using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Client.Models.Methods
{
    public class RandomRequest : RippleRequest
    {
        public RandomRequest()
        {
            Command = "random";
        }
    }
}
