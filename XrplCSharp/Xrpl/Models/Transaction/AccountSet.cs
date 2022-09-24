
using Xrpl.Models;

// 

namespace Xrpl.Models.Transactions
{
    public enum AccountSetTfFlags
    {
        asfRequireDest = 1,
        asfRequireAuth = 2,
        asfDisallowXRP = 3,
        asfDisableMaster = 4,
        asfAccountTxnID = 5,
        asfNoFreeze = 6,
        asfGlobalFreeze = 7,
        asfDefaultRipple = 8
    }

    //public interface IAccountSetFlags
    //{
    //    uint asfRequireDest { get; set; }
    //    uint asfRequireAuth { get; set; }
    //    uint asfDisallowXRP { get; set; }
    //    uint asfDisableMaster { get; set; }
    //    uint asfAccountTxnID { get; set; }
    //    uint asfNoFreeze { get; set; }
    //    uint asfGlobalFreeze { get; set; }
    //    uint asfDefaultRipple { get; set; }
    //}

    public class AccountSet : TransactionCommon, IAccountSet
    {
        public AccountSet()
        {
            TransactionType = TransactionType.AccountSet;
        }

        public AccountSet(string account) : this()
        {
            Account = account;
        }
        public uint? ClearFlag { get; set; }
        public string Domain { get; set; }
        public string EmailHash { get; set; }
        public string MessageKey { get; set; }
        public uint? SetFlag { get; set; }
        public uint? TransferRate { get; set; }
        public uint? TickSize { get; set; }
    }

    public interface IAccountSet : ITransactionCommon
    {
        uint? ClearFlag { get; set; }
        string Domain { get; set; }
        string EmailHash { get; set; }
        string MessageKey { get; set; }
        uint? SetFlag { get; set; }
        uint? TransferRate { get; set; }
        uint? TickSize { get; set; }
    }

    public class AccountSetResponse : TransactionResponseCommon, IAccountSet
    {
        public uint? ClearFlag { get; set; }
        public string Domain { get; set; }
        public string EmailHash { get; set; }
        public string MessageKey { get; set; }
        public uint? SetFlag { get; set; }
        public uint? TransferRate { get; set; }
        public uint? TickSize { get; set; }        
    }
}
