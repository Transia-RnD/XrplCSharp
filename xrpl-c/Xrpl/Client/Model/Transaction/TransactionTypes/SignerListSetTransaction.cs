using System.Collections.Generic;
using RippleDotNet.Model.Ledger;
using RippleDotNet.Model.Ledger.Objects;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
{
    public class SignerListSetTransaction : TransactionCommon, ISignerListSetTransaction
    {

        public SignerListSetTransaction()
        {
            TransactionType = TransactionType.SignerListSet;
        }

        public uint SignerQuorum { get; set; }

        public List<SignerEntry> SignerEntries { get; set; }
    }
}
