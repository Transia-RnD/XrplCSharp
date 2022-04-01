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
            Debug.WriteLine(responseCommon.Meta.TransactionResult.ToString());
            Assert.IsNotNull(responseCommon.Meta);
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
