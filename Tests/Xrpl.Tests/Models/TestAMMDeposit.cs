// https://github.com/XRPLF/xrpl.js/blob/amm/packages/xrpl/test/models/AMMDeposit.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAMMDeposit
    {
        public static Dictionary<string, dynamic> LPTokenOut;
        public static Dictionary<string, dynamic> deposit;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            LPTokenOut = new Dictionary<string, dynamic>()
            {
                { "currency", "B3813FCAB4EE68B3D0D735D6849465A9113EE048" },
                { "issuer", "rH438jEAzTs5PYtV6CHZqpDpwCKQmPW9Cg" },
                { "value", "1000" },
            };
            deposit = new Dictionary<string, dynamic>
            {
                {"TransactionType", "AMMDeposit"},
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
            //verifies valid AMMDeposit with LPTokenOut
            deposit["LPTokenOut"] = LPTokenOut;
            deposit["Flags"] = AMMDepositFlags.tfLPToken;
            await Validation.Validate(deposit);
            deposit.Remove("LPTokenOut");
            deposit["Flags"] = 0u;


            //verifies valid AMMDeposit with Amount
            deposit["Amount"] = "1000";
            deposit["Flags"] = AMMDepositFlags.tfSingleAsset;
            await Validation.Validate(deposit);
            deposit.Remove("Amount");
            deposit["Flags"] = 0u;

            //verifies valid AMMDeposit with Amount and Amount2
            deposit["Amount"] = "1000";
            deposit["Amount2"] = new Dictionary<string, dynamic>()
            {
                {"currency","ETH"},
                {"issuer","rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd"},
                {"value","2.5"},
            };
            deposit["Flags"] = AMMDepositFlags.tfTwoAsset;
            await Validation.Validate(deposit);
            deposit.Remove("Amount");
            deposit.Remove("Amount2");
            deposit["Flags"] = 0u;


            //verifies valid AMMDeposit with Amount and LPTokenOut
            deposit["Amount"] = "1000";
            deposit["LPTokenOut"] = LPTokenOut;
            deposit["Flags"] = AMMDepositFlags.tfOneAssetLPToken;
            await Validation.Validate(deposit);
            deposit.Remove("Amount");
            deposit.Remove("LPTokenOut");
            deposit["Flags"] = 0u;

            //verifies valid AMMDeposit with Amount and EPrice
            deposit["Amount"] = "1000";
            deposit["EPrice"] = "25";
            deposit["Flags"] = AMMDepositFlags.tfLimitLPToken;
            await Validation.Validate(deposit);
            deposit.Remove("Amount");
            deposit.Remove("EPrice");
            deposit["Flags"] = 0u;

            //throws w/ missing field Asset
            deposit.Remove("Asset");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: missing field Asset");
            deposit["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            deposit["Asset"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: Asset must be an Issue");
            deposit["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };

            //throws w/ missing field Asset
            deposit.Remove("Asset2");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: missing field Asset2");
            deposit["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            deposit["Asset2"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: Asset2 must be an Issue");
            deposit["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "ETH" }, { "issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" } };

            //throws w/ must set at least LPTokenOut or Amount
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: must set at least LPTokenOut or Amount");

            //throws w/ must set Amount with Amount2
            deposit["Amount2"] = new Dictionary<string, dynamic>()
            {
                { "currency", "ETH" },
                { "issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" },
                { "value", "2.5" },
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: must set Amount with Amount2");
            deposit.Remove("Amount2");

            //throws w/ must set Amount with EPrice
            deposit["EPrice"] = "25";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: must set Amount with EPrice");
            deposit.Remove("EPrice");

            //throws w/ LPTokenOut must be an IssuedCurrencyAmount
            deposit["LPTokenOut"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: LPTokenOut must be an IssuedCurrencyAmount");
            deposit.Remove("LPTokenOut");

            //throws w/ Amount must be an Amount
            deposit["Amount"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: Amount must be an Amount");
            deposit.Remove("Amount");

            //throws w/ Amount2 must be an Amount
            deposit["Amount"] = "1000";
            deposit["Amount2"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: Amount2 must be an Amount");
            deposit.Remove("Amount");
            deposit.Remove("Amount2");

            //throws w/ EPrice must be an Amount
            deposit["Amount"] = "1000";
            deposit["EPrice"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(deposit), "AMMDeposit: EPrice must be an Amount");
            deposit.Remove("Amount");
            deposit.Remove("EPrice");


        }
    }
}



