using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Extensions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Utils;
using Index = Xrpl.Models.Utils.Index;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/common.ts

namespace Xrpl.Models.Transactions
{
    public class Common
    {
        const int ISSUED_CURRENCY_SIZE = 3;
        private const int SIGNER_SIZE = 3;
        private const int MEMO_SIZE = 3;

        public static bool IsRecord(dynamic value)
        {
            return value != null && value is Dictionary<string, dynamic>;
        }
        /// <summary>
        /// Verify the form and type of an IssuedCurrencyAmount at runtime.
        /// </summary>
        /// <param name="input">The input to check the form and type of.</param>
        /// <returns>Whether the IssuedCurrencyAmount is malformed.</returns>
        public static bool IsIssuedCurrency(dynamic input)
        {
            return (
                IsRecord(input) &&
                input.Count == ISSUED_CURRENCY_SIZE &&
                input["value"] is string &&
                input["issuer"] is string &&
                input["currency"] is string
            );
        }
        /// <summary>
        /// Verify the form and type of an Amount at runtime.
        /// </summary>
        /// <param name="amount">The object to check the form and type of.</param>
        /// <returns>Whether the Amount is malformed.</returns>
        public static bool IsAmount(dynamic amount)
        {
            return amount is string || IsIssuedCurrency(amount);
        }


        /// <summary>
        /// Verify the form and type of Signer at runtime.
        /// </summary>
        /// <param name="signer">The object to check the form and type of.</param>
        /// <returns>Whether the Signer is malformed.</returns>
        public static bool IsSigner(dynamic signer)
        {
            if (signer is not Dictionary<string, dynamic> { Count: SIGNER_SIZE } value)
                return false;

            return (value.TryGetValue("Account", out var account) && account is string { }) &&
                   (value.TryGetValue("TxnSignature", out var TxnSignature) && TxnSignature is string { }) &&
                   (value.TryGetValue("SigningPubKey", out var SigningPubKey) && SigningPubKey is string { });
        }
        /// <summary>
        /// Verify the form and type of Memo at runtime.
        /// </summary>
        /// <param name="memo">The object to check the form and type of.</param>
        /// <returns>Whether the Memo is malformed.</returns>
        public static bool IsMemo(dynamic memo)
        {
            if (memo is not Dictionary<string, dynamic> {  } value)
                return false;

            var size = value.Count;

            var valid_data = value.TryGetValue("MemoData", out var MemoData) || MemoData is string;
            var valid_format = value.TryGetValue("MemoFormat", out var MemoFormat) || MemoFormat is string;
            var valid_type = value.TryGetValue("MemoType", out var MemoType) || MemoType is string;
            return size is >= 1 and <= MEMO_SIZE && valid_data && valid_format && valid_type
                   && value.OnlyHasFields(new[] { "MemoFormat", "MemoData", "MemoType" });
        }


