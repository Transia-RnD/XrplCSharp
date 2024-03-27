using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Timer = System.Timers.Timer;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/setupClient.ts

namespace Xrpl.Tests
{
    public class SetupUnitClient
    {
        public XrplClient client;
        public CreateMockRippled mockedRippled;
        public int _mockedServerPort;

        public async Task<SetupUnitClient> SetupClient()
        {
            int port = TestUtils.GetFreePort();
            var tcpListenerThread = new Thread(() =>
            {
                mockedRippled = new CreateMockRippled(port);
                mockedRippled.Start();
                _mockedServerPort = port;
            });
            tcpListenerThread.Start();
            Timer timer = new Timer(25000);
            timer.Elapsed += (sender, e) => tcpListenerThread.Abort();
            client = new XrplClient($"ws://127.0.0.1:{port}");
            client.connection.OnConnected += () =>
            {
                Debug.WriteLine("SETUP CLIENT: CONECTED");
                return Task.CompletedTask;
            };
            client.connection.OnDisconnect += (code) =>
            {
                Debug.WriteLine("SETUP CLIENT: DISCONECTED");
                return Task.CompletedTask;
            };
            client.connection.OnError += (e, em, m, d) =>
            {
                Debug.WriteLine($"SETUP CLIENT: ERROR: {e}");
                return Task.CompletedTask;
            };
            await client.Connect();
            return this;
        }
    }
}
