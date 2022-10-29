using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Models.Methods;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/pathFind.ts
//https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/subscribe.ts#L382
namespace Xrpl.Models.Subscriptions;

public class PathFindStream : BaseStream //todo rename to PathFindResponse extends BaseResponse
{
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
    public Guid? Id { get; set; }
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
/// <summary>
/// object with suggested paths to take, as described below.
/// </summary>
public class AlternativePath //todo rename to PathOption 
{
    /// <summary>
    /// Array of arrays of objects defining payment paths.
    /// </summary>
    [JsonProperty("alternatives")]
    public List<Path> PathsComputed { get; set; }
    /// <summary>
    /// Currency Amount that the source would have to send along this path for the.<br/>
    /// Destination to receive the desired amount.
    /// </summary>
    [JsonProperty("source_amount")]
    public decimal SourceAmount { get; set; }
}