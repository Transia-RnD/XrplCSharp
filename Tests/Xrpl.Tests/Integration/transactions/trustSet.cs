using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Models.Common;
using Xrpl.Models.Transactions;
using Xrpl.WalletLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/transactions/trustSet.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    [TestClass]
    public class TestITrustSet

    {
        // private static int Timeout = 20;
        public TestContext TestContext;

        public static SetupIntegration runner;

        [ClassInitialize]
        public static async Task MyClassInitializeAsync(TestContext testContext)
        {
            runner = await new SetupIntegration().SetupClient(ServerUrl.serverUrl);
        }

        [TestMethod]
        public async Task TestRequestMethod()
        {
            Wallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
            Currency limitAmount = new Currency
            {
                CurrencyCode = "USD",
                Issuer = wallet2.ClassicAddress,
                Value = "100"
            };
            TrustSet tx = new TrustSet
            {
                Account = runner.wallet.ClassicAddress,
                LimitAmount = limitAmount
            };
            Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
            await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        }

        //[TestMethod]
        //public async Task TestQualityLessOne()
        //{
        //    Wallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
        //    Currency limitAmount = new Currency
        //    {
        //        CurrencyCode = "USD",
        //        Issuer = wallet2.ClassicAddress,
        //        Value = "100"
        //    };
        //    TrustSet tx = new TrustSet
        //    {
        //        Account = runner.wallet.ClassicAddress,
        //        LimitAmount = limitAmount,
        //        QualityIn = PercentToQuality("99%"),
        //        QualityOut = PercentToQuality("99%"),
        //    };
        //    Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
        //    await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        //}

        //[TestMethod]
        //public async Task TestQualityGreaterOne()
        //{
        //    Wallet wallet2 = await Utils.GenerateFundedWallet(runner.client);
        //    Currency limitAmount = new Currency
        //    {
        //        CurrencyCode = "USD",
        //        Issuer = wallet2.ClassicAddress,
        //        Value = "100"
        //    };
        //    TrustSet tx = new TrustSet
        //    {
        //        Account = runner.wallet.ClassicAddress,
        //        LimitAmount = limitAmount,
        //        QualityIn = PercentToQuality("101%"),
        //        QualityOut = PercentToQuality("101%"),
        //    };
        //    Dictionary<string, dynamic> txJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tx.ToJson());
        //    await Utils.TestTransaction(runner.client, txJson, runner.wallet);
        //}
    }
}
