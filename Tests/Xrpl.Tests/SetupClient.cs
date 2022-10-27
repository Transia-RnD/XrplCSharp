using System;
using System.Diagnostics;
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
            var promise = new TaskCompletionSource();
            mockedRippled = new CreateMockRippled(9999);
            mockedRippled.Start();
            //_mockedServerPort = mockedRippled._port;
            client = new XrplClient($"ws://127.0.0.1:{9999}");
            await client.Connect();
            return this;
        }
    }
}
