using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class EscrowFinishTransactionResponse : TransactionResponseCommon, IEscrowFinishTransaction
    {
        public string Condition { get; set; }
        public string Fulfillment { get; set; }
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
