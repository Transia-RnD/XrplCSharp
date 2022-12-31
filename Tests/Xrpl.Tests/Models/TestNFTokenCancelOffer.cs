

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/NFTokenCancelOffer.ts

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    /// <summary>
    /// NFTokenCancelOffer Transaction Verification Testing.<br/>
    /// Providing runtime verification testing for each specific transaction type.
    /// </summary>
    [TestClass]
    public class TestUNFTokenCancelOffer
    {
        private static string BUY_OFFER =
            "AED08CC1F50DD5F23A1948AF86153A3F3B7593E5EC77D65A02BB1B29E05AB6AF";

        [TestMethod]
        public async Task TestVerify_Valid_NFTokenCancelOffer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCancelOffer" },
                { "NFTokenOffers", new List<dynamic>(){ BUY_OFFER } },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Invalid_missing_NFTokenOffers()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCancelOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCancelOffer: missing field NFTokenOffers - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_empty_NFTokenOffers()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCancelOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenOffers", new List<dynamic>()},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCancelOffer: empty field NFTokenOffers - no ERROR");
        }
    }

}

