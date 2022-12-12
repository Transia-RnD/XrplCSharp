using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/setupClient.ts

namespace XrplTests.Xrpl
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
            client = new XrplClient($"ws://127.0.0.1:{port}");
            client.OnConnected += () =>
            {
                //Console.WriteLine("SETUP CLIENT: CONECTED");
                return Task.CompletedTask;
            };
            client.OnDisconnect += (code) =>
            {
                //Console.WriteLine("SETUP CLIENT: DISCONECTED");
                return Task.CompletedTask;
            };
            client.OnError += (e, em, m, d) =>
            {
                //Console.WriteLine($"SETUP CLIENT: ERROR: {e}");
                return Task.CompletedTask;
            };
            await client.Connect();
            return this;
        }
    }
}
