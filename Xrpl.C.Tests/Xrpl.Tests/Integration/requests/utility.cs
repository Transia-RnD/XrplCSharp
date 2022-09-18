using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Tests;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/utility.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestIUtility
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
        public async Task TestPingRequest()
        {
            PingRequest request = new PingRequest();
            object response = await runner.client.Ping(request);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task TestRandomRequest()
        {
            RandomRequest request = new RandomRequest();
            object response = await runner.client.Random(request);
            Assert.IsNotNull(response);
        }
    }
}