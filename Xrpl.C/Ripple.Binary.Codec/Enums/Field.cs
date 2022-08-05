using System;

namespace Ripple.Binary.Codec.Enums
{
    public class Field : EnumItem
    {
        #region members
        public readonly bool IsSigningField;
        public readonly bool IsSerialised;
        public readonly bool IsVlEncoded;
        public readonly int NthOfType;
        public readonly FieldType Type;
        public readonly byte[] Header;

        public FromJson FromJson;
        public FromParser FromParser;

        #endregion

        public static readonly Enumeration<Field> Values = new();
        public Field(string name,
            int nthOfType,
            FieldType type,
            bool isSigningField=true,
            bool isSerialised=true) :
                base(name,
                    (type.Ordinal << 16 | nthOfType))
        {
            // catch auxiliary fields
            var valid = (nthOfType > 0) && nthOfType < 256 &&
                        type.Ordinal > 0 && type.Ordinal < 256;
            Type = type;
            IsSigningField = valid && isSigningField;
            IsSerialised = valid && isSerialised;
            NthOfType = nthOfType;
            IsVlEncoded = IsVlEncodedType();
            Header = CalculateHeader();
            Values.AddEnum(this);
        }

        public static implicit operator Field(string s)
        {
            return Values[s];
        }

        private byte[] CalculateHeader()
        {
            var nth = NthOfType;
            var type = Type.Ordinal;

            if (type < 16)
            {
                if (nth < 16) // common type, common name
                    return new [] {(byte) ((type << 4) | nth)};
                // common type, uncommon name
                return new[] {(byte) (type << 4), (byte) nth};
            }
            if (nth < 16)
                // uncommon type, common name
                return new[] {(byte) nth, (byte) type};
            // uncommon type, uncommon name
            return new byte[] {0, (byte) type, (byte) nth};
        }

        private bool IsVlEncodedType()
        {
            return Type == FieldType.Vector256 ||
                   Type == FieldType.Blob ||
                   Type == FieldType.AccountId;
        }


        public static readonly Field Generic = new(nameof(Generic), 0, FieldType.Unknown);
        public static readonly Field Invalid = new(nameof(Invalid), -1, FieldType.Unknown);

        public static readonly LedgerEntryTypeField LedgerEntryType = new(nameof(LedgerEntryType), 1);
        public static readonly TransactionTypeField TransactionType = new(nameof(TransactionType), 2);
        public static readonly Uint16Field SignerWeight = new(nameof(SignerWeight), 3);
        public static readonly Uint16Field TransferFee = new(nameof(TransferFee), 4);

        public static readonly Uint32Field Flags = new(nameof(Flags), 2);
        public static readonly Uint32Field SourceTag = new(nameof(SourceTag), 3);
        public static readonly Uint32Field Sequence = new(nameof(Sequence), 4);
        public static readonly Uint32Field PreviousTxnLgrSeq = new(nameof(PreviousTxnLgrSeq), 5);
        public static readonly Uint32Field LedgerSequence = new(nameof(LedgerSequence), 6);
        public static readonly Uint32Field CloseTime = new(nameof(CloseTime), 7);
        public static readonly Uint32Field ParentCloseTime = new(nameof(ParentCloseTime), 8);
        public static readonly Uint32Field SigningTime = new(nameof(SigningTime), 9);
        public static readonly Uint32Field Expiration = new(nameof(Expiration), 10);
        public static readonly Uint32Field TransferRate = new(nameof(TransferRate), 11);
        public static readonly Uint32Field WalletSize = new(nameof(WalletSize), 12);
        public static readonly Uint32Field OwnerCount = new(nameof(OwnerCount), 13);
        public static readonly Uint32Field DestinationTag = new(nameof(DestinationTag), 14);

