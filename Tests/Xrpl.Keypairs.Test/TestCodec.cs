using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.AddressCodec;
using static Xrpl.AddressCodec.XrplCodec;
using Xrpl.Keypairs;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-keypairs/test/codec-test.js

namespace Xrpl.Keypairs.Tests
{
    [TestClass]
    public class TestUCodec
    {

        [TestMethod]
        public void TestKeypairsEncodeAccountID()
        {
            string actual = XrplCodec.EncodeAccountID("BA8E78626EE42C41B46D46C3048DF3A1C3C87072".FromHex());
            Assert.AreEqual(actual, "rJrRMgiRgrU6hDF4pgu5DXQdWyPbY35ErN");
        }

        [TestMethod]
        public void TestKeypairsEncodeNodePublic()
        {
            string actual = XrplCodec.EncodeNodePublic("0388E5BA87A000CB807240DF8C848EB0B5FFA5C8E5A521BC8E105C0F0A44217828".FromHex());
            Assert.AreEqual(actual, "n9MXXueo837zYH36DvMc13BwHcqtfAWNJY5czWVbp7uYTj7x17TH");
        }

        [TestMethod]
        public void TestDecodeSeed()
        {
            DecodedSeed decoded = XrplCodec.DecodeSeed("sEdTM1uX8pu2do5XvTnutH6HsouMaM2");
            Assert.AreEqual(decoded.Bytes.ToHex(), "4C3A1D213FBDFB14C7C28D609469B341");
            Assert.AreEqual(decoded.Type, "ed25519");

            DecodedSeed decoded2 = XrplCodec.DecodeSeed("sn259rEFXrQrWyx3Q7XneWcwV6dfL");
            Assert.AreEqual(decoded2.Bytes.ToHex(), "CF2DE378FBDD7E2EE87D486DFB5A7BFF");
            Assert.AreEqual(decoded2.Type, "secp256k1");
        }

        [TestMethod]
        public void TestEncodeSeedWithType()
        {
            string edSeed = "sEdTM1uX8pu2do5XvTnutH6HsouMaM2";
            DecodedSeed decoded = XrplCodec.DecodeSeed("sEdTM1uX8pu2do5XvTnutH6HsouMaM2");
            Assert.AreEqual(decoded.Bytes.ToHex(), "4C3A1D213FBDFB14C7C28D609469B341");
            Assert.AreEqual(decoded.Type, "ed25519");
            Assert.AreEqual(XrplCodec.EncodeSeed(decoded.Bytes, decoded.Type), edSeed);
        }
    }
}