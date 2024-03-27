// https://github.com/XRPLF/xrpl.js/blob/amm-beta/packages/xrpl/test/models/AMMVote.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAMMVote
    {
        public static Dictionary<string, dynamic> vote;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            vote = new Dictionary<string, dynamic>
            {
                {"TransactionType", "AMMVote"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Asset", new Dictionary<string,dynamic>(){{"currency","XRP"}}},
                {"Asset2", new Dictionary<string,dynamic>(){{"currency","ETH"},{"issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" } }},
                {"TradingFee", 25u},
                {"Sequence", 1337u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid AMMVote
            await Validation.Validate(vote);

            //throws w/ missing field Asset
            vote.Remove("Asset");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: missing field Asset");
            vote["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            vote["Asset"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: Asset must be an Issue");
            vote["Asset"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };

            //throws w/ missing field Asset
            vote.Remove("Asset2");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: missing field Asset2");
            vote["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "XRP" } };
            //throws w/ Asset must be an Issue
            vote["Asset2"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: Asset2 must be an Issue");
            vote["Asset2"] = new Dictionary<string, dynamic>() { { "currency", "ETH" }, { "issuer", "rP9jPyP5kyvFRb6ZiRghAGw5u8SGAmU4bd" } };

            //throws w/ missing TradingFee
            vote.Remove("TradingFee");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: missing field TradingFee");
            vote["TradingFee"] = 12u;
            //throws w/ TradingFee must be a number
            vote["TradingFee"] = "12";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: TradingFee must be a number");
            vote["TradingFee"] = 12u;

            //throws when TradingFee is greater than 1000
            vote["TradingFee"] = 1001u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: TradingFee must be between 0 and 1000");
            vote["TradingFee"] = 12u;
            //throws when TradingFee is greater than 1000
            vote["TradingFee"] = -1;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(vote), "AMMVote: TradingFee must be between 0 and 1000");
            vote["TradingFee"] = 12u;

        }
    }
}



