// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Ticket.ts

namespace Xrpl.Models.Ledger
{
    /// <summary>
    /// The Ticket object type represents a Ticket, which tracks an account sequence number that has been set aside for future use.<br/>
    /// You can create new tickets with a TicketCreate transaction.
    /// </summary>
    public class LOTicket : BaseLedgerEntry
    {
        public LOTicket()
        {
            LedgerEntryType = LedgerEntryType.Ticket;
        }

        /// <summary>
        /// The sender of the Check. Cashing the Check debits this address's balance.<br/>
        /// The account that owns this Ticket.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// A bit-map of Boolean flags enabled for this Ticket.<br/>
        /// Currently, there are no flags defined for Tickets.<br/>
        /// No flags are defined for Checks, so this value is always 0.
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// A hint indicating which page of the sender's owner directory links to this object,
        /// in case the directory consists of multiple pages.
        /// </summary>
        public string OwnerNode { get; set; }

        /// <summary>
        /// The identifying hash of the transaction that most recently modified this object.
        /// </summary>
        public string PreviousTxnID { get; set; }

        /// <summary>
        /// The index of the ledger that contains the transaction that most recently modified this object.
        /// </summary>
        public uint PreviousTxnLgrSeq { get; set; }

        /// <summary>
        /// The maximum amount of currency this Check can debit the sender.<br/>
        /// If the Check is successfully cashed, the destination is credited in the same currency for up to this amount.<br/>
        /// The Sequence Number this Ticket sets aside.
        /// </summary>
        public uint TicketSequence { get; set; }
    }
}
