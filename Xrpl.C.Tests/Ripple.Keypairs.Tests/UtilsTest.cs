using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Keypairs.Extensions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/test/utils-test.js

namespace Ripple.Keypairs.Tests
{
    [TestClass]
    public class UtilsTest
    {
        public bool Equality(byte[] a1, byte[] b1)
        {
            int i;
            if (a1.Length == b1.Length)
            {
                i = 0;
                while (i < a1.Length && (a1[i] == b1[i])) //Earlier it was a1[i]!=b1[i]
                {
                    i++;
                }
                if (i == a1.Length)
                {
                    return true;
                }
            }

            return false;
        }

        [TestMethod]
        public void HexToBytesEmptyTest()
        {
            Assert.AreEqual(Equality("".FromHex(), new byte[0]), true);
        }

        [TestMethod]
        public void HexToBytesZeroTest()
        {
            Assert.AreEqual(Equality("000000".FromHex(), new byte[] { 0x0, 0x0, 0x0 }), true);
        }

        [TestMethod]
        public void HexToBytesDEEDBEEFTest()
        {
            Assert.AreEqual(Equality("DEADBEEF".FromHex(), new byte[] { 222, 173, 190, 239 }), true);
        }

        [TestMethod]
        public void BytesToHexDEEDBEEFTest()
        {
            Assert.AreEqual(new byte[] { 222, 173, 190, 239 }.ToHex(), "DEADBEEF");
        }
    }
}

