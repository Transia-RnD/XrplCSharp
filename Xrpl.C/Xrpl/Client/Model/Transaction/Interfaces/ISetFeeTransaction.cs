namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ISetFeeTransaction : ITransactionCommon
    {
        string BaseFee { get; set; }
        uint LedgerSequence { get; set; }
        uint ReferenceFeeUnits { get; set; }
        uint ReserveBase { get; set; }
        uint ReserveIncrement { get; set; }
    }
}