

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/accountDelete.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUAccountDelete
    {
        [TestMethod]
        public async Task TestVerify_Valid_AccountDelete()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "AccountDelete" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "rPT1Sjq2YGrBMTttX4GZHjKu9dyfzbpAYe"},
                {"DestinationTag", 13u},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", 2147483648u},
            };
            await Validation.ValidateAccountDelete(tx);
        }
        [TestMethod]
        public async Task TestVerify_InValid_missing_Destination()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "AccountDelete" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateAccountDelete(tx), "AccountDelete: missing field Destination - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "AccountDelete: missing field Destination - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Destination()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "AccountDelete" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", 65478965},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateAccountDelete(tx), "AccountDelete: invalid Destination - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "AccountDelete: invalid Destination - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_DestinationTag()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "AccountDelete" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "rPT1Sjq2YGrBMTttX4GZHjKu9dyfzbpAYe"},
                {"DestinationTag", "sdfsdfdsf"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.ValidateAccountDelete(tx), "AccountDelete: invalid DestinationTag - no ERROR");
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(tx), "AccountDelete: invalid DestinationTag - no ERROR");
        }

    }
}

