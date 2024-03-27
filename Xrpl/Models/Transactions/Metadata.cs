using Newtonsoft.Json;

using System.Collections.Generic;

using Xrpl.Client.Json.Converters;

using Xrpl.Models.Common;

//https://github.com/XRPLF/xrpl.js/blob/45963b70356f4609781a6396407e2211fd15bcf1/packages/xrpl/src/models/transactions/metadata.ts#L32
namespace Xrpl.Models.Transactions
{
    //todo replace Meta in transactionCommon to this interfaces;

    public interface ICreatedNode
    {
        string LedgerEntryType { get; set; }
        string LedgerIndex { get; set; }
        Dictionary<string, object> NewFields { get; set; }
    }

    public interface IModifiedNode
    {
        string LedgerEntryType { get; set; }
        string LedgerIndex { get; set; }
        Dictionary<string, object> FinalFields { get; set; }
        Dictionary<string, object> PreviousFields { get; set; }
        string PreviousTxnID { get; set; }
        int PreviousTxnLgrSeq { get; set; }
    }

    public interface IDeletedNode
    {
        string LedgerEntryType { get; set; }
        string LedgerIndex { get; set; }
        Dictionary<string, object> FinalFields { get; set; }
    }

    public interface INode : ICreatedNode, IModifiedNode, IDeletedNode
    {
    }

    public interface TransactionMetadata
    {
        List<INode> AffectedNodes { get; set; }
        [JsonConverter(typeof(CurrencyConverter))]
        [JsonProperty("DeliveredAmount")]
        Currency DeliveredAmount { get; set; }
        [JsonConverter(typeof(CurrencyConverter))]
        [JsonProperty("delivered_amount")]
        Currency Delivered_amount { get; set; }
        int TransactionIndex { get; set; }
        string TransactionResult { get; set; }
    }
}
