

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/ripplePathFind.ts

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIRipplePathFind
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
            
            //RipplePathFind accountTx = await runner.client.RipplePathFind(runner.wallet.ClassicAddress);
            //Assert.IsNotNull(accountTx);
        }
    }
}