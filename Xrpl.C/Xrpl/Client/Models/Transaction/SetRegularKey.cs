using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/setRegularKey.ts

namespace Xrpl.Client.Models.Transactions
{
    public class SetRegularKey : TransactionCommon, ISetRegularKey
    {
        public SetRegularKey()
        {
            TransactionType = TransactionType.SetRegularKey;
        }


        public string RegularKey { get; set; }
    }

    public interface ISetRegularKey : ITransactionCommon
    {
        string RegularKey { get; set; }
    }

    public class SetRegularKeyResponse : TransactionResponseCommon, ISetRegularKey
    {
        public string RegularKey { get; set; }
    }
}
