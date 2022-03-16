using System.Collections.Generic;
using RippleDotNet.Model.Ledger.Objects;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class SignerListSetTransactionResponse : TransactionResponseCommon, ISignerListSetTransaction
    {
        public List<SignerEntry> SignerEntries { get; set; }
        public uint SignerQuorum { get; set; }
    }
}
