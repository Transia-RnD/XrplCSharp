using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class SetFeeTransactionResponse : TransactionResponseCommon, ISetFeeTransaction
    {
        public string BaseFee { get; set; }
        public uint LedgerSequence { get; set; }
        public uint ReferenceFeeUnits { get; set; }
        public uint ReserveBase { get; set; }
        public uint ReserveIncrement { get; set; }
    }
}