        public static readonly Uint32Field HighQualityIn = new(nameof(HighQualityIn), 16);
        public static readonly Uint32Field HighQualityOut = new(nameof(HighQualityOut), 17);
        public static readonly Uint32Field LowQualityIn = new(nameof(LowQualityIn), 18);
        public static readonly Uint32Field LowQualityOut = new(nameof(LowQualityOut), 19);
        public static readonly Uint32Field QualityIn = new(nameof(QualityIn), 20);
        public static readonly Uint32Field QualityOut = new(nameof(QualityOut), 21);
        public static readonly Uint32Field StampEscrow = new(nameof(StampEscrow), 22);
        public static readonly Uint32Field BondAmount = new(nameof(BondAmount), 23);
        public static readonly Uint32Field LoadFee = new(nameof(LoadFee), 24);
        public static readonly Uint32Field OfferSequence = new(nameof(OfferSequence), 25);

        [Obsolete]
        public static readonly Uint32Field FirstLedgerSequence = new(nameof(FirstLedgerSequence), 26); // Deprecated: do not use;
        // Added new semantics in 9486fc416ca7c59b8930b734266eed4d5b714c50
        public static readonly Uint32Field LastLedgerSequence = new(nameof(LastLedgerSequence), 27);
        public static readonly Uint32Field TransactionIndex = new(nameof(TransactionIndex), 28);
        public static readonly Uint32Field OperationLimit = new(nameof(OperationLimit), 29);
        public static readonly Uint32Field ReferenceFeeUnits = new(nameof(ReferenceFeeUnits), 30);
        public static readonly Uint32Field ReserveBase = new(nameof(ReserveBase), 31);
        public static readonly Uint32Field ReserveIncrement = new(nameof(ReserveIncrement), 32);
        public static readonly Uint32Field SetFlag = new(nameof(SetFlag), 33);
        public static readonly Uint32Field ClearFlag = new(nameof(ClearFlag), 34);
        public static readonly Uint32Field SignerQuorum = new(nameof(SignerQuorum), 35);
        public static readonly Uint32Field CancelAfter = new(nameof(CancelAfter), 36);
        public static readonly Uint32Field FinishAfter = new(nameof(FinishAfter), 37);
        public static readonly Uint32Field SignerListID = new(nameof(SignerListID), 38);
        public static readonly Uint32Field SettleDelay = new(nameof(SettleDelay), 39);
        public static readonly Uint32Field TicketCount = new(nameof(TicketCount), 40);
        public static readonly Uint32Field TicketSequence = new(nameof(TicketSequence), 41);
        public static readonly Uint32Field NFTokenTaxon = new(nameof(NFTokenTaxon), 42);
        public static readonly Uint32Field MintedTokens = new(nameof(MintedTokens), 43);
        public static readonly Uint32Field BurnedTokens = new(nameof(BurnedTokens), 44);

        public static readonly Uint64Field IndexNext = new(nameof(IndexNext), 1);
        public static readonly Uint64Field IndexPrevious = new(nameof(IndexPrevious), 2);
        public static readonly Uint64Field BookNode = new(nameof(BookNode), 3);
        public static readonly Uint64Field OwnerNode = new(nameof(OwnerNode), 4);
        public static readonly Uint64Field BaseFee = new(nameof(BaseFee), 5);
        public static readonly Uint64Field ExchangeRate = new(nameof(ExchangeRate), 6);
        public static readonly Uint64Field LowNode = new(nameof(LowNode), 7);
        public static readonly Uint64Field HighNode = new(nameof(HighNode), 8);

