
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/testUtils.ts

using System.Net;
using System.Net.Sockets;

namespace Xrpl.Tests
{
    public class TestUtils
    {
        static public int GetFreePort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}

