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
using Timer = System.Timers.Timer;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/subscribe.ts

namespace Xrpl.Tests.ClientLib
{
    [TestClass]
    public class TestSWebSocketClient
    {

        [TestMethod]
        public void TestSome()
        {
            var message = "{\"fee_base\":10,\"fee_ref\":10,\"ledger_hash\":\"4118BC9FD82A6245BDD32092CE93A86676978D3EBD6F9A47C3ABCEFE80E2B3F5\",\"ledger_index\":75551992,\"ledger_time\":720984480,\"reserve_base\":10000000,\"reserve_inc\":2000000,\"txn_count\":54,\"type\":\"ledgerClosed\",\"validated_ledgers\":\"32570-75551992\"}";
            var response = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message);
            var message1 = JsonConvert.SerializeObject(response);
            var response1 = JsonConvert.DeserializeObject<LedgerStream>(message1);
        }

        [TestMethod]
        public async Task TestSubscribeWebSocket()
        {

            bool isTested = false;
            bool isFinished = false;

            var server = "wss://xrplcluster.com/";

            var client = new XrplClient(server);

            client.connection.OnConnected += async () =>
            {
                Console.WriteLine("CONNECTED");
                var subscribe = await client.Subscribe(
                new SubscribeRequest()
                {
                    Streams = new List<string>(new[]
                    {
                        "ledger",
                    })
                });
            };

            client.connection.OnError += (error, errorMessage, message, data) =>
            {
                Console.WriteLine($"CONN ERROR: {error}");
                Console.WriteLine($"CONN ERROR MESSAGE: {errorMessage}");
                Console.WriteLine($"CONN MESSAGE: {message}");
                Console.WriteLine($"CONN ERROR DATA: {data}");
                isFinished = true;
                return Task.CompletedTask;
            };

            client.connection.OnDisconnect += (code) =>
            {
                Console.WriteLine($"DISCONNECTED: {code}");
                isFinished = true;
                return Task.CompletedTask;
            };

            client.connection.OnLedgerClosed += (message) =>
            {
                Console.WriteLine($"LEDGER CLOSED: {message}");
                isFinished = true;
                isTested = true;
                return Task.CompletedTask;
            };

            Timer timer = new Timer(5000);
            timer.Elapsed += (sender, e) =>
            {
                Debug.WriteLine("TIMEOUT!!");
                client.Dispose();
                isFinished = true;
            };
            timer.Start();

            await client.Connect();

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
    }
}

