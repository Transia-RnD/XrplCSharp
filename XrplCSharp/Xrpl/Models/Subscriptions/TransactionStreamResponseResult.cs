using Newtonsoft.Json;
using Xrpl.Models.Transactions;

namespace Xrpl.Models.Subscriptions;

/// <summary>
/// Many subscriptions result in messages about transactions, including the following:
/// The transactions stream <br/>
/// The transactions_proposed stream<br/>
/// accounts subscriptions<br/>
/// accounts_proposed subscriptions<br/>
/// book (Order Book) subscriptions
/// <see href="https://xrpl.org/subscribe.html#transaction-streams"/>
/// </summary>
public class TransactionStreamResponseResult : BaseStreamResponseResult
{
    /// <summary>
    /// transaction indicates this is the notification of a transaction, which could come from several possible streams.
    /// </summary>
    [JsonProperty("type")]
    public ResponseStreamType Type { get; set; }
    /// <summary>
    /// (Unvalidated transactions only) The ledger index of the current in-progress ledger version for which this transaction is currently proposed.
    /// </summary>
    [JsonProperty("ledger_current_index")]
    public uint? LedgerCurrentIndex { get; set; }
    /// <summary>
    /// (Validated transactions only) The transaction metadata, which shows the exact outcome of the transaction in detail.
    /// </summary>
    [JsonProperty("meta")]
    public Meta Meta { get; set; }
    /// <summary>
    /// The definition of the transaction in JSON format
    /// </summary>
    [JsonProperty("transaction")]
    public dynamic TransactionJson { get; set; }

    [JsonIgnore]
    public ITransactionResponseCommon Transaction => JsonConvert.DeserializeObject<TransactionResponseCommon>(TransactionJson.ToString());

    /// <summary>
    /// If true, this transaction is included in a validated ledger and its outcome is final.<br/>
    /// Responses from the transaction stream should always be validated.
    /// </summary>
    [JsonProperty("validated")]
    public bool Validated { get; set; }

}