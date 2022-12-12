

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/NFTokenCreateOffer.ts

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;

namespace XrplTests.Xrpl.Models
{
    /// <summary>
    /// NFTokenCreateOffer Transaction Verification Testing.<br/>
    /// Providing runtime verification testing for each specific transaction type.
    /// </summary>
    [TestClass]
    public class TestUNFTokenCreateOffer
    {
        private static string NFTOKEN_ID =
            "00090032B5F762798A53D543A014CAF8B297CFF8F2F937E844B17C9E00000003";


        [TestMethod]
        public async Task TestVerify_Valid_NFTokenCreateOffer_buyside()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Valid_NFTokenCreateOffer_sellside()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Flags", NFTokenCreateOfferFlags.tfSellNFToken},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Valid_0_Amount_NFTokenCreateOffer_sellside()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "0"},
                {"Flags", NFTokenCreateOfferFlags.tfSellNFToken},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Account_is_Owner()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Expiration", "1000"},
                {"Owner", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: Owner and Account must not be equal - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Account_is_Destination()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Flags", NFTokenCreateOfferFlags.tfSellNFToken},
                {"Expiration", "1000"},
                {"Destination", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: Destination and Account must not be equal - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Invalid_out_NFTokenID()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                {"Amount", "1"},
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer:  missing field NFTokenID - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Amount()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", 1},
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: invalid Amount - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Invalid_Missing_Amount()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Destination", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: invalid Amount - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Owner_for_sell_offer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Flags", NFTokenCreateOfferFlags.tfSellNFToken},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: Owner must not be present for sell offers - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Invalid_out_Owner_for_buy_offer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "1"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: Owner must be present for buy offers - no ERROR");
        }

        [TestMethod]
        public async Task TestVerify_Invalid_0_Amount_for_buy_offer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenCreateOffer" },
                { "NFTokenID", NFTOKEN_ID },
                {"Amount", "0"},
                {"Owner", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"Expiration", "1000"},
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenCreateOffer: Amount must be greater than 0 for buy offers - no ERROR");
        }


    }

}

