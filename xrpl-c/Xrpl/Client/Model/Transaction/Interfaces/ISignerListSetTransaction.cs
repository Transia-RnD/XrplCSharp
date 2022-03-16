using System.Collections.Generic;
using RippleDotNet.Model.Ledger.Objects;

namespace RippleDotNet.Model.Transaction.Interfaces
{
    public interface ISignerListSetTransaction : ITransactionCommon
    {
        List<SignerEntry> SignerEntries { get; set; }
        uint SignerQuorum { get; set; }
    }
}