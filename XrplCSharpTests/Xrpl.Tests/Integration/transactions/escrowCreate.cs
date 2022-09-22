using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Tests.Client.Tests;
using Xrpl.XrplWallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/escrowCreate.ts

namespace Xrpl.Tests.Client.Tests.Integration
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

            Wallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            EscrowCreate setupTx = new EscrowCreate
            {
                Account = runner.wallet.ClassicAddress,
                Amount = new Currency { ValueAsXrp = 10000 },
                Destination = wallet2.ClassicAddress,
                FinishAfter = closeTime.AddSeconds(2),
            };
            Debug.WriteLine(setupTx.ToJson());
            Dictionary<string, dynamic> setupJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(setupTx.ToJson());
            await Utils.TestTransaction(runner.client, setupJson, runner.wallet);

            AccountObjectsRequest request2 = new AccountObjectsRequest(runner.wallet.ClassicAddress) { Type = "escrow" };
            AccountObjects response2 = await runner.client.AccountObjects(request2);
            Assert.AreEqual(response2.AccountObjectList.Count, 1);
        }
    }
}