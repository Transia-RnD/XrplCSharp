using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ripple.Core.Types;
using Ripple.Core.Util;

namespace Ripple.Core.Tests
{
    [TestClass]
    public class AmountValueTests
    {
        [TestMethod]
        public void TestScribble()
        {
            AssertValue("0000", "0", 0, false);
            AssertValue("-000.1", "1000,0000,0000,0000", -16, true);
            AssertValue("-.1", "1000,0000,0000,0000", -16, true, precision:1);
            AssertValue(".1", "1000,0000,0000,0000", -16, isNegative: false, precision:1);
            AssertValue("9999,9999,9999,9999", "9999,9999,9999,9999", 0, false, precision: 16);
            AssertValue("9999,9999", "9999,9999,0000,0000", -8, false, precision: 8);
            AssertValue("99.123", "9912,3000,0000,0000", -14, false, precision: 5);
            AssertValue("0.00123", "1230,0000,0000,0000", -18, false, precision: 3);
            AssertValue("120000", "1200,0000,0000,0000", -10, false, precision:2);

            //00 80C6 A47E 8D03//
            //FF FFC0 6FF2 8623
        }

        [TestMethod]
        public void IllegalAmountTest()
        {
            var thatDamnOffer = "1000000000000000100";
            var val = AmountValue.FromString(thatDamnOffer, native: true);
            Assert.AreEqual(thatDamnOffer, val.ToString());
        }

        [TestMethod]
        public void ExponentTest()
        {
            AssertExponent(".9999999999999999", -16);
            AssertExponent("9.999999999999999", -15);
            AssertExponent("99.99999999999999", -14);
            AssertExponent("999.9999999999999", -13);
            AssertExponent("9999.999999999999", -12);
            AssertExponent("99999.99999999999", -11);
            AssertExponent("999999.9999999999", -10);
            AssertExponent("9999999.999999999", -9);
            AssertExponent("99999999.99999999", -8);
            AssertExponent("999999999.9999999", -7);
            AssertExponent("9999999999.999999", -6);
            AssertExponent("99999999999.99999", -5);
            AssertExponent("999999999999.9999", -4);
            AssertExponent("9999999999999.999", -3);
            AssertExponent("99999999999999.99", -2);
            AssertExponent("999999999999999.9", -1);

            AssertExponent(".9", -16);

            AssertExponent("9", -15);
            AssertExponent("99", -14);
            AssertExponent("999", -13);
            AssertExponent("9999", -12);
            AssertExponent("99999", -11);
            AssertExponent("999999", -10);
            AssertExponent("9999999", -9);
            AssertExponent("99999999", -8);
            AssertExponent("999999999", -7);
            AssertExponent("9999999999", -6);
            AssertExponent("99999999999", -5);
            AssertExponent("999999999999", -4);
            AssertExponent("9999999999999", -3);
            AssertExponent("99999999999999", -2);
            AssertExponent("999999999999999", -1);
            AssertExponent("9999999999999999", 0);

        }

        [TestMethod, ExpectedException(typeof(PrecisionException))]
        public void PrecisionTest()
        {
            IouValue.FromString("1234" + "1234" + "1234" + "1234" + 1);
        }

        [TestMethod, ExpectedException(typeof(InvalidAmountValueException))]
        public void InvalidAmountValueTest()
        {
            IouValue.FromString("silly");
        }

        private static void AssertExponent(string valueString, int expectedExponent)
        {
            Assert.AreEqual(IouValue.FromString(valueString).Exponent, expectedExponent);
        }

        public void AssertValue(string value, string mantissa, int exponent, bool isNegative, int? precision = null)
        {
            Normalize(ref mantissa);
            Normalize(ref value);

            var val = IouValue.FromString(value);

            Assert.AreEqual(mantissa, val.Mantissa.ToString(), $"mantissa for {value}");
            Assert.AreEqual(exponent, val.Exponent, $"exponent for {value}");
            Assert.AreEqual(isNegative, val.IsNegative, $"isNegative for {value}");

            if (precision != null)
            {
                Assert.AreEqual(precision, val.Precision, $"precision for {value}");
            }
        }

        public static void Normalize(ref string v)
        {
            v = v.Replace(",", "");
        }
    }
}
