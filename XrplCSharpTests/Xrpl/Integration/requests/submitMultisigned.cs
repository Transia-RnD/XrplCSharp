using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl;
using Xrpl.Models.Methods;
using XrplTests.Xrpl.ClientLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/submitMultisigned.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestISubmitMultisigned
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
            //SubmitMultisigned accountTx = await runner.client.SubmitMultisigned(runner.wallet.ClassicAddress);
            //Assert.IsNotNull(accountTx);
        }
    }
}