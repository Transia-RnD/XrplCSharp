

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/ledgerCurrent.ts

using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Tests;

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestILedgerCurrentRequests
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
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            LedgerCurrentRequest request = new LedgerCurrentRequest();
            LOLedgerCurrentIndex response = await runner.client.LedgerCurrent(request);
            Assert.IsNotNull(response);
        }
    }
}