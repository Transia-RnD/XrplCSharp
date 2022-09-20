using System;
using System.Runtime.Serialization;

namespace Xrpl.Client.Models.Enums
{
    public enum TransactionType
    {
        AccountSet,
        AccountDelete,
        CheckCancel,
        CheckCash,
        CheckCreate,
        DepositPreauth,
        EscrowCancel,
        EscrowCreate,
        EscrowFinish,
        NFTokenAcceptOffer,
        NFTokenBurn,
        NFTokenCancelOffer,
        NFTokenCreateOffer,
        NFTokenMint,
        OfferCancel,
        OfferCreate,
        Payment,
        PaymentChannelClaim,
        PaymentChannelCreate,
        PaymentChannelFund,
        SetRegularKey,
        SignerListSet,
        TicketCreate,
        TrustSet
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
        NFTokenPage,
        Ticket,
        Check,
        DepositPreauth
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

