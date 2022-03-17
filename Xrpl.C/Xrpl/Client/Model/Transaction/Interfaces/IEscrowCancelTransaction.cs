namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface IEscrowCancelTransaction : ITransactionCommon
    {
        uint OfferSequence { get; set; }
        string Owner { get; set; }
    }
}