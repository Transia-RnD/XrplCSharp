using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class EnableAmendmentTransaction : TransactionCommon, IEnableAmendmentTransaction
    {
        public EnableAmendmentTransaction()
        {
            TransactionType = TransactionType.EnableAmendment;
        }

        public string Amendment { get; set; }

        public uint LedgerSequence { get; set; }
    }
}
