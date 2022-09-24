using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.ClientLib.Extensions;
using Xrpl.ClientLib.Json.Converters;
using Xrpl.Models;
using Xrpl.Models.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/common.ts

namespace Xrpl.Models.Transactions
{
    [JsonConverter(typeof(TransactionConverter))]
    public abstract class TransactionCommon : ITransactionCommon
    {
        protected TransactionCommon()
        {
            Fee = new Currency { Value = "10" };
        }

        /// <summary>
        /// This is a required field
        /// </summary>
        public string Account { get; set; }

        public string AccountTxnID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Fee { get; set; }

        public uint? Flags { get; set; }

        /// <summary>
        /// Although optional, the LastLedgerSequence is strongly recommended on every transaction to ensure it's validated or rejected promptly.
        /// </summary>
        public uint? LastLedgerSequence { get; set; }

        public List<Memo> Memos { get; set; }

        /// <summary>
        /// This is a required field
        /// </summary>
        public uint? Sequence { get; set; }

        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }

        public List<Signer> Signers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("date")]
        public uint? date { get; set; }

        [JsonProperty("inLedger")]
        public uint? inLedger { get; set; }

        [JsonProperty("ledger_index")]
        public uint? ledger_index { get; set; }

        public string ToJson()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }

    public class Memo
    {
        [JsonProperty("Memo")]
        public Memo2 Memo2 { get; set; }
    }

    public class Memo2
    {
        public string MemoData { get; set; }

        [JsonIgnore]
        public string MemoDataAsText => MemoData.FromHexString();

        public string MemoFormat { get; set; }

        [JsonIgnore]
        public string MemoFormatAsText => MemoFormat.FromHexString();

        public string MemoType { get; set; }

        [JsonIgnore]
        public string MemoTypeAsText => MemoType.FromHexString();
    }

    public class Signer
    {
        public string Account { get; set; }

        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }
    }

    public class Meta
    {
        public List<AffectedNode> AffectedNodes { get; set; }

        public string MetaBlob { get; set; }

        public int TransactionIndex { get; set; }

        public string TransactionResult { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        [JsonProperty("delivered_amount")]
        public Currency DeliveredAmount { get; set; }
    }

    public class FieldInfo
    {
        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Balance")]
        public object Balance { get; set; }

        [JsonProperty("Flags")]
        public int Flags { get; set; }

        [JsonProperty("OwnerCount")]
        public int OwnerCount { get; set; }

        [JsonProperty("Sequence")]
        public int Sequence { get; set; }
        
        [JsonProperty("MintedTokens")]
        public int MintedTokens { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency HighLimit { get; set; }

        public string HighNode { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LowLimit { get; set; }

        public string LowNode { get; set; }

        public List<INFToken> NFTokens { get; set; }
    }

    public class PreviousFields
    {
        [JsonConverter(typeof(CurrencyConverter))]
        public object Balance { get; set; }
        public int Sequence { get; set; }
    }
    

    public class AffectedNode
    {
        public NodeInfo CreatedNode { get; set; }

        public NodeInfo DeletedNode { get; set; }

        public NodeInfo ModifiedNode { get; set; }
    }

    public class INFToken
    {
        public NFToken NFToken { get; set; }
    }

    public class NFToken
    {
        public string NFTokenID { get; set; }

        public string URI { get; set; }
    }

    public class NodeInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        public string LedgerIndex { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint? PreviousTxnLgrSeq { get; set; }

        [JsonProperty("FinalFields")]
        public FieldInfo FinalFields { get; set; }

        [JsonProperty("NewFields")]
        public FieldInfo NewFields { get; set; }

        public dynamic PreviousFields { get; set; }
    }

    public interface ITransactionCommon
    {
        string Account { get; set; }

        string AccountTxnID { get; set; }

        Currency Fee { get; set; }

        uint? Flags { get; set; }

        uint? LastLedgerSequence { get; set; }

        List<Memo> Memos { get; set; }

        Meta Meta { get; set; }

        uint? Sequence { get; set; }

        List<Signer> Signers { get; set; }

        string SigningPublicKey { get; set; }

        string TransactionSignature { get; set; }

        TransactionType TransactionType { get; set; }

        string ToJson();
    }

    public interface ITransactionResponseCommon : IBaseTransactionResponse
    {
        string Account { get; set; }
        string AccountTxnID { get; set; }
        Currency Fee { get; set; }
        uint? Flags { get; set; }
        uint? LastLedgerSequence { get; set; }
        List<Memo> Memos { get; set; }
        Meta Meta { get; set; }
        uint? date { get; set; }
        uint? inLedger { get; set; }
        uint? ledger_index { get; set; }
        uint? Sequence { get; set; }
        List<Signer> Signers { get; set; }
        string SigningPublicKey { get; set; }
        string TransactionSignature { get; set; }
        TransactionType TransactionType { get; set; }

        string ToJson();
    }

    [JsonConverter(typeof(TransactionConverter))]
    public abstract class   TransactionResponseCommon : BaseTransactionResponse, ITransactionCommon, ITransactionResponseCommon
    {
        public string Account { get; set; }

        public string AccountTxnID { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Fee { get; set; }

        public uint? Flags { get; set; }

        /// <summary>
        /// Although optional, the LastLedgerSequence is strongly recommended on every transaction to ensure it's validated or rejected promptly.
        /// </summary>
        public uint? LastLedgerSequence { get; set; }

        public List<Memo> Memos { get; set; }

        public uint? Sequence { get; set; }

        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }

        public List<Signer> Signers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("date")]
        public uint? date { get; set; }

        [JsonProperty("inLedger")]
        public uint? inLedger { get; set; }

        [JsonProperty("ledger_index")]
        public uint? ledger_index { get; set; }

        public string ToJson()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}
