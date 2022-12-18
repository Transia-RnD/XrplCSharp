using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/ticketCreate.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestITicketCreate

    {
        // private static int Timeout = 20;
        public TestContext TestContext;

        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        //[ClassCleanup]
        //public static async Task MyClassCleanupAsync()
        //{
        //    await runner.client.Disconnect();
        //}

        [TestMethod]
        public async Task TestRequestMethod()
        {
            TicketCreate tx = new TicketCreate
            {
                Account = runner.wallet.ClassicAddress,
                TicketCount = 2
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        }
    }
}
