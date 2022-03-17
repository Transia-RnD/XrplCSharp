namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ISetRegularKeyTransaction : ITransactionCommon
    {
        string RegularKey { get; set; }
    }
}