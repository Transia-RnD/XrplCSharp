using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
//using Ripple.Binary.Codec.Types;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Xrpl.Client.Models.Methods;
using System.Globalization;
using System.Threading;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class EmptyTests
    {
        private static string account;
        private static IRippleClient client;

        private static string serverUrl = "wss://s.altnet.rippletest.net:51233";

        [ClassInitialize]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();
            account = "rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p";
        }

        [TestMethod]
        public async Task RandomTest()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-Ru");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-Ru");

            var info = await client.AccountInfo(new AccountInfoRequest("rsKbfunjbcP6u3BgFy6Nd3BFHSuND2hZLa"));
            Console.WriteLine(info.AccountData.Balance);
            var request = new SubscribeRequest()
            {
                Streams = new List<string>(new[] { "ledger" }),
                Accounts = new List<string>(new[] { "rsKbfunjbcP6u3BgFy6Nd3BFHSuND2hZLa" })
            };
            await client.Subscribe(request).ContinueWith(Task =>
            {
                Console.WriteLine(Task.Result);
            });

            Console.ReadLine();
            client.Disconnect();
        }
    }
}
