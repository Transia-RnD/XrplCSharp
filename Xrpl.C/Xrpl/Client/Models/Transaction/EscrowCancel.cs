
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Client.Models.Transactions
{
    public class EscrowCancel : TransactionCommon, IEscrowCancel
    {
        public EscrowCancel()
        {
            TransactionType = TransactionType.EscrowCancel;
        }

        public string Owner { get; set; }

        public uint OfferSequence { get; set; }
    }

    public interface IEscrowCancel : ITransactionCommon
    {
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }

    public class EscrowCancelResponse : TransactionResponseCommon, IEscrowCancel
    {
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
