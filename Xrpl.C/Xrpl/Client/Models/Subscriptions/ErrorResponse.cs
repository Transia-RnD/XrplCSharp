using System;
using Newtonsoft.Json;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

public class ErrorResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }


    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("error_code")]
    public string ErrorCode { get; set; }
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; }
    [JsonProperty("request")]
    public object Request { get; set; }



    [JsonProperty("api_version")]
    public string ApiVersion { get; set; }

}