

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/ledgerEntry.ts

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestILedgerEntryRequests
    {
        // private static int Timeout = 20;
        public TestContext TestContext { get; set; }
        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        //[TestMethod]
        //public async Task TestRequestMethod()
        //{
        //    LedgerIndex li = new LedgerIndex(LedgerIndexType.Validated);
        //    LedgerDataRequest request1 = new LedgerDataRequest() { LedgerIndex = li };
        //    LOLedgerData response1 = await runner.client.LedgerData(request1);
        //    string index = response1.State[0].LedgerObject.Index;

        //    LedgerEntryRequest request = new LedgerEntryRequest() { Index = index, LedgerIndex = li };
        //    LOLedgerEntry response = await runner.client.LedgerEntry(request);
        //    Assert.IsNotNull(response);
        //}
    }
}