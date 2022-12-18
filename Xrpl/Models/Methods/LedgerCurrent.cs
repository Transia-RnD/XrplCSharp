using Xrpl.Models.Ledger;
//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerCurrent.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The ledger_current method returns the unique identifiers of the current  in-progress ledger.<br/>
    /// Expects a response in the form of a <see cref="LOLedgerCurrentIndex"/>
    /// </summary>
    /// <code>
    /// ```ts  const ledgerCurrent: LedgerCurrentRequest = {
    ///     "command": "ledger_current"
    /// }  ```.
    /// </code>
    public class LedgerCurrentRequest : BaseRequest
    {
        public LedgerCurrentRequest()
        {
            Command = "ledger_current";
        }
    }
}
