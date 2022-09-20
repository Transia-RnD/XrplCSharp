using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Tests;
using Xrpl.XrplWallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/depositPreauth.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestIDepositPreauth
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
            DepositPreauth setupTx = new DepositPreauth
            {
                Account = runner.wallet.ClassicAddress,
                Authorize = wallet2.ClassicAddress,
            };
            Debug.WriteLine(setupTx.ToJson());
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }
    }
}