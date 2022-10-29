using Newtonsoft.Json;

using System;
using System.Collections.Generic;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/baseMethod.ts
//https://xrpl.org/response-formatting.html

namespace Xrpl.Models.Subscriptions
{
    public class BaseResponse
    {
        /// <summary>
        /// (WebSocket only) ID provided in the request that prompted this response
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// (WebSocket only) The value response indicates a direct response to an API request.<br/>
        /// Asynchronous notifications use a different value such as ledgerClosed or transaction.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The result of the query; contents vary depending on the command.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
        /// <summary>
        /// (WebSocket only) The value success indicates the request was successfully received and understood by the server.<br/>
        /// Some client libraries omit this field on success.
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// (WebSocket only) The value success indicates the request was successfully received and understood by the server.<br/>
        /// Some client libraries omit this field on success.
        /// </summary>
        [JsonProperty("result")]
        public object Result { get; set; }
        /// <summary>
        /// (May be omitted) If this field is provided, the value is the string load.<br/>
        /// This means the client is approaching the rate limiting threshold where the server will disconnect this client.
        /// </summary>
        [JsonProperty("warning")]
        public string Warning { get; set; }
        /// <summary>
        /// May be omitted) If this field is provided, it contains one or more Warnings Objects with important warnings.<br/>
        /// For details, see API Warnings (https://xrpl.org/response-formatting.html#api-warnings)
        /// </summary>
        [JsonProperty("warnings")]
        public List<RippleResponseWarning> Warnings { get; set; }
        /// <summary>
        /// (May be omitted) If true, this request and response have been forwarded from a Reporting Mode
        /// server to a P2P Mode server (and back) because the request requires data that is not available in Reporting Mode.<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("forwarded")]
        public string Forwarded { get; set; }
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }
    }
    /// <summary>
    /// When the response contains a warnings array, each member of the array represents a separate warning from the server.
    /// </summary>
    public class RippleResponseWarning //todo rename to Waning
    {
        /// <summary>
        /// A unique numeric code for this warning message.
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }
        /// <summary>
        /// A human-readable string describing the cause of this message.<br/>
        /// Do not write software that relies the contents of this message;<br/>
        /// use the id (and details, if applicable) to identify the warning instead.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// (May be omitted) Additional information about this warning.<br/>
        /// The contents vary depending on the type of warning.
        /// </summary>
        [JsonProperty("details")]
        public WarningDetails Details { get; set; }
    }
    /// <summary>
    /// This warning indicates that the one or more amendments to the XRP Ledger protocol are scheduled to become enabled,
    /// but the current server does not have an implementation for those amendments.<br/>
    /// If those amendments become enabled, the current server will become amendment blocked, so you should upgrade to the latest rippled version as soon as possible.
    /// </summary>
    public class WarningDetails
    {
        /// <summary>
        /// The time that the first unsupported amendment is expected to become enabled, in seconds since the Ripple Epoch.
        /// </summary>
        [JsonProperty("expected_date")]
        public uint ExpectedDate { get; set; }
        /// <summary>
        /// The timestamp, in UTC, when the first unsupported amendment is expected to become enabled
        /// </summary>
        [JsonProperty("expected_date_UTC")]
        public string ExpectedDate_UTC { get; set; }

    }
}
