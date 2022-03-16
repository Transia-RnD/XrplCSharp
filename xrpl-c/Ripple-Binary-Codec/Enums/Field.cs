using System;

namespace Ripple.Core.Enums
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

        public static readonly Enumeration<Field> Values = new Enumeration<Field>();
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


        public static readonly Field Generic = new Field(nameof(Generic), 0, FieldType.Unknown);
        public static readonly Field Invalid = new Field(nameof(Invalid), -1, FieldType.Unknown);

        public static readonly LedgerEntryTypeField LedgerEntryType = new LedgerEntryTypeField(nameof(LedgerEntryType), 1);
        public static readonly TransactionTypeField TransactionType = new TransactionTypeField(nameof(TransactionType), 2);
        public static readonly Uint16Field SignerWeight = new Uint16Field(nameof(SignerWeight), 3);

        public static readonly Uint32Field Flags = new Uint32Field(nameof(Flags), 2);
        public static readonly Uint32Field SourceTag = new Uint32Field(nameof(SourceTag), 3);
        public static readonly Uint32Field Sequence = new Uint32Field(nameof(Sequence), 4);
        public static readonly Uint32Field PreviousTxnLgrSeq = new Uint32Field(nameof(PreviousTxnLgrSeq), 5);
        public static readonly Uint32Field LedgerSequence = new Uint32Field(nameof(LedgerSequence), 6);
        public static readonly Uint32Field CloseTime = new Uint32Field(nameof(CloseTime), 7);
        public static readonly Uint32Field ParentCloseTime = new Uint32Field(nameof(ParentCloseTime), 8);
        public static readonly Uint32Field SigningTime = new Uint32Field(nameof(SigningTime), 9);
        public static readonly Uint32Field Expiration = new Uint32Field(nameof(Expiration), 10);
        public static readonly Uint32Field TransferRate = new Uint32Field(nameof(TransferRate), 11);
        public static readonly Uint32Field WalletSize = new Uint32Field(nameof(WalletSize), 12);
        public static readonly Uint32Field OwnerCount = new Uint32Field(nameof(OwnerCount), 13);
        public static readonly Uint32Field DestinationTag = new Uint32Field(nameof(DestinationTag), 14);

        public static readonly Uint32Field HighQualityIn = new Uint32Field(nameof(HighQualityIn), 16);
        public static readonly Uint32Field HighQualityOut = new Uint32Field(nameof(HighQualityOut), 17);
        public static readonly Uint32Field LowQualityIn = new Uint32Field(nameof(LowQualityIn), 18);
        public static readonly Uint32Field LowQualityOut = new Uint32Field(nameof(LowQualityOut), 19);
        public static readonly Uint32Field QualityIn = new Uint32Field(nameof(QualityIn), 20);
        public static readonly Uint32Field QualityOut = new Uint32Field(nameof(QualityOut), 21);
        public static readonly Uint32Field StampEscrow = new Uint32Field(nameof(StampEscrow), 22);
        public static readonly Uint32Field BondAmount = new Uint32Field(nameof(BondAmount), 23);
        public static readonly Uint32Field LoadFee = new Uint32Field(nameof(LoadFee), 24);
        public static readonly Uint32Field OfferSequence = new Uint32Field(nameof(OfferSequence), 25);

        [Obsolete]
        public static readonly Uint32Field FirstLedgerSequence = new Uint32Field(nameof(FirstLedgerSequence), 26); // Deprecated: do not use;
        // Added new semantics in 9486fc416ca7c59b8930b734266eed4d5b714c50
        public static readonly Uint32Field LastLedgerSequence = new Uint32Field(nameof(LastLedgerSequence), 27);
        public static readonly Uint32Field TransactionIndex = new Uint32Field(nameof(TransactionIndex), 28);
        public static readonly Uint32Field OperationLimit = new Uint32Field(nameof(OperationLimit), 29);
        public static readonly Uint32Field ReferenceFeeUnits = new Uint32Field(nameof(ReferenceFeeUnits), 30);
        public static readonly Uint32Field ReserveBase = new Uint32Field(nameof(ReserveBase), 31);
        public static readonly Uint32Field ReserveIncrement = new Uint32Field(nameof(ReserveIncrement), 32);
        public static readonly Uint32Field SetFlag = new Uint32Field(nameof(SetFlag), 33);
        public static readonly Uint32Field ClearFlag = new Uint32Field(nameof(ClearFlag), 34);
        public static readonly Uint32Field SignerQuorum = new Uint32Field(nameof(SignerQuorum), 35);
        public static readonly Uint32Field CancelAfter = new Uint32Field(nameof(CancelAfter), 36);
        public static readonly Uint32Field FinishAfter = new Uint32Field(nameof(FinishAfter), 37);
        public static readonly Uint32Field SignerListID = new Uint32Field(nameof(SignerListID), 38);
        public static readonly Uint32Field SettleDelay = new Uint32Field(nameof(SettleDelay), 39);
        public static readonly Uint32Field TicketCount = new Uint32Field(nameof(TicketCount), 40);
        public static readonly Uint32Field TicketSequence = new Uint32Field(nameof(TicketSequence), 41);
        public static readonly Uint32Field TokenTaxon = new Uint32Field(nameof(TokenTaxon), 42);
        public static readonly Uint32Field MintedTokens = new Uint32Field(nameof(MintedTokens), 43);
        public static readonly Uint32Field BurnedTokens = new Uint32Field(nameof(BurnedTokens), 44);

        public static readonly Uint64Field IndexNext = new Uint64Field(nameof(IndexNext), 1);
        public static readonly Uint64Field IndexPrevious = new Uint64Field(nameof(IndexPrevious), 2);
        public static readonly Uint64Field BookNode = new Uint64Field(nameof(BookNode), 3);
        public static readonly Uint64Field OwnerNode = new Uint64Field(nameof(OwnerNode), 4);
        public static readonly Uint64Field BaseFee = new Uint64Field(nameof(BaseFee), 5);
        public static readonly Uint64Field ExchangeRate = new Uint64Field(nameof(ExchangeRate), 6);
        public static readonly Uint64Field LowNode = new Uint64Field(nameof(LowNode), 7);
        public static readonly Uint64Field HighNode = new Uint64Field(nameof(HighNode), 8);

        public static readonly Hash128Field EmailHash = new Hash128Field(nameof(EmailHash), 1);

        public static readonly Hash256Field LedgerHash = new Hash256Field(nameof(LedgerHash), 1);
        public static readonly Hash256Field ParentHash = new Hash256Field(nameof(ParentHash), 2);
        public static readonly Hash256Field TransactionHash = new Hash256Field(nameof(TransactionHash), 3);
        public static readonly Hash256Field AccountHash = new Hash256Field(nameof(AccountHash), 4);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field PreviousTxnID = new Hash256Field(nameof(PreviousTxnID), 5);
        public static readonly Hash256Field LedgerIndex = new Hash256Field(nameof(LedgerIndex), 6);
        public static readonly Hash256Field WalletLocator = new Hash256Field(nameof(WalletLocator), 7);
        public static readonly Hash256Field RootIndex = new Hash256Field(nameof(RootIndex), 8);
        // Added in rippled commit: 9486fc416ca7c59b8930b734266eed4d5b714c50
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field AccountTxnID = new Hash256Field(nameof(AccountTxnID), 9);
        public static readonly Hash256Field BookDirectory = new Hash256Field(nameof(BookDirectory), 16);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field InvoiceID = new Hash256Field(nameof(InvoiceID), 17);
        public static readonly Hash256Field Nickname = new Hash256Field(nameof(Nickname), 18);
        public static readonly Hash256Field Amendment = new Hash256Field(nameof(Amendment), 19);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field TicketID = new Hash256Field(nameof(TicketID), 20);
        public static readonly Hash256Field Digest = new Hash256Field(nameof(Digest), 21);
        public static readonly Hash256Field Channel = new Hash256Field(nameof(Channel), 22);
        public static readonly Hash256Field ConsensusHash = new Hash256Field(nameof(ConsensusHash), 23);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field hash = new Hash256Field(nameof(hash), 257);
        // ReSharper disable once InconsistentNaming
        public static readonly Hash256Field index = new Hash256Field(nameof(index), 258);

        public static readonly AmountField Amount = new AmountField(nameof(Amount), 1);
        public static readonly AmountField Balance = new AmountField(nameof(Balance), 2);
        public static readonly AmountField LimitAmount = new AmountField(nameof(LimitAmount), 3);
        public static readonly AmountField TakerPays = new AmountField(nameof(TakerPays), 4);
        public static readonly AmountField TakerGets = new AmountField(nameof(TakerGets), 5);
        public static readonly AmountField LowLimit = new AmountField(nameof(LowLimit), 6);
        public static readonly AmountField HighLimit = new AmountField(nameof(HighLimit), 7);
        public static readonly AmountField Fee = new AmountField(nameof(Fee), 8);
        public static readonly AmountField SendMax = new AmountField(nameof(SendMax), 9);
        public static readonly AmountField DeliverMin = new AmountField(nameof(DeliverMin), 10);
        public static readonly AmountField MinimumOffer = new AmountField(nameof(MinimumOffer), 16);
        public static readonly AmountField RippleEscrow = new AmountField(nameof(RippleEscrow), 17);

        // Added in rippled commit: e7f0b8eca69dd47419eee7b82c8716b3aa5a9e39
        public static readonly AmountField DeliveredAmount = new AmountField(nameof(DeliveredAmount), 18);
        // Added in rippled commit: e7f0b8eca69dd47419eee7b82c8716b3aa5a9e39
        public static readonly AmountField BrokerFee = new AmountField(nameof(BrokerFee), 19);

        // These are auxiliary fields
        // ReSharper disable once InconsistentNaming
        public static readonly AmountField taker_gets_funded = new AmountField(nameof(taker_gets_funded), 258);
        // ReSharper disable once InconsistentNaming
        public static readonly AmountField taker_pays_funded = new AmountField(nameof(taker_pays_funded), 259);

        public static readonly BlobField PublicKey = new BlobField(nameof(PublicKey), 1);
        public static readonly BlobField MessageKey = new BlobField(nameof(MessageKey), 2);
        public static readonly BlobField SigningPubKey = new BlobField(nameof(SigningPubKey), 3);

        // ReSharper disable once RedundantArgumentNameForLiteralExpression
        public static readonly BlobField TxnSignature = new BlobField(nameof(TxnSignature), 4, isSigningField:false);
        public static readonly BlobField Generator = new BlobField(nameof(Generator), 5);
        public static readonly BlobField Signature = new BlobField(nameof(Signature), 6);
        public static readonly BlobField Domain = new BlobField(nameof(Domain), 7);
        public static readonly BlobField FundCode = new BlobField(nameof(FundCode), 8);
        public static readonly BlobField RemoveCode = new BlobField(nameof(RemoveCode), 9);
        public static readonly BlobField ExpireCode = new BlobField(nameof(ExpireCode), 10);
        public static readonly BlobField CreateCode = new BlobField(nameof(CreateCode), 11);
        public static readonly BlobField MemoType = new BlobField(nameof(MemoType), 12);
        public static readonly BlobField MemoData = new BlobField(nameof(MemoData), 13);
        public static readonly BlobField MemoFormat = new BlobField(nameof(MemoFormat), 14);
        public static readonly BlobField Fulfillment = new BlobField(nameof(Fulfillment), 16);
        public static readonly BlobField Condition = new BlobField(nameof(Condition), 17);
        public static readonly BlobField MasterSignature = new BlobField(nameof(MasterSignature), 18);

        public static readonly AccountIdField Account = new AccountIdField(nameof(Account), 1);
        public static readonly AccountIdField Owner = new AccountIdField(nameof(Owner), 2);
        public static readonly AccountIdField Destination = new AccountIdField(nameof(Destination), 3);
        public static readonly AccountIdField Issuer = new AccountIdField(nameof(Issuer), 4);
        public static readonly AccountIdField Target = new AccountIdField(nameof(Target), 7);
        public static readonly AccountIdField RegularKey = new AccountIdField(nameof(RegularKey), 8);

        public static readonly StObjectField ObjectEndMarker = new StObjectField(nameof(ObjectEndMarker), 1);
        public static readonly StObjectField TransactionMetaData = new StObjectField(nameof(TransactionMetaData), 2);
        public static readonly StObjectField CreatedNode = new StObjectField(nameof(CreatedNode), 3);
        public static readonly StObjectField DeletedNode = new StObjectField(nameof(DeletedNode), 4);
        public static readonly StObjectField ModifiedNode = new StObjectField(nameof(ModifiedNode), 5);
        public static readonly StObjectField PreviousFields = new StObjectField(nameof(PreviousFields), 6);
        public static readonly StObjectField FinalFields = new StObjectField(nameof(FinalFields), 7);
        public static readonly StObjectField NewFields = new StObjectField(nameof(NewFields), 8);
        public static readonly StObjectField TemplateEntry = new StObjectField(nameof(TemplateEntry), 9);
        public static readonly StObjectField Memo = new StObjectField(nameof(Memo), 10);
        public static readonly StObjectField SignerEntry = new StObjectField(nameof(SignerEntry), 11);
        public static readonly StObjectField Signer = new StObjectField(nameof(Signer), 16);
        public static readonly StObjectField Majority = new StObjectField(nameof(Majority), 18);

        public static readonly StArrayField ArrayEndMarker = new StArrayField(nameof(ArrayEndMarker), 1);
        // ReSharper disable once RedundantArgumentNameForLiteralExpression
        public static readonly StArrayField Signers = new StArrayField(nameof(Signers), 3, isSigningField:false);
        public static readonly StArrayField SignerEntries = new StArrayField(nameof(SignerEntries), 4);
        public static readonly StArrayField Template = new StArrayField(nameof(Template), 5);
        public static readonly StArrayField Necessary = new StArrayField(nameof(Necessary), 6);
        public static readonly StArrayField Sufficient = new StArrayField(nameof(Sufficient), 7);
        public static readonly StArrayField AffectedNodes = new StArrayField(nameof(AffectedNodes), 8);
        public static readonly StArrayField Memos = new StArrayField(nameof(Memos), 9);

        public static readonly Uint8Field CloseResolution = new Uint8Field(nameof(CloseResolution), 1);
        public static readonly Uint8Field Method = new Uint8Field(nameof(Method), 2);
        public static readonly EngineResultField TransactionResult = new EngineResultField(nameof(TransactionResult), 3);

        //uncommon
        public static readonly Uint8Field TickSize = new Uint8Field(nameof(TickSize), 16);

        public static readonly Hash160Field TakerPaysCurrency = new Hash160Field(nameof(TakerPaysCurrency), 1);
        public static readonly Hash160Field TakerPaysIssuer = new Hash160Field(nameof(TakerPaysIssuer), 2);
        public static readonly Hash160Field TakerGetsCurrency = new Hash160Field(nameof(TakerGetsCurrency), 3);
        public static readonly Hash160Field TakerGetsIssuer = new Hash160Field(nameof(TakerGetsIssuer), 4);

        public static readonly PathSetField Paths = new PathSetField(nameof(Paths), 1);

        public static readonly Vector256Field Indexes = new Vector256Field(nameof(Indexes), 1);
        public static readonly Vector256Field Hashes = new Vector256Field(nameof(Hashes), 2);
        public static readonly Vector256Field Features = new Vector256Field(nameof(Features), 3);
        public static readonly Vector256Field TokenOffers = new Vector256Field(nameof(TokenOffers), 4);

        public static readonly Field Transaction = new Field(nameof(Transaction), 1, FieldType.Transaction);
        public static readonly Field LedgerEntry = new Field(nameof(LedgerEntry), 1, FieldType.LedgerEntry);
        public static readonly Field Validation = new Field(nameof(Validation), 1, FieldType.Validation);
    }
}