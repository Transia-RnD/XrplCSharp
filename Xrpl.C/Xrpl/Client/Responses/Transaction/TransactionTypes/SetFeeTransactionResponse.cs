using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
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
