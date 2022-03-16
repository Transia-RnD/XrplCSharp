using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RippleDotNet.Extensions;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model.Transaction.Interfaces;

namespace RippleDotNet.Model.Transaction.TransactionTypes
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

        [JsonProperty("metaData")]
        public Meta Meta { get; set; }      

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

    public class FinalFields
    {
        public string Account { get; set; }

        public object Balance { get; set; }

        public int Flags { get; set; }

        public int OwnerCount { get; set; }

        public int Sequence { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency HighLimit { get; set; }

        public string HighNode { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public Currency LowLimit { get; set; }

        public string LowNode { get; set; }
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

    public class NodeInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }

        public string LedgerIndex { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public uint? PreviousTransactionLedgerSequence { get; set; }

        public dynamic FinalFields { get; set; }

        public dynamic NewFields { get; set; }

        public dynamic PreviousFields { get; set; }
    }
}
