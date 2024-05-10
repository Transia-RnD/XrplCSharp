using System.Runtime.Serialization;

namespace Xrpl.Models
{
    /// <summary>
    /// The type of a transaction (TransactionType field) is the most fundamental information about a transaction.<br/>
    /// This indicates what type of operation the transaction is supposed to do.
    /// </summary>
    public enum TransactionType
    {
        /// <summary> Set options on an account.</summary>
        AccountSet,
        /// <summary> Delete an account.</summary>
        AccountDelete,
        /// <summary> Cancel a check.</summary>
        CheckCancel,
        /// <summary> Redeem a check.</summary>
        CheckCash,
        /// <summary>Create a check.</summary>
        CheckCreate,
        /// <summary>Preauthorizes an account to send payments to this one.</summary>
        DepositPreauth,
        /// <summary>Reclaim escrowed XRP.</summary>
        EscrowCancel,
        /// <summary>Create an escrowed XRP payment.</summary>
        EscrowCreate,
        /// <summary>Deliver escrowed XRP to recipient.</summary>
        EscrowFinish,
        /// <summary>Accept an offer to buy or sell an NFToken.</summary>
        NFTokenAcceptOffer,
        /// <summary>Use TokenBurn to permanently destroy NFTs.</summary>
        NFTokenBurn,
        /// <summary>Cancel existing token offers to buy or sell an NFToken.</summary>
        NFTokenCancelOffer,
        /// <summary>Create an offer to buy or sell NFTs.</summary>
        NFTokenCreateOffer,
        /// <summary>Use TokenMint to issue new NFTs.</summary>
        NFTokenMint,
        /// <summary>Withdraw a currency-exchange order.</summary>
        OfferCancel,
        /// <summary>Submit an order to exchange currency.</summary>
        OfferCreate,
        /// <summary>Send funds from one account to another.</summary>
        Payment,
        /// <summary>Claim money from a payment channel.</summary>
        PaymentChannelClaim,
        /// <summary>Open a new payment channel.</summary>
        PaymentChannelCreate,
        /// <summary>Add more XRP to a payment channel.</summary>
        PaymentChannelFund,
        /// <summary>Add, remove, or modify an account's regular key pair.</summary>
        SetRegularKey,
        /// <summary>Add, remove, or modify an account's multi-signing list.</summary>
        SignerListSet,
        /// <summary>Set aside one or more sequence numbers as Tickets.</summary>
        TicketCreate,
        /// <summary>Add or modify a trust line.</summary>
        TrustSet,
        /// <summary> AMMBid is used for submitting a vote for the trading fee of an AMM Instance. </summary>
        AMMBid,
        /// <summary>
        /// AMMCreate is used to create AccountRoot and the corresponding AMM ledger entries.
        /// </summary>
        AMMCreate,
        /// <summary>
        /// Delete an empty Automated Market Maker (AMM) instance that could not be fully deleted automatically.
        /// </summary>
        AMMDelete,
        /// <summary>
        /// AMMDeposit is the deposit transaction used to add liquidity to the AMM instance pool,
        /// thus obtaining some share of the instance's pools in the form of LPTokenOut.
        /// </summary>
        AMMDeposit,
        /// <summary>
        /// AMMVote is used for submitting a vote for the trading fee of an AMM Instance.
        /// </summary>
        AMMVote,
        /// <summary>
        /// AMMWithdraw is the withdraw transaction used to remove liquidity from the AMM
        /// instance pool, thus redeeming some share of the pools that one owns in the form
        /// of LPTokenIn.
        /// </summary>
        AMMWithdraw,

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
        DepositPreauth,
        AMM
    }

    public enum StreamType
    {
        /// <summary>
        /// ledgerClosed indicates this is from the ledger stream.
        /// </summary>
        [EnumMember(Value = "ledgerClosed")]
        Ledger,
        /// <summary>
        /// The value validationReceived indicates this is from the validations stream.
        /// </summary>
        [EnumMember(Value = "validationReceived")]
        Validations,
        /// <summary>
        /// transaction indicates this is the notification of a transaction, which could come from several possible streams.
        /// </summary>
        [EnumMember(Value = "transaction")]
        Transaction,
        /// <summary>
        /// peerStatusChange indicates this comes from the Peer Status stream.
        /// </summary>
        [EnumMember(Value = "peerStatusChange")]
        PeerStatus,
        /// <summary>
        /// The format of an order book stream message is the same as that of transaction stream messages,
        /// except that OfferCreate transactions also contain the owner_funds field.
        /// </summary>
        [EnumMember(Value = "transaction")]
        OrderBook,
        /// <summary>
        /// consensusPhase indicates this is from the consensus stream.
        /// </summary>
        [EnumMember(Value = "consensusPhase")]
        ConsensusStream
    }
}

