namespace Xrpl.Client.Models.Methods
{
    public class LedgerClosedRequest : RippleRequest
    {
        public LedgerClosedRequest()
        {
            Command = "ledger_closed";
        }
    }
}
