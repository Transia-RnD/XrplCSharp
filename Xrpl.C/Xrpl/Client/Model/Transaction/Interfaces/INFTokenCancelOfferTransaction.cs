namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface INFTokenCancelOfferTransaction : ITransactionCommon
    {
        string[] NFTokenOffers { get; set; }
    }
}