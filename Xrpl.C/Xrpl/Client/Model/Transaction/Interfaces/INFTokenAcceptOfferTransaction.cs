using System;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface INFTokenAcceptOfferTransaction : ITransactionCommon
    {
        string NFTokenID { get; set; }
    }
}