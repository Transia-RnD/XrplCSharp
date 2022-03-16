using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.Interfaces;

namespace RippleDotNet.Responses.Transaction.TransactionTypes
{
    public class SetRegularKeyTransactionResponse : TransactionResponseCommon, ISetRegularKeyTransaction
    {
        public string RegularKey { get; set; }
    }
}
