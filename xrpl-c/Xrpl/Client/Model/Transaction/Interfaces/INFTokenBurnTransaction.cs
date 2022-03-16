using System;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface INFTokenBurnTransaction : ITransactionCommon
    {
        //string Issuer { get; set; } // INHEIRTED FROM COMMON
        string TokenID { get; set; }
    }
}