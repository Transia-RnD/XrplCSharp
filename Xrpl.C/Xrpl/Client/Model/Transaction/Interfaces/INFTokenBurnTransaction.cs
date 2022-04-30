using System;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface INFTokenBurnTransaction : ITransactionCommon
    {
        //string Issuer { get; set; } // INHEIRTED FROM COMMON
        string NFTokenID { get; set; }
    }
}