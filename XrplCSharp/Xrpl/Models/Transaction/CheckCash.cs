using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCash.ts

namespace Xrpl.Models.Transactions
{
    public class CheckCash : TransactionCommon, ICheckCash
    {
        public CheckCash()
        {
            TransactionType = TransactionType.CheckCash;
        }
        public string CheckID { get; set; }
        public Currency? Amount { get; set; }
        public Currency? DeliverMin { get; set; }
    }

    public interface ICheckCash : ITransactionCommon
    {
        string CheckID { get; set; }
        Currency? Amount { get; set; }
        Currency? DeliverMin { get; set; }
    }

    public class CheckCashResponse : TransactionResponseCommon, ICheckCash
    {
        public string CheckID { get; set; }
        public Currency? Amount { get; set; }
        public Currency? DeliverMin { get; set; }  
    }
}
