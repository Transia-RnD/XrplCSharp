
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class EscrowCancelTransaction : TransactionCommon, IEscrowCancelTransaction
    {
        public EscrowCancelTransaction()
        {
            TransactionType = TransactionType.EscrowCancel;
        }

        public string Owner { get; set; }

        public uint OfferSequence { get; set; }
    }

    public interface IEscrowCancelTransaction : ITransactionCommon
    {
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }

    public class EscrowCancelTransactionResponse : TransactionResponseCommon, IEscrowCancelTransaction
    {
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
