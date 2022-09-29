using Newtonsoft.Json;

using System.Collections.Generic;

using Xrpl.Models.Ledger;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountObjects.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an <see cref="AccountObjectsRequest"/>.
    /// </summary>
    public class AccountObjects //todo rename to response
    {
        /// <summary>
        /// Unique Address of the account this request corresponds to.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Array of objects owned by this account.<br/>
        /// Each object is in its raw  ledger format.
        /// </summary>
        [JsonProperty("account_objects")]
        public List<BaseLedgerEntry> AccountObjectList { get; set; } //todo change from class to interface and parse same as transactionResponse
        /// <summary>
        /// The identifying hash of the ledger that was used to generate this  response.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger version that was used to generate this  response.
        /// </summary>
        [JsonProperty("ledger_index")]
        public uint? LedgerIndex { get; set; }
        /// <summary>
        /// The ledger index of the current in-progress ledger version, which was  used to generate this response.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public uint? LedgerCurrentIndex { get; set; }
        /// <summary>
        /// The limit that was used in this request, if any.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
        /// <summary>
        /// Server-defined value indicating the response is paginated.<br/>
        /// Pass this to  the next call to resume where this call left off.<br/>
        /// Omitted when there are  no additional pages after this one.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }
        /// <summary>
        /// If included and set to true, the information in this response comes from  a validated ledger version.<br/>
        /// Otherwise, the information is subject to  change.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
    /// <summary>
    /// The account_objects command returns the raw ledger format for all objects  owned by an account.<br/>
    /// For a higher-level view of an account's trust lines and  balances, see the account_lines method instead.<br/>
    /// Expects a response in the  form of an <see cref="AccountObjects"/>.
    /// </summary>
    public class AccountObjectsRequest : BaseLedgerRequest
    {
        public AccountObjectsRequest(string account)
        {
            Account = account;
            Command = "account_objects";
        }
        /// <summary>
        /// A unique identifier for the account, most commonly the account's address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// If included, filter results to include only this type of ledger object.<br/>
        /// The valid types are: Check , DepositPreauth, Escrow, Offer, PayChannel, SignerList, Ticket, and RippleState (trust line).
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; } //todo add nullable enum
        /// <summary>
        /// The maximum number of objects to include in the results.<br/>
        /// Must be within the inclusive range 10 to 400 on non-admin connections.<br/>
        /// The default is 200.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }
        /// <summary>
        /// Value from a previous paginated response.<br/>
        /// Resume retrieving data where that response left off.
        /// </summary>
        [JsonProperty("marker")]
        public object Marker { get; set; }

        //todo not found field deletion_blockers_only?: boolean
    }
}
