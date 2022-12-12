

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/NFTokenBurn.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    /// <summary>
    /// NFTokenBurn Transaction Verification Testing.<br/>
    /// Providing runtime verification testing for each specific transaction type.
    /// </summary>
    [TestClass]
    public class TestUNFTokenBurn
    {
        private static string TOKEN_ID =
            "00090032B5F762798A53D543A014CAF8B297CFF8F2F937E844B17C9E00000003";

        [TestMethod]
        public async Task TestVerify_Valid_NFTokenBurn()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenBurn" },
                { "NFTokenID", TOKEN_ID },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Invalid_missing_NFTokenID()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenBurn" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenBurn: missing field NFTokenID - no ERROR");
        }
    }
}

