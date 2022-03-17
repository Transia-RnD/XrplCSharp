using System.Collections.Generic;
using Xrpl.Client.Model.Ledger.Objects;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ISignerListSetTransaction : ITransactionCommon
    {
        List<SignerEntry> SignerEntries { get; set; }
        uint SignerQuorum { get; set; }
    }
}