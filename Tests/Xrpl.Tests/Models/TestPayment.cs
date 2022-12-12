

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/payment.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    [TestClass]
    public class TestUPayment
    {
        public static Dictionary<string, dynamic> payment;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            payment = new Dictionary<string, dynamic>
            {
                {"TransactionType", "Payment"},
                {"Account", "rUn84CUYbNjRoTQ6mSW7BVJPSVJNLb1QLo"},
                {"Amount", "1234"},
                {"Destination", "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy"},
                {"DestinationTag", 1u},
                {"Fee", "12"},
                {"Flags", 2147483648u},
                {"LastLedgerSequence", 65953073u},
                {"Sequence", 65923914u},
                {"SigningPubKey", "02F9E33F16DF9507705EC954E3F94EB5F10D1FC4A354606DBE6297DBB1096FE654"},
                {"TxnSignature", "3045022100E3FAE0EDEC3D6A8FF6D81BC9CF8288A61B7EEDE8071E90FF9314CB4621058D10022043545CF631706D700CEE65A1DB83EFDD185413808292D9D90F14D87D3DC2D8CB"},
                {"InvoiceID", "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B"},
                {"Paths", new List<List<Dictionary<string, dynamic>>>(){new List<Dictionary<string, dynamic>>(){new Dictionary<string,dynamic>()
                {
                    {"currency", "BTC"},
                    {"issuer", "r9vbV3EHvXWjSkeQ6CAcYVPGeq7TuiXY2X"},
                }}} },
                {"SendMax", "100000000"},

            };
        }

        [TestMethod]
        public async Task TestVerifyValid()
        {

            //verifies valid PaymentTransaction
            await Validation.ValidatePayment(payment);
            await Validation.Validate(payment);

            // Verifies memos correctly
            payment["Memos"] = new List<Dictionary<string,dynamic>>(){new Dictionary<string, dynamic>()
            {
                {"MemoData", "32324324"},
            }};
            await Validation.Validate(payment);
            payment.Remove("Memos");

            // Verifies memos correctly
            payment["Memos"] = new List<Dictionary<string, dynamic>>(){new Dictionary<string, dynamic>()
            {
                {"MemoData", "32324324"},
                {"MemoType", 121221},
            }};
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "BaseTransaction: invalid Memos");
            payment.Remove("Memos");

            // throws when Amount is missing
            payment.Remove("Amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: missing Amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: missing Amount");
            payment["Amount"] = "1234";

            // throws when Amount is invalid
            payment["Amount"] = 1234;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: invalid Amount");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: invalid Amount");
            payment["Amount"] = "1234";

            // throws when Destination is missing
            payment.Remove("Destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: missing field Destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: missing field Destination");
            payment["Destination"] = "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy";

            // throws when Destination is invalid
            payment["Destination"] = 7896214;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: invalid Destination");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: invalid Destination");
            payment["Destination"] = "rfkE1aSy9G8Upk4JssnwBxhEv5p4mn2KTy";

            // throws when DestinationTag is not a number
            payment["DestinationTag"] = "1";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: DestinationTag must be a number");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: DestinationTag must be a number");
            payment["DestinationTag"] = 1u;

            // throws when InvoiceID is not a string
            payment["InvoiceID"] = 19832;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: InvoiceID must be a string");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: InvoiceID must be a string");
            payment["InvoiceID"] = "6F1DFD1D0FE8A32E40E1F2C05CF1C15545BAB56B617F9C6C2D63A6B704BEF59B";

            // throws when Paths is invalid
            payment["Paths"] = new List<List<Dictionary<string, dynamic>>>()
            {
                new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        { "account", 123 },
                    }
                }
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: invalid Paths");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: invalid Paths");
            payment["Paths"] = new List<List<Dictionary<string, dynamic>>>()
            {
                new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        { "currency", "BTC" },
                        { "issuer", "r9vbV3EHvXWjSkeQ6CAcYVPGeq7TuiXY2X" },
                    }
                }
            };

            // throws when SendMax is invalid
            payment["SendMax"] = 100000000;
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: invalid SendMax");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: invalid SendMax");
            payment["SendMax"] = "100000000";

            // verifies valid DeliverMin with tfPartialPayment flag set as a number
            payment["DeliverMin"] = "10000";
            payment["Flags"] = PaymentFlags.tfPartialPayment;
            await Validation.ValidatePayment(payment);
            await Validation.Validate(payment);
            payment["Flags"] = 2147483648u;
            payment.Remove("DeliverMin");

            // verifies valid DeliverMin with tfPartialPayment flag set as a boolean
            payment["DeliverMin"] = "10000";
            payment["Flags"] = new Dictionary<string, dynamic>()
            {
                { "tfPartialPayment", true },
            };
            await Validation.ValidatePayment(payment);
            await Validation.Validate(payment);
            payment["Flags"] = 2147483648u;
            payment.Remove("DeliverMin");

            //throws when DeliverMin is invalid
            payment["DeliverMin"] = 10000;
            payment["Flags"] = new Dictionary<string, dynamic>()
            {
                { "tfPartialPayment", true },
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: invalid DeliverMin");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: invalid DeliverMin");
            payment["Flags"] = 2147483648u;
            payment.Remove("DeliverMin");

            //throws when tfPartialPayment flag is missing with valid DeliverMin
            payment["DeliverMin"] = "10000";
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.ValidatePayment(payment), "PaymentTransaction: tfPartialPayment flag required with DeliverMin");
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(payment), "PaymentTransaction: tfPartialPayment flag required with DeliverMin");
            payment.Remove("DeliverMin");
        }
    }

}

