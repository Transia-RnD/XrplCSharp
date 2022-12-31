using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;

using Xrpl.Models.Transaction;
using Xrpl.Models.Transactions;
using Xrpl.Utils;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/models/NFTokenMint.ts

namespace XrplTests.Xrpl.Models
{

    /// <summary>
    /// NFTokenMint Transaction Verification Testing.<br/>
    /// Providing runtime verification testing for each specific transaction type.
    /// </summary>
    [TestClass]
    public class TestUNFTokenMint
    {
        [TestMethod]
        public async Task TestVerify_Valid_NFTokenMint()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", NFTokenMintFlags.tfTransferable },
                {"NFTokenTaxon", 0},
                {"Issuer", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"TransferFee", 1},
                {"URI", "http://xrpl.org".ConvertStringToHex()},
            };
            await Validation.Validate(offer);
        }
        [TestMethod]
        public async Task TestVerify_InValid_missing_NFTokenTaxon()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", NFTokenMintFlags.tfTransferable },
                {"Issuer", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"TransferFee", 1},
                {"URI", "http://xrpl.org".ConvertStringToHex()},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenMint: missing field NFTokenTaxon - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_Account_is_Issuer()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", NFTokenMintFlags.tfTransferable },
                {"NFTokenTaxon", 0},
                {"Issuer", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"TransferFee", 1},
                {"URI", "http://xrpl.org".ConvertStringToHex()},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenMint: Issuer must not be equal to Account - no ERROR");
        }
        [TestMethod]
        public async Task TestVerify_Invalid_URI_not_in_hex_format()
        {
            var offer = new Dictionary<string, dynamic>
            {
                { "TransactionType", "NFTokenMint" },
                {"Account", "rWYkbWkCeg8dP6rXALnjgZSjjLyih5NXm"},
                {"Fee", "5000000"},
                {"Sequence", 2470665u},
                { "Flags", NFTokenMintFlags.tfTransferable },
                {"NFTokenTaxon", 0},
                {"Issuer", "r9LqNeG6qHxjeUocjvVki2XR35weJ9mZgQ"},
                {"TransferFee", 1},
                {"URI", "http://xrpl.org"},
            };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => Validation.Validate(offer), "NFTokenMint:  URI must be in hex format - no ERROR");
        }
    }

}
