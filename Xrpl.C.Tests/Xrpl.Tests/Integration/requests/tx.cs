using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Ripple.Binary.Codec.Types;
using Xrpl.Client;
using Xrpl.Client.Extensions;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Client.Tests;
using Xrpl.Utils.Hashes;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/tx.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    [TestClass]
    public class TestITx
    {
        //// private static int Timeout = 20;
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

            string account = runner.wallet.ClassicAddress;
            AccountSet request = new AccountSet
            {
                Account = account,
                Domain = "example.com".ConvertStringToHex()
            };

            Dictionary<string, dynamic> txRequest = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(request.ToJson());
            Submit response = await runner.client.Submit(txRequest, runner.wallet);
            string hash = HashLedger.HashSignedTx(response.TxBlob);
            TxRequest request1 = new TxRequest(hash);
            TransactionResponseCommon accountTx = await runner.client.Tx(request1);
            Assert.IsNotNull(accountTx);
        }
    }
}