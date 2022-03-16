using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface INFTokenCreateOfferTransaction : ITransactionCommon
    {
        DateTime? Expiration { get; set; }
        new NFTokenCreateOfferFlags? Flags { get; set; }
        string TokenID { get; set; }
        string Amount { get; set; }
        string Owner { get; set; }
        string Destination { get; set; }
    }
}