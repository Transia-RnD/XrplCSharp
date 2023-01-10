

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/utils/parseNFTokenID.ts

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

            NFTokenIDData nftokenIDData = ParseNFTID.GetNFTokenIDData(nftokenId);

            Assert.AreEqual((UInt32)8, nftokenIDData.Flags);
            Assert.AreEqual("rwhALEr1jdhuxKqoTno8cyGXw9yLSsqC6A", nftokenIDData.Issuer);
            Assert.AreEqual(nftokenId, nftokenIDData.NFTokenID);
            Assert.AreEqual((UInt32)3, nftokenIDData.Taxon);
            Assert.AreEqual((UInt32)5000, nftokenIDData.TransferFee);
        }
    }
}

