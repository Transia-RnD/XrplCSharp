using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/checkCash.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestICheckCash
    {
        // private static int Timeout = 20;
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
            XrplWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            // WAITING ON BINARY REFACTOR
            //Currency sendMax = new Currency {
            //    CurrencyCode = "XRP",
            //    Value = "50"
            //};
            CheckCreate setupTx = new CheckCreate
            {
                Account = runner.wallet.ClassicAddress,
                Destination = wallet2.ClassicAddress,
                SendMax = new Currency { ValueAsXrp = 50 }
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);

            // get check ID
            AccountObjectsRequest request1 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "check" };
            AccountObjects response1 = await runner.client.AccountObjects(request1);
            string checkId = response1.AccountObjectList[0].Index;
            
            // actual test - cancel the check
            CheckCash tx = new CheckCash
            {
               Account = wallet2.ClassicAddress,
               CheckID = checkId,
               Amount = new Currency { ValueAsXrp = 50 }
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, wallet2);

            // get check ID
            AccountObjectsRequest request2 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "check" };
            AccountObjects response2 = await runner.client.AccountObjects(request1);
            Assert.AreEqual(0, response2.AccountObjectList.Count);
        }
    }
}