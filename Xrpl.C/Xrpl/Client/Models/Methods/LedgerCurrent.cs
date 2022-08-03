namespace Xrpl.Client.Models.Methods
{
    public class CurrentLedgerRequest : RippleRequest
    {
        public CurrentLedgerRequest()
        {
            Command = "ledger_current";
        }
    }
}
