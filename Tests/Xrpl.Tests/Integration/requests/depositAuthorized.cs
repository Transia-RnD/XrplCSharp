

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/depositAuthorized.ts

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIDepositAuthorized
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
            //DepositAuthorizedRequest request = new DepositAuthorizedRequest {  };
            //DepositAuthorized response = await runner.client.DepositAuthorized(request);
            //Assert.IsNotNull(response);
        }
    }
}