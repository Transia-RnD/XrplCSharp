

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/escrowCreate.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUEscrowCreate
    {
        public static Dictionary<string, dynamic> escrowCreate;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            escrowCreate = new Dictionary<string, dynamic>
            {
                {"TransactionType", "EscrowCreate"},
                {"Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"Amount", "10000"},
                {"Destination", "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW"},
                {"CancelAfter", 533257958u},
                {"FinishAfter", 533171558u},
                {"Condition", "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100"},
                {"DestinationTag", 23480u},
                {"SourceTag", 11747u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {

            //verifies valid EscrowCreate
            await Validation.ValidateEscrowCreate(escrowCreate);
            await Validation.Validate(escrowCreate);

            // invalid EscrowCreate missing amount
            escrowCreate.Remove("Amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: missing amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: missing amount");
            escrowCreate["Amount"] = "10000";

            // invalid EscrowCreate missing destination
            escrowCreate.Remove("Destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: missing destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: missing destination");
            escrowCreate["Destination"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";

            // Invalid Destination
            escrowCreate["Destination"] = 10;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: Destination must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: Destination must be a string");
            escrowCreate["Destination"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";

            // Invalid Amount
            escrowCreate["Amount"] = 1000;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: Amount must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: Amount must be a string");
            escrowCreate["Amount"] = "10000";

            // Invalid CancelAfter
            escrowCreate["CancelAfter"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: CancelAfter must be a number");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: CancelAfter must be a number");
            escrowCreate["CancelAfter"] = 533257958u;

            // Invalid FinishAfter
            escrowCreate["FinishAfter"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: FinishAfter must be a number");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: FinishAfter must be a number");
            escrowCreate["FinishAfter"] = 533171558u;

            // Invalid Condition
            escrowCreate["Condition"] = 0x141243;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: Condition must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: Condition must be a string");
            escrowCreate["Condition"] = "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100";

            // Invalid DestinationTag
            escrowCreate["DestinationTag"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: DestinationTag must be a number");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: DestinationTag must be a number");
            escrowCreate["DestinationTag"] = 23480u;

            // Missing both CancelAfter and FinishAfter
            escrowCreate.Remove("FinishAfter");
            escrowCreate.Remove("CancelAfter");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: Either CancelAfter or FinishAfter must be specified");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: Either CancelAfter or FinishAfter must be specified");
            escrowCreate["FinishAfter"] = 533171558u;
            escrowCreate["CancelAfter"] = 533257958u;

            // Missing both Condition and FinishAfter
            escrowCreate.Remove("FinishAfter");
            escrowCreate.Remove("Condition");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateEscrowCreate(escrowCreate), "EscrowCreate: Either Condition or FinishAfter must be specified");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(escrowCreate), "EscrowCreate: Either Condition or FinishAfter must be specified");
            escrowCreate["FinishAfter"] = 533171558u;
            escrowCreate["Condition"] = "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100";
        }
    }

}

