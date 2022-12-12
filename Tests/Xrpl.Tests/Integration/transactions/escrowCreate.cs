using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/escrowCreate.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestIEscrowCreate
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

            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            LedgerRequest request = new LedgerRequest() { LedgerIndex = index };
            LOLedger ledgerResponse = await runner.client.Ledger(request);
            LedgerEntity ledgerEntity = (LedgerEntity)ledgerResponse.LedgerEntity;
            DateTime closeTime = ledgerEntity.CloseTime;

            XrplWallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            EscrowCreate setupTx = new EscrowCreate
            {
                Account = runner.wallet.ClassicAddress,
                Amount = new Currency { ValueAsXrp = 10000 },
                Destination = wallet2.ClassicAddress,
                FinishAfter = closeTime.AddSeconds(2),
            };
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);

            AccountObjectsRequest request2 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "escrow" };
            AccountObjects response2 = await runner.client.AccountObjects(request2);
            Assert.AreEqual(response2.AccountObjectList.Count, 1);
        }
    }
}