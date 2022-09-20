using System;
using Newtonsoft.Json;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/common/index.ts

namespace Xrpl.Client.Models.Common
{
    public class Common
    {

        public class XRP
        {
            [JsonProperty("currency")]
            public string Currency = "XRP";
        }

        public class IssuedCurrency
        {
            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("issuer")]
            public string Issuer { get; set; }
        }

        public class IssuedCurrencyAmount: IssuedCurrency
        {
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}

