using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class EscrowFinishTransactionResponse : TransactionResponseCommon, IEscrowFinishTransaction
    {
        public string Condition { get; set; }
        public string Fulfillment { get; set; }
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
