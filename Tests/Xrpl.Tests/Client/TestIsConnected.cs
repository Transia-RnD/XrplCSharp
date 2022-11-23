using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/isConnected.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUIsConnected
    {

        public static SetupUnitClient runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupUnitClient().SetupClient();
        }

        [ClassCleanup]
        public static void MyClassCleanupAsync()
        {
            runner.client.Disconnect().Wait();
        }

        [TestMethod]
        public async Task TestConnectedDisconnect()
        {
            Assert.AreEqual(true, runner.client.IsConnected());
            await runner.client.Disconnect();
            Assert.AreEqual(false, runner.client.IsConnected());
        }
    }
}

