using Xrpl.Client.Model.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.Interfaces;

namespace Xrpl.Client.Responses.Transaction.TransactionTypes
{
    public class SetRegularKeyTransactionResponse : TransactionResponseCommon, ISetRegularKeyTransaction
    {
        public string RegularKey { get; set; }
    }
}
