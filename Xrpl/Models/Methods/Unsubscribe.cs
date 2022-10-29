// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/unsubscribe.ts

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xrpl.Models.Methods;

/// <summary>
/// The unsubscribe command tells the server to stop sending messages for a particular subscription or set of subscriptions.<br/>
/// The parameters in the request are specified almost exactly like the parameters to the subscribe method,
/// except that they are used to define which subscriptions to end instead.
/// The rt_accounts and url parameters, and the rt_transactions stream name, are deprecated and may be removed without further notice.<br/>
/// The response follows the standard format, with a successful result containing no fields.<br/>
/// https://xrpl.org/unsubscribe.html
/// </summary>
public class UnsubscribeRequest : BaseRequest
{
    public UnsubscribeRequest()
    {
        Command = "unsubscribe";
    }

    /// <summary>
    /// Array with the unique addresses of accounts to monitor for validated
    /// transactions.The addresses must be in the XRP Ledger's base58 format.
    /// The server sends a notification for any transaction that affects at least
    /// one of these accounts.
    /// </summary>
    [JsonProperty("streams")]
    public List<string>? Streams { get; set; }
    /// <summary>
    /// (Optional) Array with the unique addresses of accounts to monitor for validated transactions.
    /// The addresses must be in the XRP Ledger's base58 format.
    /// The server sends a notification for any transaction that affects at least one of these accounts
    /// </summary>
    [JsonProperty("accounts")]
    public List<string>? Accounts { get; set; }
    /// <summary>
    /// (Optional) Like accounts, but include transactions that are not yet finalized.
    /// </summary>
    [JsonProperty("accounts_proposed")]
    public List<string>? AccountsProposed { get; set; }
    /// <summary>
    /// (Optional) Array of objects defining order books  to monitor for updates.
    /// </summary>
    [JsonProperty("books")]
    public List<Book>? Books { get; set; }
}