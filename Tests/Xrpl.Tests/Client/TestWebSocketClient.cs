using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Methods;
using Xrpl.Models.Subscriptions;
using Xrpl.Sugar;
using XrplTests.Xrpl.MockRippled;
using Timer = System.Timers.Timer;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/subscribe.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUWebSocketClient
    {

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

            bool isTested = false;
            bool isFinished = false;

            var server = "wss://xrplcluster.com/";

            var client = new WebSocketClient(server);

            client.OnConnected += (t) =>
            {
                Console.WriteLine($"CONNECTED");
            };

            client.OnConnectionException += (ex) =>
            {
                Console.WriteLine($"CONNECTION EXCEPTION: {ex.Message}");
                isFinished = true;
            };

            client.OnError += (ex) =>
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                isFinished = true;
            };

            client.OnDisconnect += (code) =>
            {
                Console.WriteLine($"DISCONNECTED: {code}");
                isFinished = true;
            };

            client.OnMessageReceived += (message) =>
            {
                Console.WriteLine($"MESSAGE RECEIVED: {message}");
                Dictionary<string, dynamic> json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message);
                if (json["type"] == "ledgerClosed")
                {
                    isTested = true;
                    isFinished = true;
                }
            };

            Timer timer = new Timer(5000);
            timer.Elapsed += (sender, e) =>
            {
                Debug.WriteLine("TIMEOUT!!");
                client.Close();
                isFinished = true;
            };
            timer.Start();

            _ = client.ConnectAsync();

            while (!client._isConnected)
            {
                Debug.WriteLine($"CONNECTING... {DateTime.Now}");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }

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
            await client.Send(jsonString);

            Debug.WriteLine($"BEFORE: {DateTime.Now}");

            while (!isFinished)
            {
                Debug.WriteLine($"WAITING: {DateTime.Now}");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Debug.WriteLine($"AFTER: {DateTime.Now}");
            Debug.WriteLine($"IS FINISHED: {isFinished}");
            Debug.WriteLine($"IS TESTER: {isTested}");
            Assert.IsTrue(isTested);
        }


        [TestMethod]
        public async Task TestWebSocketClientTimeout()
        {
            bool isFinished = false;

            var server = "wss://xrplcluster.com/";

            var client = new WebSocketClient(server);

            client.OnConnected += (t) =>
            {
                Debug.WriteLine($"CONNECTED");
                Assert.IsNotNull(t);
                Assert.IsFalse(t.IsCancellationRequested);
                isFinished = true;
            };

            client.OnConnectionException += (ex) =>
            {
                Debug.WriteLine($"CONNECTION EXCEPTION: {ex.Message}");
                isFinished = true;
            };

            client.OnError += (ex) =>
            {
                Debug.WriteLine($"EXCEPTION: {ex.Message}");
                //Debug.WriteLine(message);
                isFinished = true;
            };

            client.OnDisconnect += (code) =>
            {
                Debug.WriteLine($"DISCONNECTED: {code}");
                isFinished = true;
            };

            Timer timer = new Timer(2000);
            timer.Elapsed += (sender, e) =>
            {
                client.Close();
                isFinished = true;
            };
            timer.Start();

            await client.ConnectAsync();

            Debug.WriteLine($"BEFORE: {DateTime.Now}");

            while (!isFinished)
            {
                Debug.WriteLine($"WAITING: {DateTime.Now}");
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Debug.WriteLine($"AFTER: {DateTime.Now}");
        }
    }
}

