using System.Collections.Generic;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
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

    public interface ISignerListSetTransaction : ITransactionCommon
    {
        List<SignerEntry> SignerEntries { get; set; }
        uint SignerQuorum { get; set; }
    }

    public class SignerListSetTransactionResponse : TransactionResponseCommon, ISignerListSetTransaction
    {
        public List<SignerEntry> SignerEntries { get; set; }
        public uint SignerQuorum { get; set; }
    }
}
