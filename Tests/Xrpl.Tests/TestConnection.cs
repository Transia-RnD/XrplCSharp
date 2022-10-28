﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.BinaryCodec.Types;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using static Xrpl.Client.Connection;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/connection.ts

namespace XrplTests.Xrpl
{

    [TestClass]
    public class TestConnection
    {

        public static SetupUnitClient runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupUnitClient().SetupClient();
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

        [TestMethod]
        public void TestMultipleDisconnect()
        {
            runner.client.Disconnect();
            runner.client.Disconnect();
        }

        //[TestMethod]
        //public void TestReconnect()
        //{
        //    runner.client.connection.Reconnect();
        //}

        [TestMethod]
        [ExpectedException(typeof(NotConnectedError))]
        public void TestNotConnectedError()
        {
            ConnectionOptions options = new ConnectionOptions();
            Connection connection = new Connection("url", options);

            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "ledger" },
                { "ledger_index", "validated" },
            };
            //string jtoken = JsonConvert.SerializeObject(tx);
            connection.Request(tx, null);
        }

        [TestMethod]
        public void TestSomething()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "ledger" },
                { "ledger_index", "validated" },
            };
            //string jtoken = JsonConvert.SerializeObject(tx);
            runner.client.Request(tx).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(NotConnectedError))]
        public void TestDisconnectedError()
        {
            Dictionary<string, dynamic> tx = new Dictionary<string, dynamic>
            {
                { "command", "test_command" },
                { "data", new Dictionary<string, dynamic> {
                   { "closeServer", true },
                } },
            };
            //string jtoken = JsonConvert.SerializeObject(tx);
            runner.client.Request(tx).Wait();
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
    }
}
