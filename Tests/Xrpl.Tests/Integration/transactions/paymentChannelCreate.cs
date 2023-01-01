using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using Xrpl.Models.Common;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;
using static XrplTests.Xrpl.ClientLib.Integration.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/paymentChannelCreate.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIPaymentChannelCreate
    {
        // private static int Timeout = 20;
        public TestContext TestContext { get; set; }
        public static SetupIntegration runner;
        public static IntegrationIC ic;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
            ic = await Utils.IssueIC(runner.client);
            await Utils.FundIC(runner.client, runner.wallet, ic);
        }

        [TestMethod]
        public async Task TestPaychanNC()
        {

            XrplWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            PaymentChannelCreate setupTx = new PaymentChannelCreate
            {
                Account = runner.wallet.ClassicAddress,
                Amount = new Currency() { ValueAsXrp = 13100000 },
                Destination = wallet2.ClassicAddress,
                SettleDelay = 86400,
                PublicKey = runner.wallet.PublicKey
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }

        [TestMethod]
        public async Task TestPayChanIC()
        {

            XrplWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            PaymentChannelCreate setupTx = new PaymentChannelCreate
            {
                Account = runner.wallet.ClassicAddress,
                Amount = new Currency() {
                    CurrencyCode = "USD",
                    Issuer = ic.Issuer,
                    Value = "100",
                },
                Destination = wallet2.ClassicAddress,
                SettleDelay = 86400,
                PublicKey = runner.wallet.PublicKey
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);
        }
    }
}