using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Models.Transactions;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/norippleCheck.ts
namespace Xrpl.Models.Methods
{

    /// <summary>
    /// Whether the address refers to a gateway or user.<br/>
    /// Recommendations depend on the role of the account.<br/>
    /// Issuers must have Default Ripple enabled and must disable No Ripple on all trust lines.<br/>
    /// Users should have Default Ripple disabled, and should enable No Ripple on all trust lines.
    /// </summary>
    public enum RoleType
    {
        [EnumMember(Value = "gateway")]
        Gateway,
        [EnumMember(Value = "user")]
        User
    }
    /// <summary>
    /// Response expected by a  <see cref="NoRippleCheckRequest"/> .
    /// </summary>
    public class NoRippleCheck //todo rename to NoRippleCheckResponse extends BaseResponse
    {
        [JsonProperty("ledger_current_index")]
        public uint LedgerCurrentIndex { get; set; }

        /// <summary>
        /// Array of strings with human-readable descriptions of the problems.<br/>
        /// This includes up to one entry if the account's Default Ripple setting is  not as recommended, plus up to limit entries for trust lines whose no  ripple setting is not as recommended.
        /// </summary>
        [JsonProperty("problems")]
        public List<string> Problems { get; set; }

        /// <summary>
        /// If the request specified transactions as true, this is an array of JSON  objects, each of which is the JSON form of a transaction that should fix  one of the described problems.<br/>
        /// The length of this array is the same as  the problems array, and each entry is intended to fix the problem  described at the same index into that array.
        /// </summary>
        [JsonProperty("transactions")]
        public List<TransactionCommon> Transactions { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
    /// <summary>
    /// The `noripple_check` command provides a quick way to check the status of th  default ripple field for an account and the No Ripple flag of its trust  lines, compared with the recommended settings.<br/>
    /// Expects a response in the form  of an {@link NoRippleCheckResponse}.
    /// </summary>
    /// <code>
    ///  ```ts  const noRipple: NoRippleCheckRequest ={
    /// 	"id": 0,
    /// 	"command": "noripple_check",
    /// 	"account": "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59",
    /// 	"role": "gateway",
    /// 	"ledger_index": "current",
    /// 	"limit": 2,
    /// 	"transactions": true}
    /// ```
    /// </code>
    public class NoRippleCheckRequest : BaseLedgerRequest
    {
        public NoRippleCheckRequest(string account)
        {
            Account = account;
            Command = "noripple_check";
            Role = RoleType.User;
        }
        /// <summary>
        /// A unique identifier for the account, most commonly the account's address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Whether the address refers to a gateway or user.<br/>
        /// Recommendations depend on the role of the account.<br/>
        /// Issuers must have Default Ripple enabled and must disable No Ripple on all trust lines.<br/>
        /// Users should have Default Ripple disabled, and should enable No Ripple on all trust lines.
        /// </summary>
        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoleType Role { get; set; }

        /// <summary>
        /// If true, include an array of suggested transactions, as JSON objects, that you can sign and submit to fix the problems.<br/>
        /// Defaults to false.
        /// </summary>
        [JsonProperty("transactions")]
        public bool? Transactions { get; set; }

        /// <summary>
        /// The maximum number of trust line problems to include in the results.<br/>
        /// Defaults to 300.
        /// </summary>
        [JsonProperty("limit")]
        public uint? Limit { get; set; }
    }
}
