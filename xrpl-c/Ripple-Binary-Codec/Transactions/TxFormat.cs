using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;
using Ripple.Core.Enums;
using Ripple.Core.Types;

namespace Ripple.Core.Transactions
{
    //https://github.com/ripple/rippled/blob/112a863e7346793234c973e97818ced4c6e36867/src/ripple/protocol/impl/TxFormats.cpp
    public class TxFormat : Dictionary<Field, TxFormat.Requirement>
    {
        public enum Requirement
        {
            Default,
            Optional,
            Required
        }

        public TxFormat()
        {
            // Common fields
            this[Field.TransactionType] = Requirement.Required;
            this[Field.Account] = Requirement.Required;
            this[Field.Fee] = Requirement.Required;
            this[Field.Sequence] = Requirement.Required;
            this[Field.SigningPubKey] = Requirement.Required;

            this[Field.Flags] = Requirement.Optional;
            this[Field.SourceTag] = Requirement.Optional;
            this[Field.PreviousTxnID] = Requirement.Optional;
            this[Field.LastLedgerSequence] = Requirement.Optional;
            this[Field.AccountTxnID] = Requirement.Optional;
            this[Field.OperationLimit] = Requirement.Optional;
            this[Field.Memos] = Requirement.Optional;
            this[Field.TxnSignature] = Requirement.Optional;
            this[Field.Signers] = Requirement.Optional;
        }

        public static void Validate(StObject obj)
        {
            var errors = new List<string>();
            Validate(obj, errors.Add);
            if (errors.Count > 0)
            {
                throw new TxFormatValidationException(string.Join("\n", errors));
            }
        }

        internal static void Validate(StObject obj, Action<string> onError)
        {
            if (!obj.Has(Field.TransactionType))
            {
                onError("Missing `TransactionType` field");
                return;
            }

            var tt = obj[Field.TransactionType];
            if (tt == null)
            {
                onError("`TransactionType` is set to null");
                return;
            }

            var format = Formats[tt];
            var allFields = new SortedSet<Field>(obj.Fields.Keys);
            allFields.UnionWith(format.Keys);

            foreach (var field in allFields)
            {
                var inFormat = format.ContainsKey(field);
                ISerializedType fieldValue;
                var inObject = obj.Fields.TryGetValue(field, out fieldValue);
                if (!inFormat)
                {
                    onError($"`{tt}` has no `{field}` field");
                }
                else if (format[field] == Requirement.Required)
                {
                    if (!inObject)
                    {
                        onError($"`{tt}` has required field `{field}`");
                    }
                    else if (fieldValue == null)
                    {
                        onError($"Required field `{field}` is set to null");
                    }
                    // TODO: associated type for field is wrong
                    // It should be nearly impossible anyway because FromJson
                    // throws when the json is invalid for the field type and
                    // the StObject[] indexers all use typed fields externally
                }
            }
        }

        public static Dictionary<TransactionType, TxFormat> Formats;

