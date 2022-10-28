using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Models.Transaction;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/accountDelete.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIAccountDelete
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
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            AccountChannelsRequest request = new AccountChannelsRequest(runner.wallet.ClassicAddress) { LedgerIndex = index };
            AccountChannels response = await runner.client.AccountChannels(request);
            Assert.IsNotNull(response);
            AccountDelete tx = new AccountDelete
            {
                Account = runner.wallet.ClassicAddress,
                Destination = wallet2.ClassicAddress,
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        }
    }
}