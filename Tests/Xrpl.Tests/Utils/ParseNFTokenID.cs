// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/parseNFTokenID.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec.Util;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.Utils;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Models.Common;
using Xrpl.Utils;

namespace XrplTests.Xrpl.Utils
{
    [TestClass]
    public class ParseNFTokenID
    {
        [TestMethod]
        public void ParsingNFTokenID()
        {
            const string nftokenId = "000813886377BBDA772433D7FCF16A9710D9D958D9F7129F376D5FC200005026";

            NFTokenIDData nftokenIDData = nftokenId.ParseNFTokenID();

            Assert.AreEqual((uint)8, nftokenIDData.Flags);
            Assert.AreEqual("rwhALEr1jdhuxKqoTno8cyGXw9yLSsqC6A", nftokenIDData.Issuer);
            Assert.AreEqual(nftokenId, nftokenIDData.NFTokenID);
            Assert.AreEqual((uint)3, nftokenIDData.Taxon);
            Assert.AreEqual((uint)5000, nftokenIDData.TransferFee);
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