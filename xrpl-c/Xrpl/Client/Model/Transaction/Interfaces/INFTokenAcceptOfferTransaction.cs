using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface INFTokenAcceptOfferTransaction : ITransactionCommon
    {
        string TokenID { get; set; }
    }
}