

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/NFTokenAcceptOffer.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

namespace XrplTests.Xrpl.Models
{
    /// <summary>
    /// NFTokenAcceptOffer Transaction Verification Testing.<br/>
    /// Providing runtime verification testing for each specific transaction type.
    /// </summary>
    [TestClass]
    public class TestUNFTokenAcceptOffer
    {
        private static string NFTOKEN_BUY_OFFER =
            "AED08CC1F50DD5F23A1948AF86153A3F3B7593E5EC77D65A02BB1B29E05AB6AF";

        private static string NFTOKEN_SELL_OFFER =
            "AED08CC1F50DD5F23A1948AF86153A3F3B7593E5EC77D65A02BB1B29E05AB6AE";

        [TestMethod]
        public async Task TestVerify_Valid_NFTokenAcceptOffer_With_NFTokenBuyOffer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Valid_NFTokenAcceptOffer_With_NFTokenSellOffer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                { "NFTokenBuyOffer", NFTOKEN_SELL_OFFER },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Invalid_missing_NFTokenSellOffer_and_NFTokenBuyOffer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: must set either NFTokenSellOffer or NFTokenBuyOffer - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_missing_NFTokenSellOffer_and_present_NFTokenBrokerFee()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"NFTokenBrokerFee", "1"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: both NFTokenSellOffer and NFTokenBuyOffer must be set if using brokered mode - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_missing_NFTokenBuyOffer_and_present_NFTokenBrokerFee()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                {"NFTokenBrokerFee", "1"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: both NFTokenSellOffer and NFTokenBuyOffer must be set if using brokered mode - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Valid_NFTokenAcceptOffer_with_both_offers_and_no_NFTokenBrokerFee()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Valid_NFTokenAcceptOffer_with_NFTokenBrokerFee()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"NFTokenBrokerFee", "1"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Validation.Validate(offer);
        }

        [TestMethod]
        public async Task TestVerify_Invalid_NFTokenBrokerFee_Is_0()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"NFTokenBrokerFee", "0"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: NFTokenBrokerFee must be greater than 0; omit if there is no fee - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_NFTokenBrokerFee_less_0()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"NFTokenBrokerFee", "-1"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: NFTokenBrokerFee must be greater than 0; omit if there is no fee - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Invalid_NFTokenBrokerFee()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenAcceptOffer" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                { "NFTokenSellOffer", NFTOKEN_SELL_OFFER },
                { "NFTokenBuyOffer", NFTOKEN_BUY_OFFER },
                {"NFTokenBrokerFee", 1},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                {"Flags", 2147483648u},
            };
            await Assert.ThrowsExceptionAsync<ValidationError>(() => Validation.Validate(offer), "NFTokenAcceptOffer: invalid NFTokenBrokerFee - no ERROR");
        }


    }
}

