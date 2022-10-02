//https://github.com/XRPLF/xrpl.js/blob/5fc1c795bc6fe4de34713fd8f0a3fde409378b30/packages/xrpl/src/models/methods/unsubscribe.ts

namespace Xrpl.Models.Methods;

/// <summary>
/// The unsubscribe command tells the server to stop sending messages for a particular subscription or set of subscriptions.<br/>
/// The parameters in the request are specified almost exactly like the parameters to the subscribe method,
/// except that they are used to define which subscriptions to end instead.
/// The rt_accounts and url parameters, and the rt_transactions stream name, are deprecated and may be removed without further notice.<br/>
/// The response follows the standard format, with a successful result containing no fields.<br/>
/// https://xrpl.org/unsubscribe.html
/// </summary>
public class UnsubscribeRequest : SubscribeBase
{
    public UnsubscribeRequest()
    {
        Command = "unsubscribe";
    }
}