using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Tests;
using Xrpl.XrplWallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/checkCancel.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestICheckCancel
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
            Currency sendMax = new Currency {
                CurrencyCode = "XRP",
                Value = "50"
            };
            CheckCreate tx = new CheckCreate
            {
                Account = runner.wallet.ClassicAddress,
                Destination = wallet2.ClassicAddress,
                SendMax = sendMax
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);

            // get check ID
            AccountObjectsRequest request1 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "check" };
            AccountObjects response1 = await runner.client.AccountObjects(request1);
            string checkId = response1.AccountObjectList[0].Index;
            
            // actual test - cancel the check
            //CheckCancelTransaction tx = new CheckCancelTransaction
            //{
            //    Account = runner.wallet.ClassicAddress,
            //    Destination = wallet2.ClassicAddress,
            //};
            //Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            //await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        }
    }
}