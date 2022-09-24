using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Xrpl.Models.Subscriptions
{
    public class BaseResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("warning")]
        public string Warning { get; set; }
        [JsonProperty("warnings")]
        public List<RippleResponseWarning> Warnings { get; set; }
        [JsonProperty("forwarded")]
        public string Forwarded { get; set; }
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }
    }  

    public class RippleResponseWarning
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("details")]
        public WarningDetails Details { get; set; }
    }
    public class WarningDetails
    {
        [JsonProperty("expected_date")]
        public uint ExpectedDate { get; set; }
        [JsonProperty("expected_date_UTC")]
        public string ExpectedDate_UTC { get; set; }

    }
}
