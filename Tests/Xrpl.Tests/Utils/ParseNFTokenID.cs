// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/parseNFTokenID.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec.Util;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.Utils;

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class TestParseNFT
    {
        [TestMethod]
        public void TestUParseNFTID()
        {
        }

        [TestMethod]
        public void TestUParseNFTOffer()
        {
            uint sequence = 68220799;
            string account = "rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p";

            string response = ParseNFTID.ParseNFTOffer(account, sequence);
            Assert.AreEqual(
                "B0D5A4CBA300C093A8BEE89C755E76819E0B96C586FB34DAC739EE2982F0D31A",
                response
            );
        }
    }
}