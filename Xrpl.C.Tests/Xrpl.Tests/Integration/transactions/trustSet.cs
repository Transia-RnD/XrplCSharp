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
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/trustSet.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestTrustSet

    {
        // private static int Timeout = 20;
        public TestContext TestContext;

        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        [TestMethod]
        public async Task TestRequestMethod()
        {
            rWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            Currency limitAmount = new Currency {
                CurrencyCode = "USD",
                Issuer = wallet2.ClassicAddress,
                Value = "100.10"
            };
            TrustSetTransaction tx = new TrustSetTransaction
            {
                Account = runner.wallet.ClassicAddress,
                LimitAmount = limitAmount
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            Debug.WriteLine(txJson);
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        }
    }
}
