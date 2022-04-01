using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Responses.Transaction;
using Xrpl.Client.Responses.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.TransactionTypes;
using Xrpl.Tests.Client.Tests.Properties;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class DeserializationTests
    {

        [TestMethod]
        public void CanDeserializeTransaction()
        {
            var transaction = Encoding.ASCII.GetString(Resources.Transaction);
            ITransactionResponseCommon responseCommon = JsonConvert.DeserializeObject<TransactionResponseCommon>(transaction);
            Assert.AreEqual(responseCommon.date.ToString(), "701933800");
            Assert.AreEqual(responseCommon.inLedger.ToString(), "810019");
            Assert.AreEqual(responseCommon.ledger_index.ToString(), "810019");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[0].CreatedNode.LedgerEntryType.ToString(), "NFTokenPage");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[0].CreatedNode.LedgerIndex.ToString(), "5C5024773DF0C8D708C71D8218F7C59588EB2B1EFFFFFFFFFFFFFFFFFFFFFFFF");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[0].CreatedNode.NewFields.NonFungibleTokens[0].NonFungibleToken.TokenID, "000900005C5024773DF0C8D708C71D8218F7C59588EB2B1E0000099B00000000");
            
            Assert.IsNotNull(responseCommon.Meta);
            Assert.AreEqual(responseCommon.Validated, true);
            Assert.AreEqual(responseCommon.Meta.TransactionIndex.ToString(), "0");
            Assert.AreEqual(responseCommon.Meta.TransactionResult.ToString(), "tesSUCCESS");
            Assert.IsNotNull(responseCommon);
        }

        [TestMethod]
        public void CanDeserializeBinaryTransaction()
        {
            var transaction = Encoding.ASCII.GetString(Resources.TransactionBinary);
            BinaryTransactionResponse binaryTransaction =
                JsonConvert.DeserializeObject<BinaryTransactionResponse>(transaction);
            Assert.IsNotNull(binaryTransaction);
            Assert.IsNotNull(binaryTransaction.Meta);
            Assert.IsNotNull(binaryTransaction.Transaction);
        }
    }
}
