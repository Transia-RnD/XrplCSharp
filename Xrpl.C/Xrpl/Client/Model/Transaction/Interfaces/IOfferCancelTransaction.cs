namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface IOfferCancelTransaction : ITransactionCommon
    {
        uint OfferSequence { get; set; }
    }
}