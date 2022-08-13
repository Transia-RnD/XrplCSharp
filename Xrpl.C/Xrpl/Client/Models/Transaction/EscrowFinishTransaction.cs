using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
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

    public interface IEscrowFinishTransaction : ITransactionCommon
    {
        string Condition { get; set; }
        string Fulfillment { get; set; }
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }

    public class EscrowFinishTransactionResponse : TransactionResponseCommon, IEscrowFinishTransaction
    {
        public string Condition { get; set; }
        public string Fulfillment { get; set; }
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
