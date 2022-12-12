

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/checkCreate.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUCheckCreate
    {
        [TestMethod]
        public async Task TestVerify_Valid_CheckCreate()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy"},
                {"SendMax", "100000000"}, //todo in tests must be Issued Currency
                {"Expiration", 570113521u},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"DestinationTag", 1u},
                {"Fee", "12"},
            };
            await Validation.ValidateCheckCreate(tx);
            await Validation.Validate(tx);
        }
        [TestMethod]
        public async Task TestVerify_InValid_Destination()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", 7896214789632154},
                {"SendMax", "100000000"}, //todo in tests must be Issued Currency
                {"Expiration", 570113521u},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"DestinationTag", 1u},
                {"Fee", "12"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCreate(tx), "CheckCreate: invalid Destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx), "CheckCreate: invalid Destination");
        }
        [TestMethod]
        public async Task TestVerify_InValid_SendMax()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy"},
                {"SendMax", 100000000},
                {"Expiration", 570113521u},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"DestinationTag", 1u},
                {"Fee", "12"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCreate(tx), "CheckCreate: invalid SendMax");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx), "CheckCreate: invalid SendMax");
        }
        [TestMethod]
        public async Task TestVerify_InValid_DestinationTag()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", 7896214789632154},
                {"SendMax", "100000000"}, //todo in tests must be Issued Currency
                {"Expiration", 570113521u},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"DestinationTag", "1"},
                {"Fee", "12"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCreate(tx), "CheckCreate: invalid DestinationTag");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx), "CheckCreate: invalid DestinationTag");
        }
        [TestMethod]
        public async Task TestVerify_InValid_Expiration()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy"},
                {"SendMax", "100000000"}, //todo in tests must be Issued Currency
                {"Expiration", "570113521"},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"DestinationTag", 1u},
                {"Fee", "12"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCreate(tx), "CheckCreate: invalid Expiration");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx), "CheckCreate: invalid Expiration");
        }
        [TestMethod]
        public async Task TestVerify_InValid_InvoiceID()
        {
            var tx = new Dictionary<string, dynamic>
            {
                { "TransactionType", "CheckCreate" },
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Destination", "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy"},
                {"SendMax", "100000000"}, //todo in tests must be Issued Currency
                {"Expiration", 570113521u},
                {"InvoiceID", 789656963258531},
                {"DestinationTag", 1u},
                {"Fee", "12"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidateCheckCreate(tx), "CheckCreate: invalid InvoiceID");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(tx), "CheckCreate: invalid InvoiceID");
        }
    }

}

