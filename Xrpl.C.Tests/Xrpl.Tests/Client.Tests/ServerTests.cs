using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class ServerTests
    {
        private static IRippleClient client;

        private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
        //private static string serverUrl = "wss://s2.ripple.com:443";

        public TestContext TestContext { get; set; }


        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();
        }

        //[TestMethod]
        //public async Task CanGetServerState()
        //{
        //    ServerInfo serverInfo = await client.ServerInfo();
        //    Assert.IsNotNull(serverInfo);
        //}

        //[TestMethod]
        //public async Task CanGetFees()
        //{
        //    Fee fee = await client.Fees();
        //    Assert.IsNotNull(fee);
        //}
    }
}
