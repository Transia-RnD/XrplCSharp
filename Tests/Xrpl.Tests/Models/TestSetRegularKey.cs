

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/setRegularKey.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUSetRegularKey
    {
        public static Dictionary<string, dynamic> account;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            account = new Dictionary<string, dynamic>
            {
                {"TransactionType", "SetRegularKey"},
                {"Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"Fee", "12"},
                {"Flags", 0},
                {"RegularKey", "rAR8rR8sUkBoCZFawhkWzY4Y5YoyuznwD"},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid SetRegularKey
            await Validation.ValidateSetRegularKey(account);
            await Validation.Validate(account);

            // verifies w/o SetRegularKey
            account.Remove("SetRegularKey");
            await Validation.ValidateSetRegularKey(account);
            await Validation.Validate(account);
            account["SetRegularKey"] = "rAR8rR8sUkBoCZFawhkWzY4Y5YoyuznwD";


            // throws w/ invalid RegularKey
            account["RegularKey"] = 12369846963;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateSetRegularKey(account), "SetRegularKey: RegularKey must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(account), "SetRegularKey: RegularKey must be a string");
            account["RegularKey"] = "rAR8rR8sUkBoCZFawhkWzY4Y5YoyuznwD";
        }
    }

}

