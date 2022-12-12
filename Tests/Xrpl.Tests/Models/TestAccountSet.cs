

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/accountSet.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAccountSet
    {
        public static Dictionary<string, dynamic> accountSet;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            accountSet = new Dictionary<string, dynamic>
            {
                {"TransactionType", "AccountSet"},
                {"Account", "rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn"},
                {"Fee", "12"},
                {"Sequence", 5u},
                {"Domain", "6578616D706C652E636F6D"},
                {"SetFlag", 5u},
                {"MessageKey", "03AB40A0490F9B7ED8DF29D246BF2D6269820A0EE7742ACDD457BEA7C7D0931EDB"},
            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {
            //verifies valid AccountSet
            await Validation.ValidateAccountSet(accountSet);
            await Validation.Validate(accountSet);

            //throws w/ invalid SetFlag (out of range)
            accountSet["SetFlag"] = 12u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid SetFlag - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid SetFlag - no ERROR");

            //throws w/ invalid SetFlag (incorrect type)
            accountSet["SetFlag"] = "abc";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid SetFlag - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid SetFlag - no ERROR");

            accountSet["SetFlag"] = 5u;

            //throws w/ invalid ClearFlag
            accountSet["ClearFlag"] = 12u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid ClearFlag - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid ClearFlag - no ERROR");
            accountSet.Remove("ClearFlag");

            //throws w/ invalid Domain
            accountSet["Domain"] = 6578616;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid Domain - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid Domain - no ERROR");
            accountSet["Domain"] = "6578616D706C652E636F6D";

            //throws w/ invalid EmailHash
            accountSet["EmailHash"] = 6578656789876543;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid EmailHash - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid EmailHash - no ERROR");
            accountSet.Remove("EmailHash");

            //throws w/ invalid MessageKey
            accountSet["MessageKey"] = 6578656789876543;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid MessageKey - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid MessageKey - no ERROR");
            accountSet["MessageKey"] = "03AB40A0490F9B7ED8DF29D246BF2D6269820A0EE7742ACDD457BEA7C7D0931EDB";

            //throws w/ invalid TransferRate
            accountSet["TransferRate"] = "1000000001";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid TransferRate - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid TransferRate - no ERROR");
            accountSet.Remove("TransferRate");

            //throws w/ invalid TickSize
            accountSet["TickSize"] = 20u;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateAccountSet(accountSet), "AccountSet: invalid TickSize - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(accountSet), "AccountSet: invalid TickSize - no ERROR");
            accountSet.Remove("TickSize");
        }
    }
}

