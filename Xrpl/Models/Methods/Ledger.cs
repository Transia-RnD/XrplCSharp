using Newtonsoft.Json;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledger.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Retrieve information about the public ledger.<br/>
    /// Expects a response in the form.
    /// </summary>
    /// <code>
    /// ```ts  const ledger: LedgerRequest = {
    ///     "id": 14,
    ///     "command": "ledger",
    ///     "ledger_index": "validated",
    ///     "full": false,
    ///     "accounts": false,
    ///     "transactions": false,
    ///     "expand": false,
    ///     "owner_funds": false
    /// }  ```.
    /// </code>
    public class LedgerRequest : BaseLedgerRequest
    {
        public LedgerRequest()
        {
            Command = "ledger";
        }

        /// <summary>
        /// Admin required If true, return full information on the entire ledger.<br/>
        /// Ignored if you did not specify a ledger version.<br/>
        /// Defaults to false.
        /// </summary>
        [JsonProperty("full")]
        public bool? Full { get; set; }

        /// <summary>
        /// Admin required.<br/>
        /// If true, return information on accounts in the ledger.<br/>
        /// Ignored if you did not specify a ledger version.<br/>
        /// Defaults to false.
        /// </summary>
        [JsonProperty("accounts")]
        public bool? Accounts { get; set; }
        /// <summary>
        /// If true, return information on transactions in the specified ledger version.<br/>
        /// Defaults to false.<br/>
        /// Ignored if you did not specify a ledger version.
        /// </summary>
        [JsonProperty("transactions")]
        public bool? Transactions { get; set; }
        /// <summary>
        /// Provide full JSON-formatted information for transaction/account information instead of only hashes.<br/>
        /// Defaults to false.<br/>
        /// Ignored unless you request transactions, accounts, or both.
        /// </summary>
        [JsonProperty("expand")]
        public bool? Expand { get; set; }
        /// <summary>
        /// If true, include owner_funds field in the metadata of OfferCreate transactions in the response.<br/>
        /// Defaults to false.<br/>
        /// Ignored unless transactions are included and expand is true.
        /// </summary>
        [JsonProperty("owner_funds")]
        public bool? OwnerFunds { get; set; }
        /// <summary>
        /// If true, and transactions and expand are both also true, return transaction information in binary format (hexadecimal string) instead of JSON format.
        /// </summary>
        [JsonProperty("binary")]
        public bool? Binary { get; set; }
        /// <summary>
        /// If true, and the command is requesting the current ledger, includes an array of queued transactions in the results.
        /// </summary>
        [JsonProperty("queue")]
        public bool? Queue { get; set; }
    }
}
