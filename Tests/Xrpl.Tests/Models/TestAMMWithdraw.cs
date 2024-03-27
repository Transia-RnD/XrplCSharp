// https://github.com/XRPLF/xrpl.js/blob/amm-beta/packages/xrpl/test/models/AMMWithdraw.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAMMWithdraw
    {
        public static Dictionary<string, dynamic> LPTokenIn;
        public static Dictionary<string, dynamic> withdraw;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            LPTokenIn = new Dictionary<string, dynamic>()
            {
                { "currency", "B3813FCAB4EE68B3D0D735D6849465A9113EE048" },
                { "issuer", "rH438jEAzTs5PYtV6CHZqpDpwCKQmPW9Cg" },
                { "value", "1000" },
            };
            withdraw = new Dictionary<string, dynamic>
            {
                {"TransactionType", "AMMWithdraw"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Asset", new Dictionary<string,dynamic>(){{"currency","XRP"}}},
                {"Asset2", new Dictionary<string,dynamic>(){{"currency","ETH"},{"issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" } }},
                {"Sequence", 1337u},
                {"Flags", 0u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid AMMWithdraw with LPTokenIn
            withdraw["LPTokenIn"] = LPTokenIn;
            withdraw["Flags"] = AMMWithdrawFlags.tfLPToken;
            await Validation.Validate(withdraw);
            withdraw.Remove("LPTokenIn");
            withdraw["Flags"] = 0u;


            //verifies valid AMMWithdraw with Amount
            withdraw["Amount"] = "1000";
            withdraw["Flags"] = AMMWithdrawFlags.tfSingleAsset;
            await Validation.Validate(withdraw);
            withdraw.Remove("Amount");
            withdraw["Flags"] = 0u;

            //verifies valid AMMWithdraw with Amount and Amount2
            withdraw["Amount"] = "1000";
            withdraw["Amount2"] = new Dictionary<string, dynamic>()
            {
                {"currency","ETH"},
                {"issuer","rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd"},
                {"value","2.5"},
            };
            withdraw["Flags"] = AMMWithdrawFlags.tfTwoAsset;
            await Validation.Validate(withdraw);
            withdraw.Remove("Amount");
            withdraw.Remove("Amount2");
            withdraw["Flags"] = 0u;


            //verifies valid AMMWithdraw with Amount and LPTokenIn
            withdraw["Amount"] = "1000";
            withdraw["LPTokenIn"] = LPTokenIn;
            withdraw["Flags"] = AMMWithdrawFlags.tfOneAssetLPToken;
            await Validation.Validate(withdraw);
            withdraw.Remove("Amount");
            withdraw.Remove("LPTokenIn");
            withdraw["Flags"] = 0u;

            //verifies valid AMMWithdraw with Amount and EPrice
            withdraw["Amount"] = "1000";
            withdraw["EPrice"] = "25";
            withdraw["Flags"] = AMMWithdrawFlags.tfLimitLPToken;
            await Validation.Validate(withdraw);
            withdraw.Remove("Amount");
            withdraw.Remove("EPrice");
            withdraw["Flags"] = 0u;

            //verifies valid AMMWithdraw one asset withdraw all
            withdraw["Amount"] = "1000";
            withdraw["Flags"] = AMMWithdrawFlags.tfOneAssetWithdrawAll;
            await Validation.Validate(withdraw);
            withdraw.Remove("Amount");
            withdraw["Flags"] = 0u;

            //verifies valid AMMWithdraw withdraw all
            withdraw["Flags"] = AMMWithdrawFlags.tfWithdrawAll;
            await Validation.Validate(withdraw);
            withdraw["Flags"] = 0u;


            //throws w/ missing field Asset
            withdraw.Remove("Asset");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: missing field Asset");
            withdraw["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            withdraw["Asset"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: Asset must be an Issue");
            withdraw["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };

            //throws w/ missing field Asset2
            withdraw.Remove("Asset2");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: missing field Asset2");
            withdraw["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            withdraw["Asset2"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: Asset2 must be an Issue");
            withdraw["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "ETH" }, { "issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" } };

            //throws w/ must set Amount with Amount2
            withdraw["Amount2"] = new Dictionary<string, dynamic>()
            {
                { "currency", "ETH" },
                { "issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" },
                { "value", "2.5" },
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: must set Amount with Amount2");
            withdraw.Remove("Amount2");

            //throws w/ must set Amount with EPrice
            withdraw["EPrice"] = "25";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: must set Amount with EPrice");
            withdraw.Remove("EPrice");

            //throws w/ LPTokenIn must be an IssuedCurrencyAmount
            withdraw["LPTokenIn"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: LPTokenIn must be an IssuedCurrencyAmount");
            withdraw.Remove("LPTokenIn");

            //throws w/ Amount must be an Amount
            withdraw["Amount"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: Amount must be an Amount");
            withdraw.Remove("Amount");

            //throws w/ Amount2 must be an Amount
            withdraw["Amount"] = "1000";
            withdraw["Amount2"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: Amount2 must be an Amount");
            withdraw.Remove("Amount");
            withdraw.Remove("Amount2");

            //throws w/ EPrice must be an Amount
            withdraw["Amount"] = "1000";
            withdraw["EPrice"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(withdraw), "AMMWithdraw: EPrice must be an Amount");
            withdraw.Remove("Amount");
            withdraw.Remove("EPrice");


        }
    }
}




