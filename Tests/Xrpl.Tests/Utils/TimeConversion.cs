

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/timeConversion.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using Xrpl.Utils;

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class TestUTimeConversionTests
    {
        [TestMethod]
        public void TestRippleTimeToISOTime()
        {
            var rippleTime = 0;
            var isoTime = "2000-01-01T00:00:00.000Z";
            Assert.AreEqual(isoTime, DateTimeUtils.RippleTimeToISOTime(rippleTime));
        }

        [TestMethod]
        public void TestISOTimeToRippleTime()
        {
            var rippleTime = 0;
            var isoTime = "2000-01-01T00:00:00.000Z";
            Assert.AreEqual(DateTimeUtils.ISOTimeToRippleTime(isoTime), rippleTime);
        }

        [TestMethod]
        public void TestISOTimeToRippleTimeFromDate()
        {
            var rippleTime = 0;
            var isoTime = "2000-01-01T00:00:00.000Z";
            var date = DateTime.ParseExact(isoTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture).ToUniversalTime();
            Assert.AreEqual(rippleTime, DateTimeUtils.ISOTimeToRippleTime(date));
        }

        [TestMethod]
        public void TestUnixTimeToRippleTime()
        {
            var unixTime = 946684801000;
            var rippleTime = 1;
            Assert.AreEqual(DateTimeUtils.UnixTimeToRippleTime(unixTime), rippleTime);
        }

        [TestMethod]
        public void TestRippleTimeToUnixTime()
        {
            var unixTime = 946684801000;
            var rippleTime = 1;
            Assert.AreEqual(DateTimeUtils.RippleTimeToUnixTime(rippleTime), unixTime);
        }
    }
}

