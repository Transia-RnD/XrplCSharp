namespace Xrpl.Client.Models.Methods
{
    public class LedgerCurrentRequest : RippleRequest
    {
        public LedgerCurrentRequest()
        {
            Command = "ledger_current";
        }
    }
}