        public static readonly Hash128Field EmailHash = new(nameof(EmailHash), 1);
        public static readonly Hash256Field LedgerHash = new(nameof(LedgerHash), 1);
        public static readonly Hash256Field ParentHash = new(nameof(ParentHash), 2);
        public static readonly Hash256Field TransactionHash = new(nameof(TransactionHash), 3);
        public static readonly Hash256Field AccountHash = new(nameof(AccountHash), 4);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field PreviousTxnID = new(nameof(PreviousTxnID), 5);
        public static readonly Hash256Field LedgerIndex = new(nameof(LedgerIndex), 6);
        public static readonly Hash256Field WalletLocator = new(nameof(WalletLocator), 7);
        public static readonly Hash256Field RootIndex = new(nameof(RootIndex), 8);
        // Added in rippled commit: 9486fc416ca7c59b8930b734266eed4d5b714c50
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field AccountTxnID = new(nameof(AccountTxnID), 9);
        public static readonly Hash256Field BookDirectory = new(nameof(BookDirectory), 16);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field InvoiceID = new(nameof(InvoiceID), 17);
        public static readonly Hash256Field Nickname = new(nameof(Nickname), 18);
        public static readonly Hash256Field Amendment = new(nameof(Amendment), 19);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field TicketID = new(nameof(TicketID), 20);
        public static readonly Hash256Field Digest = new(nameof(Digest), 21);
        public static readonly Hash256Field Channel = new(nameof(Channel), 22);
        public static readonly Hash256Field ConsensusHash = new(nameof(ConsensusHash), 23);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field hash = new(nameof(hash), 257);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field index = new(nameof(index), 258);
        public static readonly Hash256Field NFTokenID = new(nameof(NFTokenID), 10);
        public static readonly Hash256Field NFTokenBuyOffer = new(nameof(NFTokenBuyOffer), 28);
        public static readonly Hash256Field NFTokenSellOffer = new(nameof(NFTokenSellOffer), 29);

        public static readonly AmountField Amount = new(nameof(Amount), 1);
        public static readonly AmountField Balance = new(nameof(Balance), 2);
        public static readonly AmountField LimitAmount = new(nameof(LimitAmount), 3);
        public static readonly AmountField TakerPays = new(nameof(TakerPays), 4);
        public static readonly AmountField TakerGets = new(nameof(TakerGets), 5);
        public static readonly AmountField LowLimit = new(nameof(LowLimit), 6);
        public static readonly AmountField HighLimit = new(nameof(HighLimit), 7);
        public static readonly AmountField Fee = new(nameof(Fee), 8);
        public static readonly AmountField SendMax = new(nameof(SendMax), 9);
        public static readonly AmountField DeliverMin = new(nameof(DeliverMin), 10);
        public static readonly AmountField MinimumOffer = new(nameof(MinimumOffer), 16);
        public static readonly AmountField RippleEscrow = new(nameof(RippleEscrow), 17);

        // Added in rippled commit: e7f0b8eca69dd47419eee7b82c8716b3aa5a9e39
        public static readonly AmountField DeliveredAmount = new(nameof(DeliveredAmount), 18);
        // Added in rippled commit: e7f0b8eca69dd47419eee7b82c8716b3aa5a9e39
        public static readonly AmountField BrokerFee = new(nameof(BrokerFee), 19);

        // These are auxiliary fields
        // ReSharper disable once InconsistentNaming
        public static readonly AmountField taker_gets_funded = new(nameof(taker_gets_funded), 258);
        // ReSharper disable once InconsistentNaming
        public static readonly AmountField taker_pays_funded = new(nameof(taker_pays_funded), 259);

        public static readonly BlobField PublicKey = new(nameof(PublicKey), 1);
        public static readonly BlobField MessageKey = new(nameof(MessageKey), 2);
        public static readonly BlobField SigningPubKey = new(nameof(SigningPubKey), 3);

        // ReSharper disable once RedundantArgumentNameForLiteralExpression
        public static readonly BlobField TxnSignature = new(nameof(TxnSignature), 4, isSigningField:false);
        public static readonly BlobField URI = new(nameof(URI), 5);
        public static readonly BlobField Signature = new(nameof(Signature), 6);
        public static readonly BlobField Domain = new(nameof(Domain), 7);
        public static readonly BlobField FundCode = new(nameof(FundCode), 8);
        public static readonly BlobField RemoveCode = new(nameof(RemoveCode), 9);
        public static readonly BlobField ExpireCode = new(nameof(ExpireCode), 10);
        public static readonly BlobField CreateCode = new(nameof(CreateCode), 11);
        public static readonly BlobField MemoType = new(nameof(MemoType), 12);
        public static readonly BlobField MemoData = new(nameof(MemoData), 13);
        public static readonly BlobField MemoFormat = new(nameof(MemoFormat), 14);
        public static readonly BlobField Fulfillment = new(nameof(Fulfillment), 16);
        public static readonly BlobField Condition = new(nameof(Condition), 17);
        public static readonly BlobField MasterSignature = new(nameof(MasterSignature), 18);

