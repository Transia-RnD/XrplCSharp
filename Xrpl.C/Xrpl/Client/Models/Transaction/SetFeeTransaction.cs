using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class SetFeeTransaction : TransactionCommon, ISetFeeTransaction
    {
        public SetFeeTransaction()
        {
            TransactionType = TransactionType.SetFee;
        }

        public string BaseFee { get; set; }

        public uint ReferenceFeeUnits { get; set; }

        public uint ReserveBase { get; set; }

        public uint ReserveIncrement { get; set; }

        public uint LedgerSequence { get; set; }
    }

    public interface ISetFeeTransaction : ITransactionCommon
    {
        string BaseFee { get; set; }
        uint LedgerSequence { get; set; }
        uint ReferenceFeeUnits { get; set; }
        uint ReserveBase { get; set; }
        uint ReserveIncrement { get; set; }
    }

    public class SetFeeTransactionResponse : TransactionResponseCommon, ISetFeeTransaction
    {
        public string BaseFee { get; set; }
        public uint LedgerSequence { get; set; }
        public uint ReferenceFeeUnits { get; set; }
        public uint ReserveBase { get; set; }
        public uint ReserveIncrement { get; set; }
    }
}
