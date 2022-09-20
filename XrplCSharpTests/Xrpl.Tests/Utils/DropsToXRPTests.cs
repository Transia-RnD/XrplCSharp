using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Models.Transactions;
using Xrpl.Utils;
using Xrpl.XrplWallet;
using static Xrpl.Client.Models.Common.Common;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/dropsToXrp.ts

namespace Xrpl.Tests.Client.Tests
{
    [TestClass]
    public class TestUDropsToXrp
    {
        [TestMethod]
        public void TestDropsToXRP()
        {
            string xrp = XrpConversion.DropsToXrp("2000000");
            Assert.AreEqual(xrp, "2");
        }

        [TestMethod]
        public void TestDropsToXRPFractions()
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
        public void TestDropsToXRPZero()
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
        public void TestDropsToXRPNegative()
        {
            var xrp = XrpConversion.DropsToXrp("-2000000");
            Assert.AreEqual("-2", xrp);
        }

        [TestMethod]
        public void TestDropsToXRPDecimal()
        {
            var xrp = XrpConversion.DropsToXrp("2000000.");
            Assert.AreEqual("2", xrp);

            xrp = XrpConversion.DropsToXrp("-2000000.");
            Assert.AreEqual("-2", xrp);
        }

        // SKIPPING FROM BIGNUMBER AS WE USE BIGINT

        [TestMethod]
        public void TestDropsToXRPDouble()
        {
            var xrp = XrpConversion.DropsToXrp(2000000);
            Assert.AreEqual("2", xrp);

            xrp = XrpConversion.DropsToXrp(-2000000);
            Assert.AreEqual("-2", xrp);
        }

        [TestMethod]
        public void TestDropsToXRPSCINotation()
        {
            var xrp = XrpConversion.DropsToXrp("1e6");
            Assert.AreEqual("1", xrp);
        }

        [TestMethod]
        public void TestInvalidDropsToXRPDecimalError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1.2"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("0.10"));
        }

        [TestMethod]
        public void TestInvalidDropsToXRPValueError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("FOO"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1e-7"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("2,0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("."));
        }

        [TestMethod]
        public void TestInvalidDropsToXRPMultipleDecimalPointError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("1.0.0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.DropsToXrp("..."));
        }
    }
}

