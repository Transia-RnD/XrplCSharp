using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
using System;
using System.Diagnostics;


namespace Xrpl.Client.Tests
{
    [TestClass]
    public class AccountTests
    {
        private static string account;
        private static string xls20Account = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";

        private static string nft_id = "00090000751F85F13C661A20C610A2C32EBB48CE5819784E0000099B00000000";

        private static IRippleClient client;
        private static IRippleClient xls20client;

        //private static string serverUrl = "wss://s1.ripple.com:443";
        private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
        private static string xls20Url = "wss://xls20-sandbox.rippletest.net:51233";


        [ClassInitialize]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();

            xls20client = new RippleClient(xls20Url);
            xls20client.Connect();

            string faucetUrl = "https://faucet.altnet.rippletest.net/accounts";
            AccountResponse result = await faucetUrl.PostAsync().ReceiveJson<AccountResponse>();
            account = result.Account.Address;
        }

        [TestMethod]
        public async Task CanPerformPing()
        {
            await client.Ping();            
        }

        [TestMethod]
        public async Task CanGetAccountInfo()
        {
            AccountInfo accountInfo = await client.AccountInfo(account);
            Assert.IsNotNull(accountInfo);
        }

        [TestMethod]
        public async Task CanGetAccountChannels()
        {
            var accountChannels = await client.AccountChannels(account);
            Assert.IsNotNull(accountChannels);
        }

        [TestMethod]
        public async Task CanGetAccountCurrencies()
        {
            var currencies = await client.AccountCurrencies(account);
            Assert.IsNotNull(currencies);
        }

        [TestMethod]
        public async Task CanGetAccountLines()
        {
            var accountLines = await client.AccountLines(account);
            Assert.IsNotNull(accountLines);
        }

        [TestMethod]
        public async Task CanGetAccountNFTs()
        {
            AccountNFTs accountNfts = await xls20client.AccountNFTs(xls20Account);
            Assert.IsNotNull(accountNfts);
        }

        [TestMethod]
        public async Task CanGetAccountOffers()
        {
            AccountOffers accountOffers = await client.AccountOffers("rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn");
            Assert.IsNotNull(accountOffers);
        }

        [TestMethod]
        public async Task CanGetAccountOffersRequest()
        {
            AccountOffersRequest offersRequest = new AccountOffersRequest(account) { Limit = 100 };
            var accountOffers = await client.AccountOffers(offersRequest);
            Assert.IsNotNull(accountOffers);
        }

        [TestMethod]
        public async Task CanGetNFTBuyOffers()
        {
            var nftBuyOffers = await xls20client.NFTBuyOffers(nft_id);
            Assert.IsNotNull(nftBuyOffers);
        }

        [TestMethod]
        public async Task CanGetNFTNFTokenSellOffers()
        {
            var nftNFTokenSellOffers = await xls20client.NFTSellOffers(nft_id);
            Assert.IsNotNull(nftNFTokenSellOffers);
        }

        [TestMethod]
        public async Task CanGetAccountObjects()
        {
            var accountObjects = await client.AccountObjects(account);
            Assert.IsNotNull(accountObjects);
        }

        [TestMethod]
        public async Task CanGetAccountTransactions()
        {
            AccountTransactions accountTransactions = await client.AccountTransactions(account);
            Assert.IsNotNull(accountTransactions);
        }

        [TestMethod]
        public async Task CanGetAccountTransactionsAsBinary()
        {
            AccountTransactionsRequest transactionsRequest = new AccountTransactionsRequest(account);
            transactionsRequest.Binary = true;
            var accountTransactions = await client.AccountTransactions(transactionsRequest);
            Assert.IsNotNull(accountTransactions);
        }

        [TestMethod]
        public void CanCreateNoRippleCheckRequest()
        {
            NoRippleCheckRequest request = new NoRippleCheckRequest(account);
            request.Role = RoleType.User;

            string json = JsonConvert.SerializeObject(request);
            JObject jObject = JObject.Parse(json);
            JToken role = jObject["role"];
            Assert.AreEqual("user", role.Value<string>());
        }

        [TestMethod]
        public async Task CanPerformNoRippleCheck()
        {
            NoRippleCheck noRippleCheck = await client.NoRippleCheck(account);
            Assert.IsNotNull(noRippleCheck);
        }

        [TestMethod]
        public async Task CanPerformNoRippleCheckWithRequest()
        {
            NoRippleCheckRequest request = new NoRippleCheckRequest(account);
            request.Role = RoleType.Gateway;
            NoRippleCheck noRippleCheck = await client.NoRippleCheck(request);            
            Assert.IsNotNull(noRippleCheck);
        }

        [TestMethod]
        public async Task CanGetGatewayBalances()
        {
            var balances = await client.GatewayBalances(account);
            foreach (var currency in balances.Assets)
            {
                string issuer = currency.Key;

                foreach (var c in currency.Value)
                {
                    Debug.WriteLine(c);
                    //    var cur = (string)c.currency;
                    //    var value = (string)c.value;
                    //    var result = new Currency() { CurrencyCode = cur.Length > 3 ? cur.FromHexString().Trim('\0') : cur, Issuer = issuer, Value = value };

                    //    Debug.WriteLine(result.Issuer, result.CurrencyCode, result.Value, result.ValueAsNumber);

                }
            }
            Assert.IsNotNull(balances);
        }

        [TestMethod]
        public void CanDecodeMetaBinary()
        {
            var meta = "201C00000000F8E511006125004D1B5655AB8953C6EFD2EBDD5D6314CE0DAFF4CA5B180AE1792B47468ECA5EC5AED28CA45698430F87624433A745D429635D1B2B982EF22AC7E4112ACF43DA97AEF258DC76E6624000000254C5E530E1E7220000000024000000022D00000000624000000254D527708114A2D0815DD52160FF1979A60C50B00C09ECD669D4E1E1E511006125004D1B5655AB8953C6EFD2EBDD5D6314CE0DAFF4CA5B180AE1792B47468ECA5EC5AED28CA456CECECAD86B8E831AA8DB7C8AE259D7D5C1B85C6A5FB9A4341CF560DFB8242F10E624000000126240000002534F23B0E1E7220000000024000000132D000000016240000002533FE1668114656CFDA8B366CAFE7EDC195A6DE87921FB70C231E1E1F1031000";
            StObject obj = StObject.FromHex(meta);
            Assert.IsNotNull(obj);
        }
    }

    public class AccountResponse
    {
        public Account Account { get; set; }

        public int Amount { get; set; }

        public int Balance { get; set; }
    }

    public class Account
    {
        public string XAddress { get; set; }
        public string Secret { get; set; }

        public string ClassicAddress { get; set; }

        public string Address { get; set; }
    }
}
