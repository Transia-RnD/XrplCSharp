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
            client.OnDisconnect += (code) =>
            {
                Console.WriteLine($"DISCONECTED CODE: {code}");
                Console.WriteLine("DISCONECTED");
                return null;
            };
            await client.Connect();
            return this;
        }
    }
}
