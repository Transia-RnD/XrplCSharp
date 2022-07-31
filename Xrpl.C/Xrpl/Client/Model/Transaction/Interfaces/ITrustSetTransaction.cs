using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ITrustSetTransaction : ITransactionCommon
    {
        new TrustSetFlags? Flags { get; set; }
        Currency LimitAmount { get; set; }
        uint? QualityIn { get; set; }
        uint? QualityOut { get; set; }
    }
}