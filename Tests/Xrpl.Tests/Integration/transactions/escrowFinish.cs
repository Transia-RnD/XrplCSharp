using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/escrowFinish.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIEscrowFinish
    {
        // private static int Timeout = 20;
        public TestContext TestContext { get; set; }
        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        [TestMethod]
        public async Task TestRequestMethod()
        {

            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            LedgerRequest request = new LedgerRequest() { LedgerIndex = index };
            LOLedger ledgerResponse = await runner.client.Ledger(request);
            LedgerEntity ledgerEntity = (LedgerEntity)ledgerResponse.LedgerEntity;
            uint closeTime = ledgerEntity.CloseTime;

            XrplWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            EscrowCreate setupTx = new EscrowCreate
            {
                Account = runner.wallet.ClassicAddress,
                Amount = new Currency { ValueAsXrp = 100 },
                Destination = wallet2.ClassicAddress,
                FinishAfter = closeTime + 2,
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);

            // hash
            AccountObjectsRequest request1 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "escrow" };
            AccountObjects response1 = await runner.client.AccountObjects(request1);
            LOEscrow escrow = (LOEscrow)response1.AccountObjectList[0];

            TxRequest request2 = new TxRequest(escrow.PreviousTxnID);
            TransactionResponseCommon response2 = await runner.client.Tx(request2);
            uint sequence = (uint)response2.Sequence;

            // actual test - EscrowFinish
            EscrowFinish tx = new EscrowFinish
            {
                Account = runner.wallet.ClassicAddress,
                Owner = runner.wallet.ClassicAddress,
                OfferSequence = sequence
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);

            // get check ID
            AccountObjectsRequest request3 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "escrow" };
            AccountObjects response3 = await runner.client.AccountObjects(request3);
            Assert.AreEqual(response3.AccountObjectList.Count, 0);
        }
    }
}