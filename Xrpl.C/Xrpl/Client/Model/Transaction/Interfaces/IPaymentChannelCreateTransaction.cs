using System;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface IPaymentChannelCreateTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        DateTime? CancelAfter { get; set; }
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
        string PublicKey { get; set; }
        uint SettleDelay { get; set; }
        uint? SourceTag { get; set; }
    }
}