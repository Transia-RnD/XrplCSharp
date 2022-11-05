
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/ticketCreate.ts

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Ledger;

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

    public partial class Validation
    {
        private const uint MAX_TICKETS = 250;
        /// <summary>
        /// Verify the form and type of a TicketCreate at runtime.
        /// </summary>
        /// <param name="tx"> A TicketCreate Transaction.</param>
        /// <exception cref="ValidationError">When the TicketCreate is malformed.</exception>
        public static async Task ValidateTicketCreate(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);


            if (!tx.TryGetValue("TicketCount", out var TicketCount) || TicketCount is null)
                throw new ValidationError("TicketCreate: missing field TicketCount");
            if (TicketCount is not uint count)
                throw new ValidationError("TicketCreate: TicketCount must be a number");

            if(count is < 1 or > MAX_TICKETS)
                throw new ValidationError("TicketCreate: TicketCount must be an integer from 1 to 250");

        }
    }

}