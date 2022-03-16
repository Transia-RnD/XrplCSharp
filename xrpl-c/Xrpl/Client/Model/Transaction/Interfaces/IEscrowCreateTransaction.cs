using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IEscrowCreateTransaction : ITransactionCommon
    {
        Currency Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Condition { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        DateTime? FinishAfter { get; set; }
        uint? SourceTag { get; set; }
    }
}