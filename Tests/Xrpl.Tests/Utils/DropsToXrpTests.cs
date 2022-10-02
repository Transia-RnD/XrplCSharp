using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/dropsToXrp.ts

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class TestUDropsToXrp
    {
        [TestMethod]
        public void TestDropsToXrp()
        {
            string xrp = XrpConversion.DropsToXrp("2000000");
            Assert.AreEqual(xrp, "2");
        }

        [TestMethod]
        public void TestDropsToXrpFractions()
        {
            var xrp = XrpConversion.DropsToXrp("3456789");
            Assert.AreEqual("3.456789", xrp);

            xrp = XrpConversion.DropsToXrp("3400000");
            Assert.AreEqual("3.4", xrp);

            xrp = XrpConversion.DropsToXrp("1");
            Assert.AreEqual("0.000001", xrp);

            xrp = XrpConversion.DropsToXrp("1.0");
            Assert.AreEqual("0.000001", xrp);

            xrp = XrpConversion.DropsToXrp("1.00");
            Assert.AreEqual("0.000001", xrp);
        }

        [TestMethod]
        public void TestDropsToXrpZero()
        {
            var xrp = XrpConversion.DropsToXrp("0");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.DropsToXrp("-0");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.DropsToXrp("0.00");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.DropsToXrp("000000000");
            Assert.AreEqual("0", xrp);
        }

        [TestMethod]
        public void TestDropsToXrpNegative()
        {
            var xrp = XrpConversion.DropsToXrp("-2000000");
            Assert.AreEqual("-2", xrp);
        }

        [TestMethod]
        public void TestDropsToXrpDecimal()
        {
            var xrp = XrpConversion.DropsToXrp("2000000.");
            Assert.AreEqual("2", xrp);

            xrp = XrpConversion.DropsToXrp("-2000000.");
            Assert.AreEqual("-2", xrp);
        }

        // SKIPPING FROM BIGNUMBER AS WE USE BIGINT

        [TestMethod]
        public void TestDropsToXrpDouble()
        {
            var xrp = XrpConversion.DropsToXrp(2000000);
            Assert.AreEqual("2", xrp);

            xrp = XrpConversion.DropsToXrp(-2000000);
            Assert.AreEqual("-2", xrp);
        }

        [TestMethod]
        public void TestDropsToXrpSCINotation()
        {
            var xrp = XrpConversion.DropsToXrp("1e6");
            Assert.AreEqual("1", xrp);
        }

        [TestMethod]
        public void TestInvalidDropsToXrpDecimalError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1.2"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("0.10"));
        }

        [TestMethod]
        public void TestInvalidDropsToXrpValueError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("FOO"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1e-7"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("2,0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("."));
        }

        [TestMethod]
        public void TestInvalidDropsToXrpMultipleDecimalPointError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1.0.0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("..."));
        }
    }
}

