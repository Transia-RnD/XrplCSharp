using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.WalletLib;
using XrplTests.Xrpl.ClientLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/checkCreate.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestICheckCreate
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
            Wallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
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
            Debug.WriteLine(setupTx.ToJson());
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);

            // get check ID
            AccountObjectsRequest request1 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "check" };
            AccountObjects response1 = await runner.client.AccountObjects(request1);
            Assert.AreEqual(1, response1.AccountObjectList.Count);
        }
    }
}