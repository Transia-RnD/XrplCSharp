using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Models.Transactions
{
    public class SetRegularKeyTransaction : TransactionCommon, ISetRegularKeyTransaction
    {
        public SetRegularKeyTransaction()
        {
            TransactionType = TransactionType.SetRegularKey;
        }


        public string RegularKey { get; set; }
    }

    public interface ISetRegularKeyTransaction : ITransactionCommon
    {
        string RegularKey { get; set; }
    }

    public class SetRegularKeyTransactionResponse : TransactionResponseCommon, ISetRegularKeyTransaction
    {
        public string RegularKey { get; set; }
    }
}