        /// <summary>
        /// Parse the value of an amount, expressed either in XRP or as an Issued Currency, into a number.
        /// </summary>
        /// <param name="amount"> An Amount to parse for its value.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">The parsed amount value, or null if the amount count not be parsed.</exception>
        public static double ParseAmountValue(dynamic amount)
        {
            if (!Common.IsAmount(amount))
            {
                return double.NaN;
            }
            if (amount is string)
            {
                return double.Parse(
                    amount, NumberStyles.AllowLeadingSign
                                 | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                                 | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                 | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                 | NumberStyles.AllowExponent
                                 | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture);
            }

            return double.Parse(
                amount.value, NumberStyles.AllowLeadingSign
                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                             | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                             | NumberStyles.AllowExponent
                             | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Verify the form and type of an Issue at runtime.
        /// </summary>
        /// <param name="input">The object to check the form and type of.</param>
        /// <returns>Whether the Issue is malformed.</returns>
        public static bool IsIssue(dynamic input)
        {
            if (!IsRecord(input))
                return false;
            if (input is not Dictionary<string, dynamic> issue)
                return false;


            var length = issue.Count;
            issue.TryGetValue("currency", out var currency);
            issue.TryGetValue("issuer", out var issuer);
            return (length == 1 && currency == "XRP") || (length == 2 && currency is string && issuer is string);
        }
        /// <summary>
        /// Verify the common fields of a transaction.<br/>
        /// The validate functionality will be optional, and will check transaction form at runtime.
        /// This should be called any time a transaction will be verified.
        /// </summary>
        /// <param name="tx">An interface w/ common transaction fields.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> When the common param is malformed.</exception>
        public static Task ValidateBaseTransaction(Dictionary<string, dynamic> tx)
        {
            if (!tx.TryGetValue("Account", out var Account) || Account is null)
            {
                throw new ValidationException("BaseTransaction: missing field Account");
            }

            if (Account is not string { })
            {
                throw new ValidationException("BaseTransaction: Account not string");
            }

            if (!tx.TryGetValue("TransactionType", out var TransactionType) || TransactionType is null)
            {
                throw new ValidationException("BaseTransaction: missing field TransactionType");
            }

            if (TransactionType is not string)
            {
                throw new ValidationException("BaseTransaction: TransactionType not string");
            }


            if (Enum.GetValues<TransactionType>().All(type => type.ToString() != $"{TransactionType}"))
            {
                throw new ValidationException("BaseTransaction: Unknown TransactionType");
            }

            if (tx.TryGetValue("Fee", out var Fee) && Fee is not string { })
            {
                throw new ValidationException("BaseTransaction: invalid Fee");
            }

            if (tx.TryGetValue("Sequence", out var Sequence) && Sequence is not uint { })
            {
                throw new ValidationException("BaseTransaction: invalid Sequence");
            }
            if (tx.TryGetValue("AccountTxnID", out var AccountTxnID) && AccountTxnID is not string { })
            {
                throw new ValidationException("BaseTransaction: invalid AccountTxnID");
            }
            if (tx.TryGetValue("LastLedgerSequence", out var LastLedgerSequence) && LastLedgerSequence is not uint { })
            {
                throw new ValidationException("BaseTransaction: invalid LastLedgerSequence");
            }

            // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- Only used by JS
            tx.TryGetValue("Memos", out var Memos);
            if (Memos is not null)
            {
                if (Memos is not IEnumerable<dynamic> { } memos)
                    throw new ValidationException("BaseTransaction: invalid Memos");

                if (memos.Any(memo => !Common.IsMemo(memo)))
                {
                    throw new ValidationException("BaseTransaction: invalid Memos");
                }
            }

            // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- Only used by JS
            tx.TryGetValue("Signers", out var Signers);

            if (Signers is not null)
            {
                if (Signers is not List<dynamic> signers)
                    throw new ValidationException("BaseTransaction: invalid Signers");

                if (signers.ToArray().Length == 0)
                    throw new ValidationException("BaseTransaction: invalid Signers");

                if (signers.Any(signer => !Common.IsSigner(signer)))
                {
                    throw new ValidationException("BaseTransaction: invalid Signers");
                }

            }

            if (tx.TryGetValue("SourceTag", out var SourceTag) && SourceTag is not uint { })
            {
                throw new ValidationException("BaseTransaction: invalid SourceTag");
            }
            if (tx.TryGetValue("SigningPubKey", out var SigningPubKey) && SigningPubKey is not string { })
            {
                throw new ValidationException("BaseTransaction: invalid SigningPubKey");
            }
            if (tx.TryGetValue("TicketSequence", out var TicketSequence) && TicketSequence is not uint { })
            {
                throw new ValidationException("BaseTransaction: invalid TicketSequence");
            }
            if (tx.TryGetValue("TxnSignature", out var TxnSignature) && TxnSignature is not string { })
            {
                throw new ValidationException("BaseTransaction: invalid TxnSignature");
            }
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc />
    [JsonConverter(typeof(TransactionConverter))]
    public abstract class TransactionCommon : ITransactionCommon //todo rename to BaseTransaction
    {
        //protected TransactionCommon()
        //{
        //    Fee = new Currency { Value = "10" };
        //}

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

        /// <summary>
        /// The date/time when this transaction was included in a validated ledger.
        /// </summary>
        [JsonProperty("date")]
        public uint? date { get; set; } //todo not unknown field
                                        //possible
                                        //https://github.com/XRPLF/xrpl.js/blob/984a58e642a4cde09aee320efe195d4e651b7733/packages/xrpl/src/models/common/index.ts#L98


        [JsonProperty("inLedger")]
        public uint? inLedger { get; set; } //todo not unknown field

        /// <summary>
        /// The sequence number of the ledger that included this transaction.
        /// </summary>
        [JsonProperty("ledger_index")]
        public uint? ledger_index { get; set; } //todo not unknown field

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

    /// <summary>
    /// Additional arbitrary information used to identify transaction.
    /// </summary>
    public class MemoWrapper
    {
        /// <summary>
        /// The Memos field includes arbitrary messaging data with the transaction.
        /// </summary>
        [JsonProperty("Memo")]
        public Memo Memo { get; set; }
    }

    /// <summary>
    /// The Memos field includes arbitrary messaging data with the transaction.
    /// </summary>
    public class Memo
    {
        /// <summary>
        /// Arbitrary hex value, conventionally containing the content of the memo.
        /// </summary>
        public string MemoData { get; set; }

        /// <summary>
        /// Content of the memo.
        /// </summary>
        [JsonIgnore]
        public string MemoDataAsText => MemoData.FromHexString();
        /// <summary>
        /// Hex value representing characters allowed in URLs.<br/>
        /// Conventionally containing information on how the memo is encoded, for example as a MIME type.
        /// </summary>
        public string MemoFormat { get; set; }
        /// <summary>
        /// Conventionally containing information on how the memo is encoded, for example as a MIME type.
        /// </summary>
        [JsonIgnore]
        public string MemoFormatAsText => MemoFormat.FromHexString();

        /// <summary>
        /// Hex value representing characters allowed in URLs.<br/>
        /// Conventionally, a unique relation (according to RFC 5988 ) that defines the format of this memo.
        /// </summary>
        public string MemoType { get; set; }

        /// <summary>
        /// Conventionally, a unique relation (according to RFC 5988 ) that defines the format of this memo.
        /// </summary>
        [JsonIgnore]
        public string MemoTypeAsText => MemoType.FromHexString();
    }

    /// <summary>
    /// The Signers field contains a multi-signature, which has signatures from up to 8 key pairs, that together should authorize the transaction.
    /// </summary>
    public class Signer
    {
        /// <summary>
        /// The address associated with this signature, as it appears in the signer list.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// A signature for this transaction, verifiable using the SigningPubKey.
        /// </summary>
        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        /// <summary>
        /// The public key used to create this signature.
        /// </summary>
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

    /// <summary>
    /// transaction object
    /// </summary>
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
        /// JSON
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
        public dynamic FinalFields { get; set; }
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
        public BaseLedgerEntry Final => LOConverter.GetBaseRippleLO(LedgerEntryType, FinalFields);

        /// <summary>
        /// JSON
        /// CreatedNode <br/>
        /// The content fields of the newly-created ledger object.<br/>
        /// Which fields are present depends on what type of ledger object was created.
        /// </summary>
        public dynamic NewFields { get; set; }
        /// <summary>
        /// CreatedNode <br/>
        /// The content fields of the newly-created ledger object.<br/>
        /// Which fields are present depends on what type of ledger object was created.
        /// </summary>
        public BaseLedgerEntry New => LOConverter.GetBaseRippleLO(LedgerEntryType, NewFields);

        /// <summary>
        /// JSON
        /// ModifiedNode <br/>
        /// The previous values for all fields of the object that were changed as a result of this transaction.<br/>
        /// If the transaction only added fields to the object, this field is an empty object.
        /// </summary>
        public dynamic PreviousFields { get; set; }
        /// <summary>
        /// ModifiedNode <br/>
        /// The previous values for all fields of the object that were changed as a result of this transaction.<br/>
        /// If the transaction only added fields to the object, this field is an empty object.
        /// </summary>
        public BaseLedgerEntry Previous => LOConverter.GetBaseRippleLO(LedgerEntryType, PreviousFields);

    }
    //https://xrpl.org/transaction-common-fields.html
    /// <summary>
    /// Every transaction has the same set of common fields.
    /// </summary>
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
        //todo not found fields - SourceTag: Number (UInt32), TicketSequence:Number(UInt32), TxnSignature:string
    }

    /// <summary>
    /// Every transaction has the same set of common fields.
    /// </summary>
    public interface ITransactionResponseCommon : IBaseTransactionResponse, ITransactionCommon
    {
    }

    /// <inheritdoc cref="ITransactionResponseCommon" />
    [JsonConverter(typeof(TransactionConverter))]
    public abstract class TransactionResponseCommon : BaseTransactionResponse, ITransactionResponseCommon
    {
        /// <inheritdoc/>
        public string Account { get; set; }

        /// <inheritdoc/>
        public string AccountTxnID { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Fee { get; set; }

        /// <inheritdoc/>
        public uint? Flags { get; set; }

        /// <inheritdoc/>
        public uint? LastLedgerSequence { get; set; }

        /// <inheritdoc/>
        public List<Memo> Memos { get; set; }
        /// <inheritdoc/>
        public uint? Sequence { get; set; }
        /// <inheritdoc/>
        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }

        /// <inheritdoc/>
        public List<Signer> Signers { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        /// <inheritdoc/>
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        /// <inheritdoc/>
        public string ToJson()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}
