using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Ledger;
using Xrpl.Models.Transactions;
using Xrpl.WalletLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/signerListSet.ts

namespace XrplTests.Xrpl.ClientLib.Integration
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
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }
    }
}