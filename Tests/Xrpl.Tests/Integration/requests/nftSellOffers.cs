

//// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/requests/accountNFTs.ts

//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Xrpl.Models.Common;
//using Xrpl.Models.Ledger;
//using Xrpl.Models.Methods;

//namespace XrplTests.Xrpl.ClientLib.Integration
//{
//    [TestClass]
//    public class TestINFTBuyOffers
//    {
//        // private static int Timeout = 20;
//        public TestContext TestContext { get; set; }
//        public static SetupIntegration runner;

//        [ClassInitialize]
//        public static async Task MyClassInitializeAsync(TestContext testContext)
//        {
//            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
//        }

//        [TestMethod]
//        public async Task TestRequestMethod()
//        {
//            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
//            string nft_id = "";
//            NFTSellOffersRequest request = new NFTSellOffersRequest(nft_id);
//            NFTSellOffers response = await runner.client.NFTSellOffers(request);
//            Assert.IsNotNull(response);
//        }
//    }
//}