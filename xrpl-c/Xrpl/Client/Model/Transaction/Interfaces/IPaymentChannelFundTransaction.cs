using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IPaymentChannelFundTransaction : ITransactionCommon
    {
        string Amount { get; set; }
        string Channel { get; set; }
        DateTime? Expiration { get; set; }
    }
}