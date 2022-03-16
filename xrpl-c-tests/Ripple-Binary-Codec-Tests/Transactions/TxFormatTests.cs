using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ripple.Core.Enums;
using Ripple.Core.Types;

namespace Ripple.Core.Tests
{
    [TestClass]
    public class TxFormatTests
    {
        [TestMethod, ExpectedException(typeof(TxFormatValidationException))]
        public void MissingFieldsTest()
        {
            var so = new StObject
            {
                [Field.TransactionType] = TransactionType.Payment
            };
            TxFormat.Validate(so);
        }

        [TestMethod]
        public void ValidateTest()
        {
            var so = new StObject
            {
                [Field.TransactionType] = TransactionType.Payment,
                [Field.Account] = "r9kiSEUEw6iSCNksDVKf9k3AyxjW3r1qPf",
                [Field.Sequence] = 12000,
                [Field.SigningPubKey] = Blob.FromHex("02517EB0EEB4424BB01D6D9932F8838D8EF4EC989CF5B8097C9CF675F534EF10BF"),
                [Field.Fee] = "12000",
                [Field.Destination] = "r9kiSEUEw6iSCNksDVKf9k3AyxjW3r1qPf",
                [Field.Amount] = "100"
            };

            // No problems here
            TxFormat.Validate(so);
            // But if we set the amount to null
            so[Field.Amount] = null;

            AssertTxFormatException(() =>
            {
                TxFormat.Validate(so);
            }, "`Amount` is set to null");
        }

        [TestMethod]
        public void MissingTransactionTypeTest()
        {
            AssertTxFormatException(() => {
                TxFormat.Validate(new StObject());
            }, "Missing `TransactionType` field");
        }

        [TestMethod]
        public void NullTransactionTypeTest()
        {
            AssertTxFormatException(() => {
                TxFormat.Validate(new StObject {[Field.TransactionType] = null});
            }, "`TransactionType` is set to null");
        }

        public static void AssertTxFormatException(Action a, string messagePattern=null)
        {
            AssertException<TxFormatValidationException>(a, messagePattern);
        }

        public static void AssertException<T> (Action a, string messagePattern=null) where T: Exception
        {
            Exception thrown = null;
            try
            {
                a();
            }
            catch (Exception e)
            {
                thrown = e;
            }
            Assert.IsNotNull(thrown);
            if (messagePattern != null)
            {
                Assert.IsTrue(Regex.IsMatch(thrown.Message, messagePattern),
                              $"Exception message did not match `{messagePattern}`\n" +
                              $"Message: {thrown.Message}");
            }
            Assert.IsInstanceOfType(thrown, typeof(T));
        }
    }
}
