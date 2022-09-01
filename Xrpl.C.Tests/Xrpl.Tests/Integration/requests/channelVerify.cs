

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/channelVerify.ts

using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Tests;

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestChannelVerify
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
            ChannelVerifyRequest request = new ChannelVerifyRequest
            {
                ChannelId = "5DB01B7FFED6B67E6B0414DED11E051D2EE2B7619CE0EAA6286D67A3A4D5BDB3",
                Signature = "304402204EF0AFB78AC23ED1C472E74F4299C0C21F1B21D07EFC0A3838A420F76D783A400220154FB11B6F54320666E4C36CA7F686C16A3A0456800BBC43746F34AF50290064",
                PublicKey = "aB44YfzW24VDEJQ2UuLPV2PvqcPCSoLnL7y5M1EzhdW4LnK5xMS3",
                Amount = 1000000,

            };
            ChannelVerify response = await runner.client.ChannelVerify(request);
            Assert.IsNotNull(response);
        }
    }
}