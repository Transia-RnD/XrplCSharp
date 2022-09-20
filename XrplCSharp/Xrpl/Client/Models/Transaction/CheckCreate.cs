using Newtonsoft.Json;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Enums;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/checkCreate.ts

namespace Xrpl.Client.Models.Transactions
{
    public class CheckCreate : TransactionCommon, ICheckCreate
    {
        public CheckCreate()
        {
            TransactionType = TransactionType.CheckCreate;
        }
        public string Destination { get; set; }
        public Currency SendMax { get; set; }
        public uint? DestinationTag { get; set; }
        public uint? Expiration { get; set; }
        public uint? InvoiceID { get; set; }
    }

    public interface ICheckCreate : ITransactionCommon
    {
        string Destination { get; set; }
        Currency SendMax { get; set; }
        uint? DestinationTag { get; set; }
        uint? Expiration { get; set; }
        uint? InvoiceID { get; set; }
    }

    public class CheckCreateResponse : TransactionResponseCommon, ICheckCreate
    {
        public string Destination { get; set; }
        public Currency SendMax { get; set; }
        public uint? DestinationTag { get; set; }
        public uint? Expiration { get; set; }
        public uint? InvoiceID { get; set; }       
    }
}
