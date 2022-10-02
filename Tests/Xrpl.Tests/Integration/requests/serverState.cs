

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/serverState.ts

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIServerState
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
            //ServerState accountTx = await runner.client.ServerState(runner.wallet.ClassicAddress);
            //Assert.IsNotNull(accountTx);
        }
    }
}