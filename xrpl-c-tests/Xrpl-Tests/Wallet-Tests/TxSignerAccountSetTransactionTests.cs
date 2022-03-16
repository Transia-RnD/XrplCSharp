using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Ripple.TxSigning.Tests
{
    [TestClass]
    public class TxSignerAccountSetTransactionTests
    {
        TxSigner signer;

        // secret and account pair
        static string accountSecret = "snR7czZRBW5tRPTNsnhS1UpY3vx5X";
        static string account = "r99V1djgizxfcscPUpUbbR2C2akai7UXDe";

        [TestInitialize]
        public void TestInitialize()
        {
            signer = TxSigner.FromSecret(accountSecret);
        }

        #region Tests for missing properties

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_MissingAccountProperty_ThrowsInvalidTxException()
        {
            signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Fee = "100", Sequence = 10 }));
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_MissingFeeProperty_ThrowsInvalidTxException()
        {
            signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Account = account, Sequence = 10 }));
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_MissingSequenceProperty_ThrowsInvalidTxException()
        {
            signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Account = account, Fee = "100" }));
        }

        #endregion

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_InvalidProperty_ThrowsInvalidTxException()
        {
            signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Account = account, Fee = "100", Sequence = 10, InvalidProperty = 100 }));
        }

        [TestMethod, ExpectedInvalidTxException]
        public void SignJson_InvalidAccountFormat_ThrowsInvalidTxException()
        {
            var invalidAccount = "rINVALID";

            signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Account = invalidAccount, Fee = "100", Sequence = 10 }));
        }

        [TestMethod]
        public void SignJson_ValidMinimalAccountSetTransaction_ValidSignedTx()
        {
            var r = signer.SignJson(JObject.FromObject(new { TransactionType = "AccountSet", Account = account, Fee = "100", Sequence = 10 }));

            AssertValidSignedTx(r);
        }

        private void AssertValidSignedTx(SignedTx signedTx)
        {
            Assert.IsNotNull(signedTx, "SignedTx can't be null");
            Assert.IsNotNull(signedTx.TxJson, "SignedTx.TxJson can't be null");
            Assert.IsNotNull(signedTx.TxBlob, "SignedTx.TxBlob can't be null");
            Assert.IsNotNull(signedTx.Hash, "SignedTx.Hash can't be null");
        }
    }
}
