

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/depositPreauth.ts

namespace Xrpl.Models.Transactions
{
    public class DepositPreauth : TransactionCommon, IDepositPreauth
    {
        public DepositPreauth()
        {
            TransactionType = TransactionType.DepositPreauth;
        }
        public string Authorize { get; set; }
        public string Unauthorize { get; set; }
    }

    public interface IDepositPreauth : ITransactionCommon
    {
        string Authorize { get; set; }
        string Unauthorize { get; set; }
    }

    public class DepositPreauthResponse : TransactionResponseCommon, IDepositPreauth
    {
        public string Authorize { get; set; }
        public string Unauthorize { get; set; }    
    }
}
