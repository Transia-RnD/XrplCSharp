using Newtonsoft.Json;
using xrpl_c.Xrpl.Client.Model.Enums;

namespace Xrpl.Client.Model.Transaction.Interfaces
{
    public interface ITicketCreateTransaction : ITransactionCommon
    {
        /// <summary> A bit-map of Boolean flags enabled for this Ticket. Currently, there are no flags defined for Tickets. </summary>
        new TrustSetFlags? Flags { get; set; }
        /// <summary> The value 0x0054, mapped to the string Ticket, indicates that this object is a Ticket object. </summary>
        LedgerEntryType LedgerEntryType { get; set; }
        /// <summary>
        /// A hint indicating which page of the owner directory links to this object, in case the directory consists of multiple pages.
        /// Note: The object does not contain a direct link to the owner directory containing it, since that value can be derived from the Account.
        /// </summary>
        string OwnerNode { get; set; }
        /// <summary> The identifying hash of the transaction that most recently modified this object. </summary>
        [JsonProperty("PreviousTxnID")]
        public string PreviousTransactionId { get; set; }
        /// <summary> The index of the ledger that contains the transaction that most recently modified this object. </summary>
        [JsonProperty("PreviousTxnLgrSeq")]
        public uint PreviousTransactionLedgerSequence { get; set; }
        /// <summary> The Sequence Number this Ticket sets aside. </summary>
        public uint TicketSequence { get; set; }
        /// <summary>
        /// How many Tickets to create. This must be a positive number and cannot cause the account to own more than 250 Tickets after executing this transaction.
        /// </summary>
        public uint TicketCount { get; set; }
    }
}