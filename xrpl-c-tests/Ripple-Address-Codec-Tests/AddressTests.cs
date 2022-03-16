using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ripple.Address.Tests
{
    using static AddressCodec;

    [TestClass]
    public class AddressTests
    {
        [TestMethod]
        public void NodePublicTest()
        {
            AssertEncodes("NodePublic",
              ("0388E5BA87A000CB807240DF8C848EB0B" +
               "5FFA5C8E5A521BC8E105C0F0A44217828"),
              "n9MXXueo837zYH36DvMc13BwHcqtfAWNJY5czWVbp7uYTj7x17TH");
        }

        [TestMethod]
        public void SeedTest()
        {
            var decodedKoblitz = DecodeSeed("sn259rEFXrQrWyx3Q7XneWcwV6dfL");
            Assert.AreEqual(decodedKoblitz.Bytes.Length, 16);
            Assert.AreEqual(decodedKoblitz.Type, "secp256k1");

            AssertEncodes("K256Seed",
                  "CF2DE378FBDD7E2EE87D486DFB5A7BFF",
                  "sn259rEFXrQrWyx3Q7XneWcwV6dfL");

            var decoded = DecodeSeed("sEdTM1uX8pu2do5XvTnutH6HsouMaM2");
            Assert.AreEqual(decoded.Bytes.Length, 16);
            Assert.AreEqual(decoded.Type, "ed25519");

            AssertEncodes(
                "EdSeed",
                "4C3A1D213FBDFB14C7C28D609469B341",
                 "sEdTM1uX8pu2do5XvTnutH6HsouMaM2");
        }

        [TestMethod]
        public void AddressTest()
        {
            AssertEncodes(
                "Address",
                "0000000000000000000000000000000000000000",
                "rrrrrrrrrrrrrrrrrrrrrhoLvTp");

            AssertEncodes(
                "Address",
                "BA8E78626EE42C41B46D46C3048DF3A1C3C87072",
                "rJrRMgiRgrU6hDF4pgu5DXQdWyPbY35ErN");
        }

        public void AssertEncodes(string type, string hex, string base58)
        {
            Type kls = typeof(AddressCodec);
            var encode = kls.GetMethod("Encode" + type);
            var decode = kls.GetMethod("Decode" + type);
            var validate = kls.GetMethod("IsValid" + type);
            var encoded = Helpers.Invoke<string>(encode, Helpers.DecodeHex(hex));
            var decoded = Helpers.Invoke<byte[]>(decode, base58);
            var valid = Helpers.Invoke<bool>(validate, base58);
            var hexIsValidB58 = Helpers.Invoke<bool>(validate, hex);

            Assert.AreEqual(base58, encoded);
            Assert.AreEqual(hex, Helpers.EncodeHex(decoded));
            Assert.IsTrue(valid);
            Assert.IsFalse(hexIsValidB58);
        }
    }
}