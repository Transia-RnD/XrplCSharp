using System.Collections.Generic;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/signerListSet.ts

namespace Xrpl.Client.Models.Transactions
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
