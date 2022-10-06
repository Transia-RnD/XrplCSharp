
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/ticketCreate.ts

namespace Xrpl.Models.Transaction
{
    /// <inheritdoc cref="ITicketCreate" />
    public class TicketCreate : TransactionCommon, ITicketCreate
    {
        public TicketCreate()
        {
            TransactionType = TransactionType.TicketCreate;
        }


        /// <inheritdoc />
        public uint TicketCount { get; set; }
    }

    /// <summary>
    /// A TicketCreate transaction sets aside one or more sequence numbers as  Tickets.
    /// </summary>
    public interface ITicketCreate : ITransactionCommon
    {
        /// <summary>
        /// How many Tickets to create.<br/>
        /// This must be a positive number and cannot cause the account to own more than 250 Tickets after executing this transaction.
        /// </summary>
        public uint TicketCount { get; set; }
    }

    /// <inheritdoc cref="ITicketCreate" />
    public class TicketCreateResponse : TransactionResponseCommon, ITicketCreate
    {
        /// <inheritdoc/>
        public uint TicketCount { get; set; }
    }
}