namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface IEscrowFinishTransaction : ITransactionCommon
    {
        string Condition { get; set; }
        string Fulfillment { get; set; }
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }
}