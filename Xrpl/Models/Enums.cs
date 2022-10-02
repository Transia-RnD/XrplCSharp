using System.Runtime.Serialization;

namespace Xrpl.Models
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
    /// <summary>
    /// Each ledger version's state data is a set of ledger objects, sometimes called ledger entries,
    /// which collectively represent all settings, balances, and relationships at a given point in time.<br/>
    /// To store or retrieve an object in the state data, the protocol uses that object's unique Ledger Object ID.<br/>
    /// In the peer protocol, ledger objects have a canonical binary format.In rippled APIs, ledger objects are represented as JSON objects.
    /// </summary>
    public enum LedgerEntryType
    {
        /// <summary>
        /// The settings, XRP balance, and other metadata for one account.
        /// </summary>
        AccountRoot,
        /// <summary>
        /// Singleton object with status of enabled and pending amendments.
        /// </summary>
        Amendments,
        /// <summary>
        /// Contains links to other objects.
        /// </summary>
        DirectoryNode,
        /// <summary>
        /// Contains XRP held for a conditional payment.
        /// </summary>
        Escrow,
        /// <summary>
        /// Singleton object with consensus-approved base transaction cost and reserve requirements.
        /// </summary>
        FeeSettings,
        /// <summary>
        /// Lists of prior ledger versions' hashes for history lookup.
        /// </summary>
        LedgerHashes,
        /// <summary>
        /// An order to make a currency trade.
        /// </summary>
        Offer,
        /// <summary>
        /// A channel for asynchronous XRP payments.
        /// </summary>
        PayChannel,
        /// <summary>
        /// Links two accounts, tracking the balance of one currency between them.<br/>
        /// The concept of a trust line is an abstraction of this object type.
        /// </summary>
        RippleState,
        /// <summary>
        /// A list of addresses for multi-signing transactions.
        /// </summary>
        SignerList,
        /// <summary>
        /// List of validators currently believed to be offline.
        /// </summary>
        NegativeUNL,
        /// <summary>
        /// Create offers to buy or sell NFTs.
        /// </summary>
        NFTokenOffer,
        /// <summary>
        /// Ledger structure for recording NFTokens.
        /// </summary>
        NFTokenPage,
        /// <summary>
        /// A list of addresses for multi-signing transactions.
        /// </summary>
        Ticket,
        /// <summary>
        /// A Ticket tracks an account sequence number that has been set aside for future use.
        /// </summary>
        Check,
        /// <summary>
        /// A record of preauthorization for sending payments to an account that requires authorization.
        /// </summary>
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

