using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/accountDelete.ts

namespace Xrpl.Client.Models.Transactions
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
