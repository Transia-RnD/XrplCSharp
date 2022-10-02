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

        /// <inheritdoc />
        public string Account { get; set; }

        /// <inheritdoc />
        public string AccountTxnID { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Fee { get; set; }
        /// <inheritdoc />
        public uint? Flags { get; set; }
        /// <inheritdoc />
        public uint? LastLedgerSequence { get; set; }
        /// <inheritdoc />
        public List<Memo> Memos { get; set; }
        /// <inheritdoc />
        public uint? Sequence { get; set; }
        /// <inheritdoc />
        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }
        /// <inheritdoc />
        public List<Signer> Signers { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        /// <inheritdoc />
        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        /// <inheritdoc />
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("date")]
        public uint? date { get; set; }

        [JsonProperty("inLedger")]
        public uint? inLedger { get; set; }

        [JsonProperty("ledger_index")]
        public uint? ledger_index { get; set; }

        /// <inheritdoc />
        public string ToJson()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return JsonConvert.SerializeObject(this, serializerSettings);
        }

        //todo not found fields -  SourceTag?: number, TicketSequence?: number
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

    /// <summary>
    /// Transaction metadata is a section of data that gets added to a transaction after it is processed.<br/>
    /// Any transaction that gets included in a ledger has metadata, regardless of whether it is successful.<br/>
    /// The transaction metadata describes the outcome of the transaction in detail.<br/>
    /// <remarks>
    /// Warning: The changes described in transaction metadata are only final if the transaction is in a validated ledger version.
    /// </remarks>
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// List of ledger objects that were created, deleted, or modified by this transaction, and specific changes to each.
        /// </summary>
        public List<AffectedNode> AffectedNodes { get; set; }

        public string MetaBlob { get; set; } //todo unknown field

        /// <summary>
        /// The transaction's position within the ledger that included it.<br/>
        /// This is zero-indexed.<br/>
        /// (For example, the value 2 means it was the 3rd transaction in that ledger.)
        /// </summary>
        public int TransactionIndex { get; set; }

        /// <summary>
        /// A result code indicating whether the transaction succeeded or how it failed.
        /// </summary>
        public string TransactionResult { get; set; }

        /// <summary>
        /// (Omitted for non-Payment transactions) The Currency Amount actually received by the Destination account.<br/>
        /// Use this field to determine how much was delivered, regardless of whether the transaction is a partial payment.<br/>
        /// See this description for details.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        [JsonProperty("delivered_amount")]
        public Currency ActuallyDeliveredAmount { get; set; }
        /// <summary>
        /// (May be omitted) For a partial payment, this field records the amount of currency actually delivered to the destination.<br/>
        /// To avoid errors when reading transactions, instead use the delivered_amount field, which is provided for all Payment transactions, partial or not.
        /// </summary>
        [JsonConverter(typeof(CurrencyConverter))]
        [JsonProperty("DeliveredAmount")]
        public Currency PartialDeliveredAmount { get; set; }
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
        public string Owner { get; set; }
        public string RootIndex { get; set; }
        public string BookDirectory { get; set; }
        public string BookNode { get; set; }
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerGets { get; set; }
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency TakerPays { get; set; }
        public string ExchangeRate { get; set; }
        public string TakerGetsCurrency { get; set; }
        public string TakerGetsIssuer { get; set; }
        public string TakerPaysCurrency { get; set; }
        public string TakerPaysIssuer { get; set; }

    }

    public class PreviousFields
    {
        [JsonConverter(typeof(CurrencyConverter))]
        public object Balance { get; set; } //todo change type to Currency
        public int Sequence { get; set; }
    }

    /// <summary>
    /// The AffectedNodes array contains a complete list of the objects in the ledger that this transaction modified in some way. 
    /// </summary>
    public class AffectedNode
    {
        /// <summary>
        /// indicates that the transaction created a new object in the ledger.
        /// </summary>
        public NodeInfo CreatedNode { get; set; }
        /// <summary>
        /// indicates that the transaction removed an object from the ledger.
        /// </summary>
        public NodeInfo DeletedNode { get; set; }
        /// <summary>
        /// indicates that the transaction modified an existing object in the ledger.
        /// <remarks>
        /// If the modified ledger object has PreviousTxnID and PreviousTxnLgrSeq fields,
        /// the transaction always updates them with the transaction's own identifying hash and the index of the ledger version that included the transaction,
        /// but these fields' new value is not listed in the FinalFields of the ModifiedNode object,
        /// and their previous values are listed at the top level of the ModifiedNode object rather than in the nested PreviousFields object.
        /// </remarks>
        /// </summary>
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
        /// <summary>
        /// The type of ledger object
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; set; }
        /// <summary>
        /// The ID of this ledger object in the ledger's state tree.<br/>
        /// Note: This is not the same as a ledger index, even though the field name is very similar.
        /// </summary>
        public string LedgerIndex { get; set; }
        /// <summary>
        /// ModifiedNode <br/>
        /// (May be omitted) The identifying hash of the previous transaction to modify this ledger object.<br/>
        /// Omitted for ledger object types that do not have a PreviousTxnID field.
        /// </summary>
        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }

        /// <summary>
        /// ModifiedNode <br/>
        /// (May be omitted) The Ledger Index of the ledger version containing the previous transaction to modify this ledger object.<br/>
        /// Omitted for ledger object types that do not have a PreviousTxnLgrSeq field.
        /// </summary>
        [JsonProperty("PreviousTxnLgrSeq")]
        public uint? PreviousTxnLgrSeq { get; set; }
        /// <summary>
        /// <remarks>
        /// DeletedNode <br/>
        /// The content fields of the ledger object immediately before it was deleted.<br/>
        /// Which fields are present depends on what type of ledger object was created.
        /// </remarks>
        /// <remarks>
        /// ModifiedNode <br/>
        /// The content fields of the ledger object after applying any changes from this transaction.<br/>
        /// Which fields are present depends on what type of ledger object was created.<br/>
        /// This omits the PreviousTxnID and PreviousTxnLgrSeq fields, even though most types of ledger objects have them.
        /// </remarks>
        /// </summary>
        [JsonProperty("FinalFields")]
        public FieldInfo FinalFields { get; set; }

        /// <summary>
        /// CreatedNode <br/>
        /// The content fields of the newly-created ledger object.<br/>
        /// Which fields are present depends on what type of ledger object was created.
        /// </summary>
        [JsonProperty("NewFields")]
        public FieldInfo NewFields { get; set; }
        /// <summary>
        /// ModifiedNode <br/>
        /// The previous values for all fields of the object that were changed as a result of this transaction.<br/>
        /// If the transaction only added fields to the object, this field is an empty object.
        /// </summary>
        public dynamic PreviousFields { get; set; }
    }

    public interface ITransactionCommon
    {
        /// <summary>
        /// This is a required field
        /// The unique address of the account that initiated the transaction.
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// Hash value identifying another transaction.<br/>
        /// If provided, this transaction is only valid if the sending account's previously-sent transaction matches the provided hash.
        /// </summary>
        string AccountTxnID { get; set; }

        /// <summary>
        /// Integer amount of XRP, in drops, to be destroyed as a cost for distributing this transaction to the network.<br/>
        /// Some transaction types have different minimum requirements.
        /// </summary>
        Currency Fee { get; set; }

        /// <summary>
        /// Set of bit-flags for this transaction.<br/>
        /// number | GlobalFlags
        /// </summary>
        uint? Flags { get; set; }

        /// <summary>
        /// Although optional, the LastLedgerSequence is strongly recommended on every transaction to ensure it's validated or rejected promptly.<br/>
        /// Highest ledger index this transaction can appear in.<br/>
        /// Specifying this field places a strict upper limit on how long the transaction can wait to be validated or rejected.
        /// </summary>
        uint? LastLedgerSequence { get; set; }

        /// <summary>
        /// Additional arbitrary information used to identify this transaction.
        /// </summary>
        List<Memo> Memos { get; set; }

        /// <summary>
        /// Transaction metadata is a section of data that gets added to a transaction after it is processed.<br/>
        /// Any transaction that gets included in a ledger has metadata, regardless of whether it is successful.<br/>
        /// The transaction metadata describes the outcome of the transaction in detail.<br/>
        /// <remarks>
        /// Warning: The changes described in transaction metadata are only final if the transaction is in a validated ledger version.
        /// </remarks>
        /// </summary>
        Meta Meta { get; set; }

        /// <summary>
        /// The sequence number of the account sending the transaction.<br/>
        /// A transaction is only valid if the Sequence number is exactly 1 greater than the previous transaction from the same account.<br/>
        /// The special case 0 means the transaction is using a Ticket instead.
        /// </summary>
        uint? Sequence { get; set; }

        /// <summary>
        /// Array of objects that represent a multi-signature which authorizes this transaction.
        /// </summary>
        List<Signer> Signers { get; set; }

        /// <summary>
        /// Hex representation of the public key that corresponds to the private key used to sign this transaction.<br/>
        /// If an empty string, indicates a multi-signature is present in the Signers field instead.
        /// </summary>
        string SigningPublicKey { get; set; }

        /// <summary>
        /// The signature that verifies this transaction as originating from the account it says it is from.
        /// </summary>
        string TransactionSignature { get; set; }

        /// <summary>
        /// The type of transaction.<br/>
        /// Valid types include: `Payment`, `OfferCreate`, `SignerListSet`,
        /// `EscrowCreate`, `EscrowFinish`, `EscrowCancel`, `PaymentChannelCreate`,
        /// `PaymentChannelFund`, `PaymentChannelClaim`, and `DepositPreauth`.
        /// </summary>
        TransactionType TransactionType { get; set; }
        /// <summary>
        /// convert transaction to string json value
        /// </summary>
        /// <returns></returns>
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
