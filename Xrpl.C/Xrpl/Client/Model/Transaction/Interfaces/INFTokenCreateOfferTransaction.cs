using System;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface INFTokenCreateOfferTransaction : ITransactionCommon
    {
        DateTime? Expiration { get; set; }
        new NFTokenCreateOfferFlags? Flags { get; set; }
        string NFTokenID { get; set; }
        Currency Amount { get; set; }
        string Owner { get; set; }
        string Destination { get; set; }
    }
}