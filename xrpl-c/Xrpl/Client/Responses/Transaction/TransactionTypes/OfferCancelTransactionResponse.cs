using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class OfferCancelTransactionResponse : TransactionResponseCommon, IOfferCancelTransaction
    {
        public uint OfferSequence { get; set; }
    }
}
