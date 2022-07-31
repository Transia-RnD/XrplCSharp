using Xrpl.Client.Model.Transaction.Interfaces;

using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.TransactionTypes
{
    public class SetRegularKeyTransaction : TransactionCommon, ISetRegularKeyTransaction
    {
        public SetRegularKeyTransaction()
        {
            TransactionType = TransactionType.SetRegularKey;
        }


        public string RegularKey { get; set; }
    }
}
