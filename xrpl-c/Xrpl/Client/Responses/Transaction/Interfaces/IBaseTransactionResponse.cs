using System;

namespace RippleDotNet.Responses.Transaction.Interfaces
{
    public interface IBaseTransactionResponse
    {
        DateTime? Date { get; set; }

        string Hash { get; set; }

        uint? InLedger { get; set; }

        uint? LedgerIndex { get; set; }

        bool? Validated { get; set; }
    }
}