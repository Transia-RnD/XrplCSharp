

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/checkCancel.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUCheckCancel
    {
        [TestMethod]
        public async Task TestVerify_Valid_CheckCancel()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCancel" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"CheckID", "49647F0D748DC3FE26BDACBC57F251AADEFFF391403EC9BF87C97F67E9977FB0"},
            };
            await Validation.ValidateCheckCancel(tx);
            await Validation.Validate(tx);
        }
        [TestMethod]
        public async Task TestVerify_InValid_CheckID()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCancel" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm" },
                {"CheckID", 4964734566545678 }, //todo no check for CheckID size
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCancel(tx));
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx));
        }
    }

}

