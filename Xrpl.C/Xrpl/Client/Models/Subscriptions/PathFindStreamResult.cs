using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Models.Methods;

namespace xrpl_c.Xrpl.Client.Models.Subscriptions;

public class PathFindStreamResult
{
    /// <summary>
    /// result type (path_find)
    /// </summary>
    [JsonProperty("type")]
    public ResponseStreamType Type { get; set; }
    /// <summary>
    /// Unique address that would send a transaction.
    /// </summary>
    [JsonProperty("source_account")]
    public string SourceAccount { get; set; }
    /// <summary>
    /// Unique address of the account that would receive a transaction.
    /// </summary>
    [JsonProperty("destination_account")]
    public string DestinationAccount { get; set; }
    /// <summary>
    /// Currency Amount that the destination would receive in a transaction. 
    /// </summary>
    [JsonProperty("destination_amount")]
    public decimal DestinationAmount { get; set; }
    /// <summary>
    /// If false, this is the result of an incomplete search. A later reply may have a better path.If true, then this is the best path found.<br/>
    /// (It is still theoretically possible that a better path could exist, but rippled won't find it.)<br/>
    /// Until you close the pathfinding request, rippled continues to send updates each time a new ledger closes.<br/>
    /// </summary>
    [JsonProperty("full_reply")]
    public bool FullReply { get; set; }
    /// <summary>
    /// The ID provided in the WebSocket request is included again at this level.
    /// </summary>
    [JsonProperty("Id")]
    public Guid Id { get; set; }
    /// <summary>
    /// Currency Amount that would be spent in the transaction.
    /// </summary>
    [JsonProperty("send_max")]
    public decimal? SendMax { get; set; }
    /// <summary>
    /// Array of objects with suggested paths to take. If empty, then no paths were found connecting the source and destination accounts
    /// </summary>
    [JsonProperty("alternatives")]

    public List<AlternativePath> Alternatives { get; set; }
}

public class AlternativePath
{
    [JsonProperty("alternatives")]
    public List<Path> PathsComputed { get; set; }
    [JsonProperty("source_amount")]
    public decimal SourceAmount { get; set; }
}