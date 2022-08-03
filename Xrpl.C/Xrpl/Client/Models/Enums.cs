using System;
using System.Runtime.Serialization;

namespace Xrpl.Client.Models.Enums
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
        SignerList,
        NFTokenPage
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
}

