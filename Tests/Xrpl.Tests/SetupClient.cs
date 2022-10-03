using System.Diagnostics;
using System.Threading.Tasks;
using Xrpl.Client;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/setupClient.ts

namespace XrplTests.Xrpl
{
    public class SetupUnit
    {
        public XrplClient client;
        public CreateMockRippled mockedRippled;
        public int _mockedServerPort;

        public async Task<SetupUnit> SetupClient()
        {
            Debug.WriteLine("SetupClient");
            var promise = new TaskCompletionSource();
            mockedRippled = new CreateMockRippled(9999);
            mockedRippled.Start();
            Debug.WriteLine($"ws://localhost:{9999}");
            //_mockedServerPort = mockedRippled._port;
            //client = new Client("wss://hooks-testnet-v2.xrpl-labs.com");
            //client = new XrplClient($"ws://localhost:{9999}");
            //client.Connect();
            return this;
        }
    }
}
