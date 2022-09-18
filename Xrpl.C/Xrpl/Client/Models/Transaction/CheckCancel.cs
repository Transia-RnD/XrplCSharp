using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCancel.ts

namespace Xrpl.Client.Models.Transactions
{
    public class CheckCancel : TransactionCommon, ICheckCancel
    {
        public CheckCancel()
        {
            TransactionType = TransactionType.CheckCancel;
        }
        public string CheckID { get; set; }
    }

    public interface ICheckCancel : ITransactionCommon
    {
        string CheckID { get; set; }
    }

    public class CheckCancelResponse : TransactionResponseCommon, ICheckCancel
    {
        public string CheckID { get; set; }    
    }
}
