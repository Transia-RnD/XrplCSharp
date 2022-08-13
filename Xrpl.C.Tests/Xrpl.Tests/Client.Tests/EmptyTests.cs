using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
using Xrpl.Client.Json.Converters;
using System;
using System.Diagnostics;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class EmptyTests
    {
        private static string account;
        private static IRippleClient client;

        private static string serverUrl = "wss://xrplcluster.com";
        
        [ClassInitialize]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();
            account = "rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p";
        }

        [TestMethod]
        public async Task CanGetAccountOffersVerify()
        {
            try
            {
                AccountOffers accountOffers = await client.AccountOffers("rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p");
                Debug.WriteLine(accountOffers.Offers.Count);
                Assert.IsNotNull(accountOffers);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        [TestMethod]
        public async Task CanGetGatewayBalances()
        {
            var balances = await client.GatewayBalances("rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p");
            foreach (var currency in balances.Assets)
            {
                string issuer = currency.Name;
                Debug.WriteLine(issuer);
                foreach (var c in currency.Value)
                {
                    string cur = c.currency;
                    Currency result = new Currency() { CurrencyCode = c.currency, Issuer = issuer, Value = c.value };
                    Debug.WriteLine(result.CurrencyCode);
                    Debug.WriteLine(result.Value);
                    Debug.WriteLine(result.ValueAsNumber);
                    //    Debug.WriteLine(result.Issuer, result.CurrencyCode, result.Value, result.ValueAsNumber);

                }
            }
            Assert.IsNotNull(balances);
        }

        [TestMethod]
        public async Task CanGetAccountLines()
        {
            var accountLines = await client.AccountLines(account);
            foreach (var line in accountLines.TrustLines)
            {
                Debug.WriteLine(line.Balance);
                Debug.WriteLine(line.BalanceAsNumber.ToString());
            }
            Assert.IsNotNull(accountLines);
        }

    }
}
