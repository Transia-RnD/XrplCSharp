

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/paymentChannelClaim.ts

namespace Xrpl.Models.Transactions
{
    public class EscrowFinish : TransactionCommon, IEscrowFinish
    {
        public EscrowFinish()
        {
            TransactionType = TransactionType.EscrowFinish;
        }

        public EscrowFinish(string owner, uint offerSequence, string condition, string fulfillment)
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

    public interface IEscrowFinish : ITransactionCommon
    {
        string Condition { get; set; }
        string Fulfillment { get; set; }
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }

    public class EscrowFinishResponse : TransactionResponseCommon, IEscrowFinish
    {
        public string Condition { get; set; }
        public string Fulfillment { get; set; }
        public uint OfferSequence { get; set; }
        public string Owner { get; set; }
    }
}
