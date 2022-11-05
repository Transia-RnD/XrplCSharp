using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;
using Xrpl.Models.Subscriptions;
using Xrpl.Sugar;
using XrplTests.Xrpl.MockRippled;
using Timer = System.Timers.Timer;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/subscribe.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUEmpty
    {


        [TestMethod]
        public async Task TestSubscribe1()
        {

            bool isFinished = false;
            var server = "wss://xrplcluster.com/";

            var client = new XrplClient(server);

            await client.Connect();

            client.OnLedgerClosed += (message) =>
            {
                Console.WriteLine($"LEDGER CALLBACK: {message}");
                isFinished = true;
            };

            client.OnDisconnect += (c) =>
            {
                Console.WriteLine($"DISCONNECT CALLBACK: {c}");
                //isFinished = true;
            };

            var subscribe = await client.Subscribe(
            new SubscribeRequest()
            {
                Streams = new List<string>(new[]
                {
                    "ledger",
                })
            });

            Debug.WriteLine($"BEFORE: {DateTime.Now}");
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(8));
            //Debug.WriteLine($"AFTER: {DateTime.Now}");

            Timer timer = new Timer(8000);
            timer.Elapsed += (sender, e) =>
            {
                isFinished = true;
            };
            timer.Start();

            while (!isFinished)
            {
                Debug.WriteLine($"WAITING: {DateTime.Now}");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Debug.WriteLine($"AFTER: {DateTime.Now}");
            //await client.Disconnect();
            //Assert.Fail();
        }

        [TestMethod]
        public void TestSome()
        {
            var message = "{\"fee_base\":10,\"fee_ref\":10,\"ledger_hash\":\"4118BC9FD82A6245BDD32092CE93A86676978D3EBD6F9A47C3ABCEFE80E2B3F5\",\"ledger_index\":75551992,\"ledger_time\":720984480,\"reserve_base\":10000000,\"reserve_inc\":2000000,\"txn_count\":54,\"type\":\"ledgerClosed\",\"validated_ledgers\":\"32570-75551992\"}";
            var response = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message);
            var message1 = JsonConvert.SerializeObject(response);
            var response1 = JsonConvert.DeserializeObject<LedgerStream>(message1);
            Debug.WriteLine(response1);
        }


        [TestMethod]
        public async Task TestSubscribeWebSocket()
        {
            var server = "wss://xrplcluster.com/";

            var client = new WebSocketClient(server);

            await client.ConnectAsync();

            bool isFinished = false;
            client.OnMessageReceived += (ws, message) =>
            {
                Console.WriteLine(message);
                isFinished = true;
            };

            var request = new SubscribeRequest()
            {
                Streams = new List<string>(new[]
                    {
                        "ledger",
                    })
            };
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.FloatParseHandling = FloatParseHandling.Double;
            serializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
            string jsonString = JsonConvert.SerializeObject(request, serializerSettings);
            await client.SendMessageAsync(jsonString);

            while (!isFinished)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}

