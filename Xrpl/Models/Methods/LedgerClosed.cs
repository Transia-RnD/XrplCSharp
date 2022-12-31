using Xrpl.Models.Ledger;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerClosed.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The ledger_closed method returns the unique identifiers of the most recently  closed ledger.<br/>
    /// Expects a response in the form of a <see cref="LOBaseLedger"/>.
    /// </summary>
    /// <code>
    /// ```ts  const ledgerCurrent: LedgerCurrentRequest = {
    ///     "command": "ledger_current"
    /// }  ```.
    /// </code>
    public class LedgerClosedRequest : BaseRequest
    {
        public LedgerClosedRequest()
        {
            Command = "ledger_closed";
        }
    }
}
