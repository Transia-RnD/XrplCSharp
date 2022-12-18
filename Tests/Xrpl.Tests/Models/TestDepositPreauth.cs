

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/depositPreauth.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUDepositPreauth
    {
        public static Dictionary<string, dynamic> depositPreauth;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            depositPreauth = new Dictionary<string, dynamic>
            {
                {"TransactionType", "DepositPreauth"},
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {

            //verifies valid DepositPreauth when only Authorize is provided
            depositPreauth["Authorize"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";
            await Validation.ValidateDepositPreauth(depositPreauth); 
            await Validation.Validate(depositPreauth);
            depositPreauth.Remove("Authorize");

            // valid DepositPreauth when only Unauthorize is provided
            depositPreauth["Unauthorize"] = "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n";
            await Validation.ValidateDepositPreauth(depositPreauth);
            await Validation.Validate(depositPreauth);
            depositPreauth.Remove("Unauthorize");

            //throws when both Authorize and Unauthorize are provided
            depositPreauth["Unauthorize"] = "raKEEVSGnKSD9Zyvxu4z6Pqpm4ABH8FS6n";
            depositPreauth["Authorize"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateDepositPreauth(depositPreauth), "DepositPreauth: can't provide both Authorize and Unauthorize fields");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(depositPreauth), "DepositPreauth: can't provide both Authorize and Unauthorize fields");
            depositPreauth.Remove("Authorize");
            depositPreauth.Remove("Unauthorize");

            //throws when neither Authorize nor Unauthorize are provided
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateDepositPreauth(depositPreauth), "DepositPreauth: must provide either Authorize or Unauthorize field");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(depositPreauth), "DepositPreauth: must provide either Authorize or Unauthorize field");

            //throws when Authorize is not a string
            depositPreauth["Authorize"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateDepositPreauth(depositPreauth), "DepositPreauth: Authorize must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(depositPreauth), "DepositPreauth: Authorize must be a string");
            depositPreauth.Remove("Authorize");

            //throws when Unauthorize is not a string
            depositPreauth["Unauthorize"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateDepositPreauth(depositPreauth), "DepositPreauth: Unauthorize must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(depositPreauth), "DepositPreauth: Unauthorize must be a string");
            depositPreauth.Remove("Unauthorize");

            //throws when an Account attempts to unauthorize its own address
            depositPreauth["Unauthorize"] = depositPreauth["Authorize"] = "rsA2LpzuawewSBQXkiju3YQTMzW13pAAdW";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateDepositPreauth(depositPreauth), "DepositPreauth: Account can't unauthorize its own address");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(depositPreauth), "DepositPreauth: Account can't unauthorize its own address");
            depositPreauth.Remove("Authorize");
            depositPreauth.Remove("Unauthorize");


        }
    }

}

