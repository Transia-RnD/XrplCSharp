using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Enums;
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
            Assert.AreEqual(responseCommon.date.ToString(), "706918490");
            Assert.AreEqual(responseCommon.inLedger.ToString(), "2465631");
            Assert.AreEqual(responseCommon.ledger_index.ToString(), "2465631");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[1].ModifiedNode.LedgerEntryType.ToString(), "NFTokenPage");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[1].ModifiedNode.LedgerIndex.ToString(), "751F85F13C661A20C610A2C32EBB48CE5819784EFFFFFFFFFFFFFFFFFFFFFFFF");
            Assert.AreEqual(responseCommon.Meta.AffectedNodes[1].ModifiedNode.FinalFields.NFTokens[0].NFToken.NFTokenID, "00090000751F85F13C661A20C610A2C32EBB48CE5819784E0000099B00000000");
            
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
