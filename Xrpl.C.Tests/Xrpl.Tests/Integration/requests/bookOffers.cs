

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/bookOffers.ts

using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Tests;

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestIBookOffersRequests
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
            TakerAmount takerGets = new TakerAmount { Currency = "XRP" };
            TakerAmount takerPays = new TakerAmount { Currency = "USD", Issuer = runner.wallet.ClassicAddress };
            BookOffersRequest request = new BookOffersRequest() { TakerGets = takerGets, TakerPays = takerPays };
            BookOffers bookOffers = await runner.client.BookOffers(request);
            Assert.IsNotNull(bookOffers);
        }
    }
}