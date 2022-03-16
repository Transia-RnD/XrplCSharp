namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface ISetRegularKeyTransaction : ITransactionCommon
    {
        string RegularKey { get; set; }
    }
}