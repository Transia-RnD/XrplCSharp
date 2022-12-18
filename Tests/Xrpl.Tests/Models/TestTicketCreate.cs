

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/ticketCreate.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUTicketCreate
    {
        public static Dictionary<string, dynamic> ticketCreate;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            ticketCreate = new Dictionary<string, dynamic>
            {
                {"TransactionType", "TicketCreate"},
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"TicketCount", 150u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid TicketCreate
            await Validation.ValidateTicketCreate(ticketCreate);
            await Validation.Validate(ticketCreate);

            // throws when TicketCount is missing
            ticketCreate.Remove("TicketCount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateTicketCreate(ticketCreate), "TicketCreate:  missing field TicketCount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ticketCreate), "TicketCreate:  missing field TicketCount");
            ticketCreate["TicketCount"] = 150u;

            // throws when TicketCount is not a number
            ticketCreate["TicketCount"] = "150";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateTicketCreate(ticketCreate), "TicketCreate:  TicketCount must be a number");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ticketCreate), "TicketCreate:  TicketCount must be a number");
            ticketCreate["TicketCount"] = 150u;

            // throws when TicketCount is not an uint
            ticketCreate["TicketCount"] = 12.5;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateTicketCreate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            ticketCreate["TicketCount"] = 150u;

            // throws when TicketCount is < 1
            ticketCreate["TicketCount"] = 0u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateTicketCreate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            ticketCreate["TicketCount"] = 150u;

            // throws when TicketCount is > 250
            ticketCreate["TicketCount"] = 251u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateTicketCreate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ticketCreate), "TicketCreate:  TicketCount must be an integer from 1 to 250");
            ticketCreate["TicketCount"] = 150u;
        }
    }

}

