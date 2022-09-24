using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;
using Xrpl.Models.Methods;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/accountDelete.ts

namespace Xrpl.Models.Transactions
{

    public interface IAccountDelete : ITransactionCommon
    {
        string Destination { get; set; }
        uint? DestinationTag { get; set; }
    }
    
    public class AccountDelete : TransactionCommon, IAccountDelete
    {
        public AccountDelete()
        {
            TransactionType = TransactionType.AccountDelete;
        }

        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }
    }

    public class AccountDeleteResponse : TransactionResponseCommon, IAccountDelete
    {
        public string Destination { get; set; }

        public uint? DestinationTag { get; set; }
    }
}
