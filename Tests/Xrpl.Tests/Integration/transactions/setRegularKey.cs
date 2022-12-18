using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Ledger;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/setRegularKey.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestISetRegularKey
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
            SetRegularKey setupTx = new SetRegularKey
            {
                Account = runner.wallet.ClassicAddress,
                RegularKey = wallet2.ClassicAddress,
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }
    }
}