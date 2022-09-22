

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/fee.ts

using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Tests.Client.Tests;

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestIFee
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
            FeeRequest request = new FeeRequest { };
            Fee response = await runner.client.Fee(request);
            Assert.IsNotNull(response);
        }
    }
}