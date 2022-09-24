using System.Collections.Generic;
using Xrpl.Models.Ledger;
using Xrpl.Models;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/signerListSet.ts

namespace Xrpl.Models.Transactions
{
    public class SignerListSet : TransactionCommon, ISignerListSet
    {

        public SignerListSet()
        {
            TransactionType = TransactionType.SignerListSet;
        }

        public uint SignerQuorum { get; set; }

        public List<SignerEntry> SignerEntries { get; set; }
    }

    public interface ISignerListSet : ITransactionCommon
    {
        List<SignerEntry> SignerEntries { get; set; }
        uint SignerQuorum { get; set; }
    }

    public class SignerListSetResponse : TransactionResponseCommon, ISignerListSet
    {
        public List<SignerEntry> SignerEntries { get; set; }
        public uint SignerQuorum { get; set; }
    }
}
