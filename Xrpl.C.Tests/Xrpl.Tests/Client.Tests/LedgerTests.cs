using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Enums;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class LedgerTests
    {
        private static IRippleClient client;

        private static string serverUrl = "wss://s1.ripple.com:443";

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();
        }

        [TestMethod]
        public async Task CanGetLedger()
        {
            var request = new LedgerRequest { LedgerIndex = new LedgerIndex(LedgerIndexType.Validated), Transactions = true, Expand = true };
            var ledger = await client.Ledger(request);
            Assert.IsNotNull(ledger);
        }

        [TestMethod]
        public async Task CanGetLedgerAsBinary()
        {
            var request = new LedgerRequest { LedgerIndex = new LedgerIndex(LedgerIndexType.Validated), Transactions = true, Binary = true };
            var ledger = await client.Ledger(request);
            LedgerBinaryEntity entity = (LedgerBinaryEntity)ledger.LedgerEntity;

            for (int i = 0; i < entity.Transactions.Count; ++i)
            {
                Console.WriteLine(entity.Transactions[i].ToString());
                string TransactionHash = entity.Transactions[i].ToString();
                IBaseTransactionResponse transaction = await client.Transaction(TransactionHash);
                Assert.IsNotNull(transaction);
            }
            Assert.IsNotNull(ledger);
        }

        //[TestMethod]
        //public async Task CanPerformSubscribe()
        //{
        //    var request = new SubscribeRequest { Streams = new List<string> { "ledger" } };
        //    await client.Subscribe(request).ContinueWith(t =>
        //    {
        //        //Console.WriteLine(t);
        //        var ledger = t.Result;
        //        Console.WriteLine(ledger);
        //    });
        //    //Console.WriteLine(result);
        //}

        [TestMethod]
        public async Task CanGetClosedLedger()
        {
            var ledger = await client.ClosedLedger();
            Assert.IsNotNull(ledger);
        }

        [TestMethod]
        public async Task CanGetCurrentLedger()
        {
            var ledger = await client.CurrentLedger();
            Assert.IsNotNull(ledger);
        }

        [TestMethod]
        public async Task CanGetLedgerData()
        {
            var closedLedger = await client.ClosedLedger();
            LedgerDataRequest request = new LedgerDataRequest();
            request.LedgerHash = closedLedger.LedgerHash;
            var ledgerData = await client.LedgerData(request);
            Assert.IsNotNull(ledgerData);
        }

        [TestMethod]
        public async Task CanGetLedgerDataAsBinary()
        {
            var closedLedger = await client.ClosedLedger();
            LedgerDataRequest request = new LedgerDataRequest();
            request.LedgerHash = closedLedger.LedgerHash;
            request.Binary = true;
            var ledgerData = await client.LedgerData(request);
            Assert.IsNotNull(ledgerData);
        }

        [TestMethod]
        public void CanDecodeBinary()
        {
            string binary =
                "1100722200210000250178D1CA37000000000000000038000000000000028355C0C37CE200B509E0A529880634F7841A9EF4CB65F03C12E6004CFAD9718D66946280000000000000000000000000000000000000004743420000000000000000000000000000000000000000000000000166D6071AFD498D000000000000000000000000000047434200000000002599D1D255BCA61189CA64C84528F2FCBE4BFC3867800000000000000000000000000000000000000047434200000000006EEBB1D1852CE667876A0B3630861FB6C6AB358E";

            var obj = StObject.FromHex(binary);
            Assert.IsNotNull(obj);
        }
    }
}
