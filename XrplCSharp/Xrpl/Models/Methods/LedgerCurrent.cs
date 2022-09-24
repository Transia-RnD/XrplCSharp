namespace Xrpl.Models.Methods
{
    public class LedgerCurrentRequest : RippleRequest
    {
        public LedgerCurrentRequest()
        {
            Command = "ledger_current";
        }
    }
}
