using System.Collections.Generic;
using Xrpl.Client.Model.Ledger.Objects;
using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class SignerListSetTransactionResponse : TransactionResponseCommon, ISignerListSetTransaction
    {
        public List<SignerEntry> SignerEntries { get; set; }
        public uint SignerQuorum { get; set; }
    }
}
