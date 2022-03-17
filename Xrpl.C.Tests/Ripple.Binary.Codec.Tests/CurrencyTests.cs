using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Binary.Codec.Types;
using System;

namespace Ripple.Binary.Codec.Tests
{
    [TestClass]
    public class CurrencyTests
    {
        [TestMethod]
        public void UsdTest()
        {
            Currency c = "USD";
            Assert.IsFalse(c.IsNative);
            Assert.AreEqual("USD", c.IsoCode);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void InvalidCurrencyTest()
        {
            Currency c = "ABCD";
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void InvalidHexTest()
        {
            Currency c = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZ";
        }

    }
}