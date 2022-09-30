using Newtonsoft.Json;

using System.Collections.Generic;

using Xrpl.Models.Ledger;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/accountInfo.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// Response expected from an <see cref="AccountInfoRequest"/>.
    /// </summary>
    public class AccountInfo //todo rename to AccountInfoResponse
    {
        /// <summary>
        /// The AccountRoot ledger object with this account's information, as stored in the ledger.
        /// </summary>
        [JsonProperty("account_data")]
        public LOAccountRoot AccountData { get; set; }
        /// <summary>
        /// The ledger index of the current in-progress ledger, which was used when retrieving this information.
        /// </summary>
        [JsonProperty("ledger_current_index")]
        public int LedgerCurrentIndex { get; set; }
        /// <summary>
        /// Information about queued transactions sent by this account.<br/>
        /// This information describes the state of the local rippled server, which may be different from other servers in the peer-to-peer XRP Ledger network.<br/>
        /// Some fields may be omitted because the values are calculated "lazily" by the queuing mechanism.
        /// </summary>
        [JsonProperty("queue_data")]
        public QueueData QueueData { get; set; }
        /// <summary>
        /// True if this data is from a validated ledger version;<br/>
        /// if omitted or set to false, this data is not final.
        /// </summary>
        [JsonProperty("validated")]
        public bool Validated { get; set; }

        //todo not found fields - signer_lists?: SignerList[],  ledger_index?: number,
    }

    //https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/accountInfo.ts#L42
    /// <summary>
    /// Information about each queued transaction from address.
    /// </summary>
    public class AccountTransaction //todo rename to QueueTransaction?
    {
        /// <summary>
        /// Whether this transaction changes this address's ways of authorizing transactions.
        /// </summary>
        [JsonProperty("auth_change")]
        public bool AuthChange { get; set; }
        /// <summary>
        /// The Transaction Cost of this transaction, in drops of XRP.
        /// </summary>
        [JsonProperty("fee")]
        public string Fee { get; set; }
        /// <summary>
        /// The transaction cost of this transaction, relative to the minimum cost for this type of transaction, in fee levels.
        /// </summary>
        [JsonProperty("fee_level")]
        public string FeeLevel { get; set; }
        /// <summary>
        /// The maximum amount of XRP, in drops, this transaction could send or destroy. 
        /// </summary>
        [JsonProperty("max_spend_drops")]
        public string MaxSpendDrops { get; set; }
        /// <summary>
        /// The Sequence Number of this transaction.
        /// </summary>
        [JsonProperty("seq")]
        public int Sequence { get; set; }

        public int? LastLedgerSequence { get; set; } //todo unknown field
    }

    /// <summary>
    /// Information about queued transactions sent by account.<br/>
    /// This information describes the state of the local rippled server, which may be different from other servers in the peer-to-peer XRP Ledger network.<br/>
    /// Some fields may be omitted because the values are calculated "lazily" by the queuing mechanism.
    /// </summary>
    public class QueueData
    {
        /// <summary>
        /// Whether a transaction in the queue changes this address's ways of authorizing transactions.
        /// If true, this address can queue no further transactions until that transaction has been executed or dropped from the queue.
        /// </summary>
        [JsonProperty("auth_change_queued")]
        public bool? AuthChangeQueued { get; set; }
        /// <summary>
        /// The highest Sequence Number among transactions queued by this address.
        /// </summary>
        [JsonProperty("highest_sequence")]
        public int? HighestSequence { get; set; }
        /// <summary>
        /// The lowest Sequence Number among transactions queued by this address. 
        /// </summary>
        [JsonProperty("lowest_sequence")]
        public int? LowestSequence { get; set; }
        /// <summary>
        /// Integer amount of drops of XRP that could be debited from this address
        /// if every transaction in the queue consumes the maximum amount of XRP possible.
        /// </summary>
        [JsonProperty("max_spend_drops_total")]
        public string MaxSpendDropsTotal { get; set; }
        /// <summary>
        /// Information about each queued transaction from this address.
        /// </summary>
        [JsonProperty("transactions")]
        public List<AccountTransaction> Transactions { get; set; }
        /// <summary>
        /// Number of queued transactions from this address.
        /// </summary>
        [JsonProperty("txn_count")]
        public int TxnCount { get; set; }
    }
    /// <summary>
    /// The `account_info` command retrieves information about an account, its activity, and its XRP balance.<br/>
    /// All information retrieved is relative to a particular version of the ledger. Returns an <see cref="AccountInfo"/>.
    /// </summary>
    /// <code>
    /// {
    /// 	"id": 2,
    /// 	"command": "account_info",
    /// 	"account": "rG1QQv2nh2gr7RCZ1P8YYcBUKCCN633jCn",
    /// 	"strict": true,
    /// 	"ledger_index": "current",
    /// 	"queue": true
    /// }
    /// </code>
    public class AccountInfoRequest : BaseLedgerRequest
    {
        public AccountInfoRequest(string account)
        {
            Account = account;
            Command = "account_info";
        }
        /// <summary>
        ///  A unique identifier for the account, most commonly the account's address.
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// If true, then the account field only accepts a public key or XRP Ledger address.<br/>
        /// Otherwise, account can be a secret or passphrase (not recommended).
        /// The default is false.
        /// </summary>
        [JsonProperty("strict")]
        public bool? Strict { get; set; }
        /// <summary>
        /// Whether to get info about this account's queued transactions.<br/>
        /// Can only be used when querying for the data from the current open ledger.<br/>
        /// Not available from servers in Reporting Mode.
        /// </summary>
        [JsonProperty("queue")]
        public bool? Queue { get; set; }
        /// <summary>
        /// Request SignerList objects associated with this account.
        /// </summary>
        [JsonProperty("signer_lists")]
        public bool? SignerLists { get; set; }
    }
}
