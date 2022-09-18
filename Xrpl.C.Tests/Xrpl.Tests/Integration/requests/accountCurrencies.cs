using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Tests;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/accountCurrencies.ts


namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class AccountICurrenciesRequests
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
            AccountCurrenciesRequest request = new AccountCurrenciesRequest(runner.wallet.ClassicAddress) { LedgerIndex = index, Strict = true };
            AccountCurrencies response = await runner.client.AccountCurrencies(request);
            Assert.IsNotNull(response);
        }
    }
}