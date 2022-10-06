using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.AddressCodec;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/mockRippledTest.ts

namespace XrplTests.Xrpl
{
    [TestClass]
    public class TestMockRippled
    {

        public static SetupUnit runner;

        //[ClassInitialize]
        //public static async Task MyClassInitializeAsync(TestContext testContext)
        //{
        //    runner = await new SetupUnit().SetupClient();
        //}

        //[ClassCleanup]
        //public static async Task MyClassCleanupAsync(TestContext testContext)
        //{
        //    await runner.client.Disconnect();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(XrplError), "")]
        //public async Task TestErrorMockNotProvided()
        //{
        //    ServerInfoRequest request = new ServerInfoRequest();
        //    await runner.client.ServerInfo(request);
        //}
    }
}

