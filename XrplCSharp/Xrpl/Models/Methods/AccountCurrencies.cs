using Newtonsoft.Json;

using System.Collections.Generic;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountCurrencies.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The expected response from an <see cref="AccountCurrenciesRequest"/>.
    /// </summary>
    public class AccountCurrencies //todo rename to AccountCurrenciesResponse
    {
        /// <summary>
        /// The identifying hash of the ledger version used to retrieve this data as hex.
        /// </summary>
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }
        /// <summary>
        /// The ledger index of the ledger version used to retrieve this data.
        /// </summary>
        [JsonProperty("ledger_index")]
        public int LedgerIndex { get; set; }
        /// <summary>
        /// Array of Currency Codes for currencies that this account can receive.
        /// </summary>
        [JsonProperty("receive_currencies")]
        public List<string> ReceiveCurrencies { get; set; }
        /// <summary>
        /// Array of Currency Codes for currencies that this account can send.
        /// </summary>
        [JsonProperty("send_currencies")]
        public List<string> SendCurrencies { get; set; }
        /// <summary>
        /// If true, this data comes from a validated ledger.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }

    }

    /// <summary>
    /// The `account_currencies` command retrieves a list of currencies that an account can send or receive, based on its trust lines. 
    /// Expects an <see cref="AccountCurrencies"/>
    /// </summary>
    /// <code>
    /// {
    /// 	"command": "account_currencies",
    /// 	"account": "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59",
    /// 	"strict": true,
    /// 	"ledger_index": "validated"
    /// }
    /// </code>
    public class AccountCurrenciesRequest : BaseLedgerRequest
    {
        public AccountCurrenciesRequest(string account)
        {
            Command = "account_currencies";
            Account = account;
        }
        /// <summary>
        /// A unique identifier for the account, most commonly the account's address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// If true, then the account field only accepts a public key or XRP Ledger address.<br/>
        /// Otherwise, account can be a secret or passphrase (not recommended).<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("strict")]
        public bool? Strict { get; set; }
    }
}
