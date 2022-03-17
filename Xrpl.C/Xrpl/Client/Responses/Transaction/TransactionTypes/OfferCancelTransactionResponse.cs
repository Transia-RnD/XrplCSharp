using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class OfferCancelTransactionResponse : TransactionResponseCommon, IOfferCancelTransaction
    {
        public uint OfferSequence { get; set; }
    }
}
