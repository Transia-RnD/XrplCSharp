using Xrpl.Client.Models.Enums;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/ticketCreate.ts

namespace Xrpl.Client.Models.Transactions
{
    public class TicketCreateTransaction : TransactionCommon, ITicketCreateTransaction
    {
        public TicketCreateTransaction()
        {
            TransactionType = TransactionType.TicketCreate;
        }


        /// <summary>
        /// How many Tickets to create. This must be a positive number and cannot cause the account to own more than 250 Tickets after executing this transaction.
        /// </summary>
        public uint TicketCount { get; set; }
    }

    public interface ITicketCreateTransaction : ITransactionCommon
    {
        public uint TicketCount { get; set; }
    }

    public class TicketCreateTransactionResponse : TransactionResponseCommon, ITicketCreateTransaction
    {
        /// <inheritdoc/>
        public uint TicketCount { get; set; }
    }
}

