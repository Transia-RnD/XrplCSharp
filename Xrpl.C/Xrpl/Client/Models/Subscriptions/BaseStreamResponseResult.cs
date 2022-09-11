using Newtonsoft.Json;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

public class BaseStreamResponseResult
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

}