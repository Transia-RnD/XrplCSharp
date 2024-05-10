// https://github.com/XRPLF/xrpl.js/blob/amm-beta/packages/xrpl/test/models/AMMCreate.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAMMCreate
    {
        public static Dictionary<string, dynamic> ammCreate;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            ammCreate = new Dictionary<string, dynamic>
            {
                {"TransactionType", "AMMCreate"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Amount", "1000"},
                {"Amount2", new Dictionary<string,dynamic>()
                {
                    {"currency","USD"},
                    {"issuer","rPyfep3gcLzkosKC9XiE77Y8DZWG6iWDT9"},
                    {"value","1000"},
                }},
                {"TradingFee", 12u},
                {"Sequence", 1337u},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid AMMCreate
            await Validation.Validate(ammCreate);

            //throws w/ missing Amount
            ammCreate.Remove("Amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: missing field Amount");
            ammCreate["Amount"] = "1000";
            //throws w/ Amount must be an Amount
            ammCreate["Amount"] = 1000;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: Amount must be an Amount");
            ammCreate["Amount"] = "1000";

            //throws w/ missing Amount2
            ammCreate.Remove("Amount2");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: missing field Amount2");
            ammCreate["Amount2"] = new Dictionary<string, dynamic>()
            {
                {"currency","USD"},
                {"issuer","rPyfep3gcLzkosKC9XiE77Y8DZWG6iWDT9"},
                {"value","1000"},
            };
            //throws w/ Amount must be an Amount2
            ammCreate["Amount2"] = 1000;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: Amount2 must be an Amount");
            ammCreate["Amount"] = new Dictionary<string, dynamic>()
            {
                {"currency","USD"},
                {"issuer","rPyfep3gcLzkosKC9XiE77Y8DZWG6iWDT9"},
                {"value","1000"},
            };
            //throws w/ missing TradingFee
            ammCreate.Remove("TradingFee");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: missing field TradingFee");
            ammCreate["TradingFee"] = 12u;
            //throws w/ TradingFee must be a number
            ammCreate["TradingFee"] = "12";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: TradingFee must be a number");
            ammCreate["TradingFee"] = 12u;

            //throws when TradingFee is greater than 1000
            ammCreate["TradingFee"] = 1001u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: TradingFee must be between 0 and 1000");
            ammCreate["TradingFee"] = 12u;
            //throws when TradingFee is greater than 1000
            ammCreate["TradingFee"] = -1;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(ammCreate), "AMMCreate: TradingFee must be between 0 and 1000");
            ammCreate["TradingFee"] = 12u;

        }
    }
}


