using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Ripple.Core;

namespace Ripple.TxSigning.Tests
{
    [TestClass]
    public class TxSignerPaymentTransactionTests
    {
        TxSigner signer;

        // secret and account pair
        static string fromAccount = "r99V1djgizxfcscPUpUbbR2C2akai7UXDe";
        static string fromAccountSecret = "snR7czZRBW5tRPTNsnhS1UpY3vx5X";

        static string destinationAccount = "rQ3rK7n7wy9GrDnT221wDVXxVGoGi95jyz";

        [TestInitialize]
        public void TestInitialize()
        {
            signer = TxSigner.FromSecret(fromAccountSecret);
        }

        [TestMethod]
        public void SignJson_MinimalValidTransaction_ValidSignedTx()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = "100000",
            });

            var r = signer.SignJson(json);

            AssertValidSignedTx(r);
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_XrpAmountTooLarge_ThrowsInvalidTxException()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = "10000000000000000000000000000000000000000",
            });

            var r = signer.SignJson(json);
        }

        [TestMethod]
        public void SignJson_ValidIouAmount_ValidSignedTx()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = new { value = "100000", currency = "USD", issuer = fromAccount },
            });

            var r = signer.SignJson(json);

            AssertValidSignedTx(r);
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_AmountObjectHasPascalCaseProperties_ThrowsInvalidTxException()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = new { Value = "100000", Currency = "USD", Issuer = fromAccount },
            });

            var r = signer.SignJson(json);
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_AmountObjectHasCounterpartyInsteadOfIssuer_ThrowsInvalidTxException()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = new { value = "100000", currency = "USD", counterparty = fromAccount },
            });

            var r = signer.SignJson(json);
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_AmountObjectHasExtraProperty_ThrowsInvalidTxException()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = new { value = "100000", currency = "USD", issuer = fromAccount, extra = 1234 },
            });

            var r = signer.SignJson(json);
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_IouValuePrecisionTooHigh_ThrowsInvalidTxException()
        {
            var json = JObject.FromObject(new {
                TransactionType = "Payment",
                Account = fromAccount,
                Destination = destinationAccount,
                Fee = "100",
                Sequence = 10,
                Amount = new { value = "123456789012345678", currency = "USD", issuer = fromAccount },
            });

            var r = signer.SignJson(json);
        }

        private void AssertValidSignedTx(SignedTx signedTx)
        {
            Assert.IsNotNull(signedTx, "SignedTx can't be null");
            Assert.IsNotNull(signedTx.TxJson, "SignedTx.TxJson can't be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(signedTx.TxBlob), "SignedTx.TxBlob can't be null or whitespace");
            Assert.IsFalse(string.IsNullOrWhiteSpace(signedTx.Hash), "SignedTx.Hash can't be null or whitespace");
        }
    }
}
