using Newtonsoft.Json;
using Xrpl.Client.Models.Enums;
using Xrpl.Client.Json.Converters;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/ledger/Ticket.ts

namespace Xrpl.Client.Models.Ledger
{
    public class LOTicket : BaseLedgerEntry
    {
        public LOTicket()
        {
            LedgerEntryType = LedgerEntryType.Ticket;
        }

        /** The sender of the Check. Cashing the Check debits this address's balance. */
        public string Account { get; set; }
        /**
        * A bit-map of boolean flags. No flags are defined for Checks, so this value
        * is always 0.
        */
        public string Flags { get; set; }
        /**
        * A hint indicating which page of the sender's owner directory links to this
        * object, in case the directory consists of multiple pages.
        */
        public string OwnerNode { get; set; }
        /**
        * The identifying hash of the transaction that most recently modified this
        * object.
        */
        public string PreviousTxnID { get; set; }
        /**
        * The index of the ledger that contains the transaction that most recently
        * modified this object.
        */
        public uint PreviousTxnLgrSeq { get; set; }
        /**
        * The maximum amount of currency this Check can debit the sender. If the
        * Check is successfully cashed, the destination is credited in the same
        * currency for up to this amount.
        */
        /** The Sequence Number this Ticket sets aside. */
        public uint TicketSequence { get; set; }
    }
}
