using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1.Ocsp;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Tests;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/subscribe.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestISubscribe
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
        public async Task TestSubscribe()
        {
            SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Streams = new List<string> { "ledger" },
                Accounts = new List<string> { runner.wallet.ClassicAddress },
            };
            Debug.WriteLine(subscribeRequest);
            await runner.client.Subscribe(subscribeRequest).ContinueWith(t =>
            {
                Debug.WriteLine(t.Result);
            });
        }
    }
}