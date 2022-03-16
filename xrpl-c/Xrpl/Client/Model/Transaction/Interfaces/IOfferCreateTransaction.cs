using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface IOfferCreateTransaction : ITransactionCommon
    {
        DateTime? Expiration { get; set; }
        new OfferCreateFlags? Flags { get; set; }
        uint? OfferSequence { get; set; }
        Currency TakerGets { get; set; }
        Currency TakerPays { get; set; }

    }
}