        static TxFormat()
        {
            Formats = new Dictionary<TransactionType, TxFormat>
            {
                [TransactionType.AccountSet] = new TxFormat
                {
                    [Field.EmailHash] = Requirement.Optional,
                    [Field.WalletLocator] = Requirement.Optional,
                    [Field.WalletSize] = Requirement.Optional,
                    [Field.MessageKey] = Requirement.Optional,
                    [Field.Domain] = Requirement.Optional,
                    [Field.TransferRate] = Requirement.Optional,
                    [Field.SetFlag] = Requirement.Optional,
                    [Field.ClearFlag] = Requirement.Optional,
                    [Field.TickSize] = Requirement.Optional
                },
                [TransactionType.TrustSet] = new TxFormat
                {
                    [Field.LimitAmount] = Requirement.Optional,
                    [Field.QualityIn] = Requirement.Optional,
                    [Field.QualityOut] = Requirement.Optional
                },
                [TransactionType.OfferCreate] = new TxFormat
                {                    
                    [Field.TakerPays] = Requirement.Required,
                    [Field.TakerGets] = Requirement.Required,
                    [Field.Expiration] = Requirement.Optional,
                    [Field.OfferSequence] = Requirement.Optional
                },
                [TransactionType.OfferCancel] = new TxFormat
                {                    
                    [Field.OfferSequence] = Requirement.Required
                },
                [TransactionType.SetRegularKey] = new TxFormat
                {
                    [Field.RegularKey] = Requirement.Optional
                },
                [TransactionType.Payment] = new TxFormat
                {                    
                    [Field.Destination] = Requirement.Required,
                    [Field.Amount] = Requirement.Required,
                    [Field.SendMax] = Requirement.Optional,
                    [Field.Paths] = Requirement.Default,
                    [Field.InvoiceID] = Requirement.Optional,
                    [Field.DestinationTag] = Requirement.Optional,
                    [Field.DeliverMin] = Requirement.Optional
                },

                [TransactionType.EscrowCreate] = new TxFormat
                {
                    [Field.Amount] = Requirement.Required,
                    [Field.Destination] = Requirement.Required,
                    [Field.Condition] = Requirement.Optional,
                    [Field.CancelAfter] = Requirement.Optional,
                    [Field.FinishAfter] = Requirement.Optional,                    
                    [Field.DestinationTag] = Requirement.Optional,                    
                },
                [TransactionType.EscrowCancel] = new TxFormat
                {
                    [Field.Owner] = Requirement.Required,
                    [Field.OfferSequence] = Requirement.Required
                },
                [TransactionType.EscrowFinish] = new TxFormat
                {
                    [Field.Owner] = Requirement.Required,
                    [Field.OfferSequence] = Requirement.Required,
                    [Field.Condition] = Requirement.Optional,
                    [Field.Fulfillment] = Requirement.Optional
                },
                [TransactionType.EnableAmendment] = new TxFormat
                {
                    [Field.LedgerSequence] = Requirement.Optional,
                    [Field.Amendment] = Requirement.Required
                },
                [TransactionType.SetFee] = new TxFormat
                {
                    [Field.LedgerSequence] = Requirement.Optional,
                    [Field.BaseFee] = Requirement.Required,
                    [Field.ReferenceFeeUnits] = Requirement.Required,
                    [Field.ReserveBase] = Requirement.Required,
                    [Field.ReserveIncrement] = Requirement.Required
                },  
                [TransactionType.TicketCreate] = new TxFormat
                {
                    [Field.Target] = Requirement.Optional,
                    [Field.Expiration] = Requirement.Optional
                },
                [TransactionType.TicketCancel] = new TxFormat
                {
                    [Field.TicketID] = Requirement.Required
                },
                // The SignerEntries are optional because a SignerList is deleted by
                // setting the SignerQuorum to zero and omitting SignerEntries.
                [TransactionType.SignerListSet] = new TxFormat
                {
                    [Field.SignerQuorum] = Requirement.Required,
                    [Field.SignerEntries] = Requirement.Optional
                },
                [TransactionType.PaymentChannelCreate] = new TxFormat()
                {
                    [Field.Destination] = Requirement.Required,
                    [Field.Amount] = Requirement.Required,
                    [Field.SettleDelay] = Requirement.Required,
                    [Field.PublicKey] = Requirement.Required,
                    [Field.CancelAfter] = Requirement.Optional,
                    [Field.DestinationTag] = Requirement.Optional
                },
                [TransactionType.PaymentChannelFund] = new TxFormat()
                {
                    [Field.Channel] = Requirement.Required,
                    [Field.Amount] = Requirement.Required,
                    [Field.Expiration] = Requirement.Optional
                },
                [TransactionType.PaymentChannelClaim] = new TxFormat()
                {
                    [Field.Channel] = Requirement.Required,
                    [Field.Amount] = Requirement.Optional,
                    [Field.Balance] = Requirement.Optional,
                    [Field.Signature] = Requirement.Optional,
                    [Field.PublicKey] = Requirement.Optional
                }
            };
        }
    }

    public class TxFormatValidationException : FormatException
    {
        public TxFormatValidationException(string msg) : base(msg)
        {
        }
    }
}
