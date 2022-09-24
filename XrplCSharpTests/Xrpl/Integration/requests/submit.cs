

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/submit.ts

using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using XrplTests.Xrpl.ClientLib;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestISubmit
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
            //Submit accountTx = await runner.client.SubmitRequest(runner.wallet.ClassicAddress);
            //Assert.IsNotNull(accountTx);
        }
    }
}