using System;
using Newtonsoft.Json;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/baseMethod.ts
//https://xrpl.org/error-formatting.html#error-formatting
namespace Xrpl.Models.Subscriptions;

public class ErrorResponse
{
    /// <summary>
    /// ID provided in the Web Socket request that prompted this response
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// "error" if the request caused an error
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
    /// <summary>
    /// Typically "response", which indicates a successful response to a command.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; }
    /// <summary>
    /// A unique code for the type of error that occurred
    /// </summary>
    [JsonProperty("error")]
    public string Error { get; set; }
    [JsonProperty("error_code")]
    public string ErrorCode { get; set; }
    [JsonProperty("error_message")]
    public string ErrorMessage { get; set; }
    /// <summary>
    /// A copy of the request that prompted this error, in JSON format.<br/>
    /// Caution: If the request contained any secrets, they are copied here!
    /// </summary>
    [JsonProperty("request")]
    public object Request { get; set; }

    /// <summary>
    /// (May be omitted) The api_version specified in the request, if any.
    /// </summary>

    [JsonProperty("api_version")]
    public string ApiVersion { get; set; }

}