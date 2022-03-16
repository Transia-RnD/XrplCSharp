using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface INFTokenMintTransaction : ITransactionCommon
    {
        new NFTokenMintFlags? Flags { get; set; }
        uint? TokenTaxon { get; set; }
        string Issuer { get; set; }
        uint? TransferFee { get; set; }
        string URI { get; set; }
    }
}