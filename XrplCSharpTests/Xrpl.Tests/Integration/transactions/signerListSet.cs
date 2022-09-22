using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Tests.Client.Tests;
using Xrpl.XrplWallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/signerListSet.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestISignerListSet
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
            SignerEntry signer1 = new SignerEntry { Account = "r5nx8ZkwEbFztnc8Qyi22DE9JYjRzNmvs", SignerWeight = 1 };
            SignerEntry signer2 = new SignerEntry { Account = "r3RtUvGw9nMoJ5FuHxuoVJvcENhKtuF9ud", SignerWeight = 1 };
            SignerListSet setupTx = new SignerListSet
            {
                Account = runner.wallet.ClassicAddress,
                SignerQuorum = 2,
                SignerEntries = { signer1, signer2 }
            };
            Debug.WriteLine(setupTx.ToJson());
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }
    }
}