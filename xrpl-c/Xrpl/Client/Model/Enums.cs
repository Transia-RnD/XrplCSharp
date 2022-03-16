using System;
using System.Runtime.Serialization;

namespace RippleDotNet.Model
{
    public enum TransactionType
    {
        Payment,
        OfferCreate,
        OfferCancel,
        TrustSet,
        AccountSet,
        SetRegularKey,
        SignerListSet,
        EscrowCreate,
        EscrowFinish,
        EscrowCancel,
        PaymentChannelCreate,
        PaymentChannelFund,
        PaymentChannelClaim,
        EnableAmendment,
        SetFee,
        AccountDelete,
        NFTokenMint,
        NFTokenBurn,
        NFTokenCreateOffer,
        NFTokenCancelOffer,
        NFTokenAcceptOffer
    }

    public enum LedgerEntryType
    {
        AccountRoot,
        Amendments,
        DirectoryNode,
        Escrow,
        FeeSettings,
        LedgerHashes,
        Offer,
        PayChannel,
        RippleState,
        SignerList
    }

    [Flags]
    public enum RippleStateFlags
    {
        lsfLowReserve = 65536,
        lsfHighReserve = 131072,
        lsfLowAuth = 262144,
        lsfHighAuth = 524288,
        lsfLowNoRipple = 1048576,
        lsfHighNoRipple = 2097152,
        lsfLowFreeze = 4194304,
        lsfHighFreeze = 8388608
    }

    [Flags]
    public enum AccountRootFlags
    {
        lsfPasswordSpent = 65536,
        lsfRequireDestTag = 131072,
        lsfRequireAuth = 262144,
        lsfDisallowXRP = 524288,
        lsfDisableMaster = 1048576,
        lsfNoFreeze = 2097152,
        lsfGlobalFreeze = 4194304,
        lsfDefaultRipple = 8388608
    }

    [Flags]
    public enum OfferFlags
    {
        lsfPassive = 65536,
        lsfSell = 131072
    }

    
    public enum RoleType
    {
        [EnumMember(Value = "gateway")]
        Gateway,
        [EnumMember(Value = "user")]
        User
    }

    public enum StreamType
    {
        [EnumMember(Value = "ledgerClosed")]
        Ledger,
        [EnumMember(Value = "validationReceived")]
        Validations,
        [EnumMember(Value = "transaction")]
        Transaction,
        [EnumMember(Value = "peerStatusChange")]
        PeerStatus,
        [EnumMember(Value = "transaction")]
        OrderBook
    }

    public enum ServerState
    {
        [EnumMember(Value = "disconnected")]
        Disconnected,
        [EnumMember(Value = "connected")]
        Connected,
        [EnumMember(Value = "syncing")]
        Syncing,
        [EnumMember(Value = "tracking")]
        Tracking,
        [EnumMember(Value = "full")]
        Full,
        [EnumMember(Value = "validating")]
        Validating,
        [EnumMember(Value = "proposing")]
        Proposing
    }

    [Flags]
    public enum PaymentFlags : uint
    {
        tfNoDirectRipple = 65536,
        tfPartialPayment = 131072,
        tfLimitQuality = 262144,
    }

    public enum AccountSetFlags
    {
        asfRequireDest = 1,
        asfRequireAuth = 2,
        asfDisallowXRP = 3,
        asfDisableMaster = 4,
        asfAccountTxnID = 5,
        asfNoFreeze = 6,
        asfGlobalFreeze = 7,
        asfDefaultRipple = 8
    }

    [Flags]
    public enum TrustSetFlags : uint
    {
        tfSetfAuth = 65536,
        tfSetNoRipple = 131072,
        tfClearNoRipple = 262144,
        tfSetFreeze = 1048576,
        tfClearFreeze = 2097152
    }

    [Flags]
    public enum OfferCreateFlags : uint
    {
        tfPassive = 65536,
        tfImmediateOrCancel = 131072,
        tfFillOrKill = 262144,
        tfSell = 524288
    }

    [Flags]
    public enum NFTokenMintFlags : uint
    {
        tfBurnable = 1,
        tfOnlyXRP = 2,
        tfTrustLine = 3,
        tfTransferable = 4
    }

    [Flags]
    public enum NFTokenCreateOfferFlags : uint
    {
        tfSellToken = 1
    }

    [Flags]
    public enum PaymentChannelClaimFlags : uint
    {
        tfRenew = 65536,
        tfClose = 131072
    }

    public enum EnableAmendmentFlags
    {
        tfGotMajority = 65536,
        tfLostMajority = 131072
    }

    public enum LedgerIndexType
    {
        [EnumMember(Value = "current")]
        Current,
        [EnumMember(Value = "closed")]
        Closed,
        [EnumMember(Value = "validated")]
        Validated
    }

    public enum LedgerEntryRequestType
    {
        [EnumMember(Value = "index")]
        Index,
        [EnumMember(Value = "account_root")]
        AccountRoot,
        [EnumMember(Value = "directory")]
        Directory,
        [EnumMember(Value = "offer")]
        Offer,
        [EnumMember(Value = "ripple_state")]
        RippleState
    }
}