        public static readonly AccountIdField Account = new(nameof(Account), 1);
        public static readonly AccountIdField Owner = new(nameof(Owner), 2);
        public static readonly AccountIdField Destination = new(nameof(Destination), 3);
        public static readonly AccountIdField Issuer = new(nameof(Issuer), 4);
        public static readonly AccountIdField Target = new(nameof(Target), 7);
        public static readonly AccountIdField RegularKey = new(nameof(RegularKey), 8);

        public static readonly StObjectField ObjectEndMarker = new(nameof(ObjectEndMarker), 1);
        public static readonly StObjectField TransactionMetaData = new(nameof(TransactionMetaData), 2);
        public static readonly StObjectField CreatedNode = new(nameof(CreatedNode), 3);
        public static readonly StObjectField DeletedNode = new(nameof(DeletedNode), 4);
        public static readonly StObjectField ModifiedNode = new(nameof(ModifiedNode), 5);
        public static readonly StObjectField PreviousFields = new(nameof(PreviousFields), 6);
        public static readonly StObjectField FinalFields = new(nameof(FinalFields), 7);
        public static readonly StObjectField NewFields = new(nameof(NewFields), 8);
        public static readonly StObjectField TemplateEntry = new(nameof(TemplateEntry), 9);
        public static readonly StObjectField Memo = new(nameof(Memo), 10);
        public static readonly StObjectField SignerEntry = new(nameof(SignerEntry), 11);
        public static readonly StObjectField Signer = new(nameof(Signer), 16);
        public static readonly StObjectField Majority = new(nameof(Majority), 18);

        public static readonly StArrayField ArrayEndMarker = new(nameof(ArrayEndMarker), 1);
        // ReSharper disable once RedundantArgumentNameForLiteralExpression
        public static readonly StArrayField Signers = new(nameof(Signers), 3, isSigningField:false);
        public static readonly StArrayField SignerEntries = new(nameof(SignerEntries), 4);
        public static readonly StArrayField Template = new(nameof(Template), 5);
        public static readonly StArrayField Necessary = new(nameof(Necessary), 6);
        public static readonly StArrayField Sufficient = new(nameof(Sufficient), 7);
        public static readonly StArrayField AffectedNodes = new(nameof(AffectedNodes), 8);
        public static readonly StArrayField Memos = new(nameof(Memos), 9);

        public static readonly Uint8Field CloseResolution = new(nameof(CloseResolution), 1);
        public static readonly Uint8Field Method = new(nameof(Method), 2);
        public static readonly EngineResultField TransactionResult = new(nameof(TransactionResult), 3);

        //uncommon
        public static readonly Uint8Field TickSize = new(nameof(TickSize), 16);

        public static readonly Hash160Field TakerPaysCurrency = new(nameof(TakerPaysCurrency), 1);
        public static readonly Hash160Field TakerPaysIssuer = new(nameof(TakerPaysIssuer), 2);
        public static readonly Hash160Field TakerGetsCurrency = new(nameof(TakerGetsCurrency), 3);
        public static readonly Hash160Field TakerGetsIssuer = new(nameof(TakerGetsIssuer), 4);

        public static readonly PathSetField Paths = new(nameof(Paths), 1);

        public static readonly Vector256Field Indexes = new(nameof(Indexes), 1);
        public static readonly Vector256Field Hashes = new(nameof(Hashes), 2);
        public static readonly Vector256Field Features = new(nameof(Features), 3);
        public static readonly Vector256Field NFTokenOffers = new(nameof(NFTokenOffers), 4);

        public static readonly Field Transaction = new(nameof(Transaction), 1, FieldType.Transaction);
        public static readonly Field LedgerEntry = new(nameof(LedgerEntry), 1, FieldType.LedgerEntry);
        public static readonly Field Validation = new(nameof(Validation), 1, FieldType.Validation);
    }
}