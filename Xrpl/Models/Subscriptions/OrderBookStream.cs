using Newtonsoft.Json;
using Xrpl.Models.Transaction;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/subscribe.ts

namespace Xrpl.Models.Subscriptions;

/// <summary>
/// When you subscribe to one or more order books with the books field, you get back any transactions that affect those order books.
/// <see href="https://xrpl.org/subscribe.html#order-book-streams"/>
/// </summary>
public class OrderBookStream : BaseStream
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
    /// (Validated transactions only) The transaction metadata, which shows the exact outcome of the transaction in detail.
    /// </summary>
    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
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