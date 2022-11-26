

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/escrowCreate.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUEscrowCreate
    {
        public static Dictionary<string, dynamic> depositPreauth;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            depositPreauth = new Dictionary<string, dynamic>
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
            await Validation.ValidateEscrowCreate(depositPreauth);
            await Validation.Validate(depositPreauth);

            // invalid EscrowCreate missing amount
            depositPreauth.Remove("Amount");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: missing amount");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: missing amount");
            depositPreauth["Amount"] = "10000";

            // invalid EscrowCreate missing destination
            depositPreauth.Remove("Destination");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: missing destination");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: missing destination");
            depositPreauth["Destination"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";

            // Invalid Destination
            depositPreauth["Destination"] = 10;
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: Destination must be a string");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: Destination must be a string");
            depositPreauth["Destination"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";

            // Invalid Amount
            depositPreauth["Amount"] = 1000;
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: Amount must be a string");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: Amount must be a string");
            depositPreauth["Amount"] = "10000";

            // Invalid CancelAfter
            depositPreauth["CancelAfter"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: CancelAfter must be a number");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: CancelAfter must be a number");
            depositPreauth["CancelAfter"] = 533257958u;

            // Invalid FinishAfter
            depositPreauth["FinishAfter"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: FinishAfter must be a number");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: FinishAfter must be a number");
            depositPreauth["FinishAfter"] = 533171558u;

            // Invalid Condition
            depositPreauth["Condition"] = 0x141243;
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: Condition must be a string");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: Condition must be a string");
            depositPreauth["Condition"] = "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100";

            // Invalid DestinationTag
            depositPreauth["DestinationTag"] = "100";
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: DestinationTag must be a number");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: DestinationTag must be a number");
            depositPreauth["DestinationTag"] = 23480u;

            // Missing both CancelAfter and FinishAfter
            depositPreauth.Remove("FinishAfter");
            depositPreauth.Remove("CancelAfter");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: Either CancelAfter or FinishAfter must be specified");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: Either CancelAfter or FinishAfter must be specified");
            depositPreauth["FinishAfter"] = 533171558u;
            depositPreauth["CancelAfter"] = 533257958u;

            // Missing both Condition and FinishAfter
            depositPreauth.Remove("FinishAfter");
            depositPreauth.Remove("Condition");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCreate(depositPreauth), "EscrowCreate: Either Condition or FinishAfter must be specified");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCreate: Either Condition or FinishAfter must be specified");
            depositPreauth["FinishAfter"] = 533171558u;
            depositPreauth["Condition"] = "A0258020E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855810100";
        }
    }

}

