namespace RippleDotNet.Requests.Ledger
{
    public class ClosedLedgerRequest : RippleRequest
    {
        public ClosedLedgerRequest()
        {
            Command = "ledger_closed";
        }
    }
}
