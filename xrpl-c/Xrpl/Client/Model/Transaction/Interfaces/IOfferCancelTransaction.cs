namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IOfferCancelTransaction : ITransactionCommon
    {
        uint OfferSequence { get; set; }
    }
}