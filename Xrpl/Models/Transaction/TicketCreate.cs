


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/ticketCreate.ts

namespace Xrpl.Models.Transaction
{
    public class TicketCreate : TransactionCommon, ITicketCreate
    {
        public TicketCreate()
        {
            TransactionType = TransactionType.TicketCreate;
        }


        /// <summary>
        /// How many Tickets to create. This must be a positive number and cannot cause the account to own more than 250 Tickets after executing this transaction.
        /// </summary>
        public uint TicketCount { get; set; }
    }

    public interface ITicketCreate : ITransactionCommon
    {
        public uint TicketCount { get; set; }
    }

    public class TicketCreateResponse : TransactionResponseCommon, ITicketCreate
    {
        /// <inheritdoc/>
        public uint TicketCount { get; set; }
    }
}