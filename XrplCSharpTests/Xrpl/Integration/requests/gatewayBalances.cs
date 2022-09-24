

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/gatewayBalances.ts

using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using XrplTests.Xrpl.ClientLib;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIGatewayBalances
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
            GatewayBalancesRequest request = new GatewayBalancesRequest(runner.wallet.ClassicAddress)
            {
                LedgerIndex = index,
                Strict = true,
            };
            GatewayBalances response = await runner.client.GatewayBalances(request);
            Assert.IsNotNull(response);
        }
    }
}