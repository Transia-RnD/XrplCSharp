using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.BinaryCodec.Types;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using static Xrpl.Client.Connection;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/connection.ts

namespace Xrpl.Tests
{

    [TestClass]
    public class TestUConnection
    {

        public static SetupUnitClient runner;

        [TestInitialize]
        public async Task MyTestInitializeAsync()
        {
            runner = await new SetupUnitClient().SetupClient();
        }

        [TestCleanup]
        public async Task MyTestCleanupAsync()
        {
            await runner.client.Disconnect();
        }

        [TestMethod]
        public void TestDefaultOptions()
        {
            ConnectionOptions options = new ConnectionOptions();
            Connection connection = new Connection("url", options);
            Assert.AreEqual(connection.GetUrl(), "url");
            Assert.IsTrue(connection.config.proxy == null);
            Assert.IsTrue(connection.config.authorization == null);
        }

        //[TestMethod]
        //public async void TestMultipleDisconnect()
        //{
        //    await runner.client.Disconnect();
        //    await runner.client.Disconnect();
        //}

        //[TestMethod]
        //public void TestReconnect()
        //{
        //    runner.client.connection.Reconnect();
        //}

        [TestMethod]
        [ExpectedException(typeof(NotConnectedException))]
        public async Task TestNotConnectedException()
        {
            ConnectionOptions options = new ConnectionOptions();
            Connection connection = new Connection("url", options);

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "ledger" },
                { "ledger_index", "validated" },
            };
            await connection.Request(tx, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DisconnectedException))]
        public async Task TestDisconnectedError()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "test_command" },
                { "data", new Dictionary<string, dynamic> {
                   { "closeServer", true },
                } },
            };
            await runner.client.Request(tx);
        }

        //[TestMethod]
        //public void TestTimeoutError()
        //{
        //    
        //}

        //[TestMethod]
        //public void TestDisconnectedErrorOnSend()
        //{
        //    
        //}

        //[TestMethod]
        //public void TestDisconnectedErrorOnInitial()
        //{
        //    
        //}

        //[TestMethod]
        //public void TestResponseFormatError()
        //{
        //    
        //}

        //[TestMethod]
        //public void TestReconnectUnexpected()
        //{
        //    
        //}

        //[TestMethod]
        //public void TestReconnectUnexpected()
        //{
        //    
        //}

        [TestMethod]
        [ExpectedException(typeof(XrplException))]
        public async Task TestNoCrashError()
        {
            runner.mockedRippled.suppressOutput = true;
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "test_garbage" },
            };
            await runner.client.connection.Request(tx);
        }
    }
}
