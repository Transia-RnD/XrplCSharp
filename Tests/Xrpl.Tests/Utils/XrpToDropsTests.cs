using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client.Exceptions;
using Xrpl.Utils;

// https://github.com/XrpLF/xrpl.js/blob/main/packages/xrpl/test/utils/dropsToXrp.ts

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class TestUXrpToDrops
    {
        [TestMethod]
        public void TestXrpToDrops()
        {
            string xrp = XrpConversion.XrpToDrops("2");
            Assert.AreEqual("2000000", xrp);
        }

        [TestMethod]
        public void TestXrpToDropsFractions()
        {
            var xrp = XrpConversion.XrpToDrops("3.456789");
            Assert.AreEqual("3456789", xrp);

            xrp = XrpConversion.XrpToDrops("3.4");
            Assert.AreEqual("3400000", xrp);

            xrp = XrpConversion.XrpToDrops("0.000001");
            Assert.AreEqual("1", xrp);

            xrp = XrpConversion.XrpToDrops("0.0000010");
            Assert.AreEqual("1", xrp);
        }

        [TestMethod]
        public void TestXrpToDropsZero()
        {
            var xrp = XrpConversion.XrpToDrops("0");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.XrpToDrops("-0");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.XrpToDrops("0.00");
            Assert.AreEqual("0", xrp);

            xrp = XrpConversion.XrpToDrops("000000000");
            Assert.AreEqual("0", xrp);
        }

        [TestMethod]
        public void TestXrpToDropsNegative()
        {
            var xrp = XrpConversion.XrpToDrops("-2");
            Assert.AreEqual("-2000000", xrp);
        }

        [TestMethod]
        public void TestXrpToDropsDecimal()
        {
            var xrp = XrpConversion.XrpToDrops("2.");
            Assert.AreEqual("2000000", xrp);

            xrp = XrpConversion.XrpToDrops("-2.");
            Assert.AreEqual("-2000000", xrp);
        }

        // SKIPPING FROM BIGNUMBER AS WE USE BIGINT

        //[TestMethod]
        //public void TestXrpToDropsDouble()
        //{
        //    var xrp = XrpConversion.XrpToDrops(2000000);
        //    Assert.AreEqual("2", xrp);

        //    xrp = XrpConversion.XrpToDrops(-2000000);
        //    Assert.AreEqual("-2", xrp);
        //}

        [TestMethod]
        public void TestXrpToDropsSCINotation()
        {
            var xrp = XrpConversion.XrpToDrops("1e-6");
            Assert.AreEqual("1", xrp);
        }

        [TestMethod]
        public void TestInvalidXrpToDropsDecimalError()
        {
            Assert.ThrowsException<ValidationException>(() => XrpConversion.XrpToDrops("1.1234567"));
            Assert.ThrowsException<ValidationException>(() => XrpConversion.XrpToDrops("0.0000001"));
        }

        [TestMethod]
        public void TestInvalidXrpToDropsValueError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.XrpToDrops("FOO"));
            Assert.ThrowsException<ValidationException>(() => XrpConversion.XrpToDrops("1e-7"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.XrpToDrops("2,0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.XrpToDrops("."));
        }

        [TestMethod]
        public void TestInvalidXrpToDropsMultipleDecimalPointError()
        {
            Assert.ThrowsException<FormatException>(() => XrpConversion.XrpToDrops("1.0.0"));
            Assert.ThrowsException<FormatException>(() => XrpConversion.XrpToDrops("..."));
        }
    }
}

