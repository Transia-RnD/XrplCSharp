

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/escrowCancel.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUEscrowCancel
    {
        public static Dictionary<string, dynamic> depositPreauth;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            depositPreauth = new Dictionary<string, dynamic>
            {
                {"TransactionType", "EscrowCancel"},
                {"Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"Owner", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"OfferSequence", 7u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {

            //verifies valid EscrowCancel
            await Validation.ValidateEscrowCancel(depositPreauth);
            await Validation.Validate(depositPreauth);

            // valid EscrowCancel missing owner
            depositPreauth.Remove("Owner");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCancel(depositPreauth), "EscrowCancel: missing Owner");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCancel: missing Owner");
            depositPreauth["Owner"] = "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn";

            // valid EscrowCancel missing OfferSequence
            depositPreauth.Remove("OfferSequence");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCancel(depositPreauth), "EscrowCancel: missing OfferSequence");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCancel: missing OfferSequence");
            depositPreauth["OfferSequence"] = 7u;

            // Invalid owner
            depositPreauth["Owner"] = 10;
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCancel(depositPreauth), "EscrowCancel: Owner must be a string");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCancel: Owner must be a string");
            depositPreauth["Owner"] = "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn";

            // Invalid OfferSequence
            depositPreauth["OfferSequence"] = "10";
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateEscrowCancel(depositPreauth), "EscrowCancel: OfferSequence must be a number");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(depositPreauth), "EscrowCancel: OfferSequence must be a number");
            depositPreauth["OfferSequence"] = 7u;
        }
    }

}

