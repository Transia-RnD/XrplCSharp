

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/hashes.ts

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Utils.Hashes;

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class TestHashes
    {
        [TestMethod]
        public void TestLedgerSpaceHex()
        {
            var expectedEntryHex = "0078";
            Debug.WriteLine(LedgerSpace.Paychan);
            var actualEntryHex = Hashes.LedgerSpaceHex(LedgerSpace.Paychan);

            Assert.AreEqual(expectedEntryHex, actualEntryHex);
        }

        [TestMethod]
        public void TestAddressToHex()
        {
            var account = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
            var expectedEntryHex = "B5F762798A53D543A014CAF8B297CFF8F2F937E8";
            var actualEntryHex = Hashes.AddressToHex(account);

            Assert.AreEqual(expectedEntryHex, actualEntryHex);
        }

        //[TestMethod]
        //public void TestAccountRootEntryHash()
        //{
        //    var account = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
        //    var expectedEntryHash = "2B6AC232AA4C4BE41BF49D2459FA4A0347E1B543A4C92FCEE0821C0201E2E9A8";
        //    var actualEntryHash = HashAccountRoot(account);

        //    Assert.AreEqual(actualEntryHash, expectedEntryHash);
        //}

        [TestMethod]
        public void TestPaymentChannelEntryHash()
        {
            var account = "rDx69ebzbowuqztksVDmZXjizTd12BVr4x";
            var dstAccount = "rLFtVprxUEfsH54eCWKsZrEQzMDsx1wqso";
            var sequence = 82;
            var expectedEntryHash = "E35708503B3C3143FB522D749AAFCC296E8060F0FB371A9A56FAE0B1ED127366";
            var actualEntryHash = Hashes.HashPaymentChannel(account, dstAccount, sequence);
            Assert.AreEqual(expectedEntryHash, actualEntryHash);
        }

        [TestMethod]
        public void TestSequence()
        {
            var expected = "00000052";
            var sequence = 82;
            var BYTE_LENGTH = 4;
            var actual = sequence.ToString("X").PadLeft(BYTE_LENGTH * 2, '0');
            Assert.AreEqual(expected, actual);
        }
    }
}

