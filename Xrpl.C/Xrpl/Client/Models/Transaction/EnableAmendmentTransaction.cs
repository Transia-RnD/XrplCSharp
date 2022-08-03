using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
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

    public interface IEnableAmendmentTransaction : ITransactionCommon
    {
        string Amendment { get; set; }
        uint LedgerSequence { get; set; }
    }

    public class EnableAmendmentTransactionResponse : TransactionResponseCommon, IEnableAmendmentTransaction
    {
        public string Amendment { get; set; }
        public uint LedgerSequence { get; set; }
    }
}
