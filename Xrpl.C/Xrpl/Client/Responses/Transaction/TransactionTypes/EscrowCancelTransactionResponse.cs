using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class EscrowCancelTransactionResponse : TransactionResponseCommon, IEscrowCancelTransaction
    {
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
