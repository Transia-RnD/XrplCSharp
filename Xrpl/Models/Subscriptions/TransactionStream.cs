using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Models.Transaction;

//https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/subscribe.ts#L253
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
public class TransactionStream : BaseStream
{
    /// <summary>
    /// String Transaction result code
    /// </summary>
    [JsonProperty("engine_result")]
    public string EngineResult { get; set; }
    /// <summary>
    /// Numeric transaction response code, if applicable.
    /// </summary>
    [JsonProperty("engine_result_code")]
    public int EngineResultCode { get; set; }
    /// <summary>
    /// Human-readable explanation for the transaction response
    /// </summary>
    [JsonProperty("engine_result_message")]
    public string EngineResultMessage { get; set; }

    /// <summary>
    /// (Validated transactions only) The identifying hash of the ledger version that includes this transaction
    /// </summary>
    [JsonProperty("ledger_hash")]
    public string LedgerHash { get; set; }
    /// <summary>
    /// (Validated transactions only) The ledger index of the ledger version that includes this transaction.
    /// </summary>
    [JsonProperty("ledger_index")]
    public ulong? LedgerIndex { get; set; }
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

    /// <summary>
    /// May be omitted) If this field is provided, it contains one or more Warnings Objects with important warnings.<br/>
    /// For details, see API Warnings (https://xrpl.org/response-formatting.html#api-warnings)
    /// </summary>
    [JsonProperty("warnings")]
    public List<RippleResponseWarning> Warnings { get; set; }
}