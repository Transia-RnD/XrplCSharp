using System.Collections.Generic;

using Xrpl.Client.Model.Ledger;
using Xrpl.Client.Model.Ledger.Objects;
using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
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
