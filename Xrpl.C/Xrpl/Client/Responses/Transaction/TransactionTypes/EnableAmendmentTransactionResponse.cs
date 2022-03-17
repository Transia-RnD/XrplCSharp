using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class EnableAmendmentTransactionResponse : TransactionResponseCommon, IEnableAmendmentTransaction
    {
        public string Amendment { get; set; }
        public uint LedgerSequence { get; set; }
    }
}
