using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Extensions;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Utils.Hashes;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/tx.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestITx
    {
        //// private static int Timeout = 20;
        public TestContext TestContext { get; set; }
        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        [TestMethod]
        public async Task TestRequestMethod()
        {

            string account = runner.wallet.ClassicAddress;
            AccountSet request = new AccountSet
            {
                Account = account,
                Domain = "example.com".ConvertStringToHex()
            };

            Dictionary<string, dynamic> txRequest = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(request.ToJson());
            Submit response = await runner.client.Submit(txRequest, runner.wallet);
            string hash = HashLedger.HashSignedTx(response.TxBlob);
            TxRequest request1 = new TxRequest(hash);
            TransactionResponseCommon accountTx = await runner.client.Tx(request1);
            Assert.IsNotNull(accountTx);
        }
    }
}