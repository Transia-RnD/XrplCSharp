using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class EscrowFinishTransaction : TransactionCommon, IEscrowFinishTransaction
    {
        public EscrowFinishTransaction()
        {
            TransactionType = TransactionType.EscrowFinish;
        }

        public EscrowFinishTransaction(string owner, uint offerSequence, string condition, string fulfillment)
        {
            Owner = owner;
            OfferSequence = offerSequence;
            Condition = condition;
            Fulfillment = fulfillment;
        }

        public string Owner { get; set; }

        public uint OfferSequence { get; set; }

        public string Condition { get; set; }

        public string Fulfillment { get; set; }
    }
}
