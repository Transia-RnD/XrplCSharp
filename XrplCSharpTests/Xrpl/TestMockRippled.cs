using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.ClientLib;
using Xrpl.Models.Methods;
using Xrpl.WalletLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/mockRippledTest.ts

namespace XrplTests.Xrpl
{
    [TestClass]
    public class TestMockRippled
    {

        public static SetupUnit runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupUnit().SetupClient();
        }

        [TestMethod]
        public async Task TestErrorMockNotProvided()
        {
            ServerInfoRequest request = new ServerInfoRequest();
            //await runner.client.ServerInfo(request);
        }
    }
}

