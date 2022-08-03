namespace Xrpl.Client.Models.Methods
{
    public class ClosedLedgerRequest : RippleRequest
    {
        public ClosedLedgerRequest()
        {
            Command = "ledger_closed";
        }
    }
}
