using System;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface INFTokenMintTransaction : ITransactionCommon
    {
        new NFTokenMintFlags? Flags { get; set; }
        uint NFTokenTaxon { get; set; }
        string Issuer { get; set; }
        uint? TransferFee { get; set; }
        string URI { get; set; }
    }
}