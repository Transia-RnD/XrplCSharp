//using System;
//using System.Linq;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Xrpl.Wallet;
//using Xrpl.Client.Extensions;
//using Xrpl.Client.Json.Converters;
//using Xrpl.Client.Models.Methods;
//using Xrpl.Client.Models.Transactions;
//using Xrpl.Client.Models.Common;
//using Xrpl.Client.Models.Enums;

//namespace Xrpl.Client.Tests
//{
//    [TestClass]
//    public class TransactionTests
//    {
//        private static IRippleClient client;
//        private static IRippleClient xls20client;
//        private static JsonSerializerSettings serializerSettings;

//        private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
//        private static string xls20Url = "wss://xls20-sandbox.rippletest.net:51233";
//        //private static string tokenID = "";


//        //private static string serverUrl = "wss://s1.ripple.com:443";
//        //private static string serverUrl = "wss://s2.ripple.com:443";


//        [ClassInitialize]
//        public static void MyClassInitialize(TestContext testContext)
//        {
//            client = new RippleClient(serverUrl);
//            client.Connect();

//            xls20client = new RippleClient(xls20Url);
//            xls20client.Connect();

//            serializerSettings = new JsonSerializerSettings();
//            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
//            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
//            serializerSettings.Converters.Add(new TransactionConverter());
//        }


//        //[TestMethod]
//        //public async Task CanGetTransaction()
//        //{
//        //    //transaction on mainnet
//        //    RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
//        //    rippleClient.Connect();
//        //    ITransactionResponseCommon transaction = await rippleClient.Transaction("F1CFA020DB5DF2AF3E06D9E84B50EFAA2854D7269238C1F188BE007C9D2B5FB8");
//        //    Assert.IsNotNull(transaction);
//        //}

//        //[TestMethod]
//        //public async Task CanGetTransactionAsBinary()
//        //{
//        //    //transaction on mainnet
//        //    RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
//        //    rippleClient.Connect();
//        //    IBaseTransactionResponse transaction = await rippleClient.TransactionAsBinary("5FF261E0E463EF3CA9E2BD4F0754E398A3DBAADF71A3911190C5F9A1241ED403");
//        //    Assert.IsNotNull(transaction);
//        //}

//        [TestMethod]
//        public async Task CanGetTransactions()
//        {
//            RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
//            rippleClient.Connect();
//            var transactions = await rippleClient.AccountTransactions("rPGKpTsgSaQiwLpEekVj1t5sgYJiqf2HDC");
//            Console.WriteLine(transactions.Transactions.Count);
//        }

//        [TestMethod]
//        public void CanSerializeAndDeserializeHex()
//        {
//            //https://ripple.com/build/transactions/#domain
//            var domain = "example.com";
//            var hex = domain.ConvertStringToHex();
//            Assert.AreEqual(0, string.Compare("6578616d706c652e636f6d", hex, StringComparison.OrdinalIgnoreCase));
//            var result = hex.FromHexString();
//            Assert.AreEqual(domain, result);
//        }

//        [TestMethod]
//        public void CanCreatePaymentTransaction()
//        {
//            IPaymentTransaction paymentTransaction = new PaymentTransaction();
//            paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
//            paymentTransaction.Destination = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";
//            paymentTransaction.Amount = new Currency { ValueAsXrp = 1 };

//            const string expectedResult = "{\"Amount\":\"1000000\",\"Destination\":\"rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7\",\"Account\":\"rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V\",\"Fee\":\"10\",\"TransactionType\":\"Payment\"}";

//            Assert.AreEqual(expectedResult, paymentTransaction.ToJson());
//        }

//        //[TestMethod]
//        //public async Task CanSignAndSubmitPaymentTransaction()
//        //{
//        //    IPaymentTransaction paymentTransaction = new PaymentTransaction();
//        //    paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
//        //    paymentTransaction.Destination = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";
//        //    paymentTransaction.Amount = new Currency { ValueAsXrp = 1 };

//        //    SubmitRequest request = new SubmitRequest();
//        //    request.Transaction = paymentTransaction;
//        //    //request.Offline = false;
//        //    request.Secret = "shqqJc2dqXzB6dEhLDBrVRBPkUQVd";

//        //    Submit result = await client.SubmitTransaction(request);
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);            
//        //    Assert.IsNotNull(result.Transaction.Hash);           
//        //}

//        [TestMethod]
//        public async Task CanSubmitPaymentTransaction()
//        {
//            IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
//            rippleClient.Connect();

//            AccountInfo accountInfo = await rippleClient.AccountInfo("rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7");

//            IPaymentTransaction paymentTransaction = new PaymentTransaction();
//            paymentTransaction.Account = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";
//            paymentTransaction.Destination = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
//            paymentTransaction.Amount = new Currency { ValueAsXrp = 20 };
//            paymentTransaction.Sequence = accountInfo.AccountData.Sequence;
//            paymentTransaction.Fee = new Currency { Value = "15" };

//            var json = paymentTransaction.ToJson();
//            TxSigner signer = TxSigner.FromSecret("shqqJc2dqXzB6dEhLDBrVRBPkUQVd");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await rippleClient.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);
//        }

//        [TestMethod]
//        public async Task CanEstablishTrust()
//        {
//            AccountInfo accountInfo = await client.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//            ITrustSetTransaction trustSet = new TrustSetTransaction();
//            trustSet.LimitAmount = new Currency { CurrencyCode = "XYZ", Issuer = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7", Value = "1000000" };
//            trustSet.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//            trustSet.Sequence = accountInfo.AccountData.Sequence;

//            var json = trustSet.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);
//        }

//        [TestMethod]
//        public async Task CanGetBookOffers()
//        {
//            //IRippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
//            //rippleClient.Connect();
//            //BookOffersRequest request = new BookOffersRequest();

//            ////request.TakerGets = new Currency { CurrencyCode = "EUR", Issuer = "rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq" };
//            ////request.TakerPays = new Currency();

//            ////request.TakerGets = new Currency();
//            ////request.TakerPays = new Currency { CurrencyCode = "EUR", Issuer = "rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq" };

//            //request.Limit = 10;

//            //var offers = await rippleClient.BookOffers(request);

//            //foreach (var bookOffer in offers.Offers)
//            //{
//            //    Debug.WriteLine(bookOffer.Account);
//            //}

//            //Assert.IsNotNull(offers);
//        }

//        [TestMethod]
//        public async Task CanSetOffer()
//        {

//            AccountInfo accountInfo = await client.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//            IOfferCreateTransaction offerCreate = new OfferCreateTransaction();
//            offerCreate.Sequence = accountInfo.AccountData.Sequence;
//            offerCreate.TakerGets = new Currency { ValueAsXrp = 10 };
//            offerCreate.TakerPays = new Currency { CurrencyCode = "XYZ", Issuer = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7", Value = "10" };
//            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
//            offerCreate.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";

//            var json = offerCreate.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        //[TestMethod]
//        //public async Task CanGetTestNetBookOffers()
//        //{
//        //    BookOffersRequest request = new BookOffersRequest();

//        //    request.TakerGets = new Currency();
//        //    request.TakerPays = new Currency { CurrencyCode = "XYZ", Issuer = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7" };

//        //    request.Limit = 10;

//        //    var offers = await client.BookOffers(request);

//        //    Assert.IsNotNull(offers);
//        //}

//        [TestMethod]
//        public async Task CanFillOrder()
//        {
//            AccountInfo accountInfo = await client.AccountInfo("rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7");

//            IOfferCreateTransaction offerCreate = new OfferCreateTransaction();
//            offerCreate.Sequence = accountInfo.AccountData.Sequence;
//            offerCreate.TakerGets = new Currency { CurrencyCode = "XYZ", Issuer = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7", Value = "10" };
//            offerCreate.TakerPays = new Currency { ValueAsXrp = 10 };
//            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
//            offerCreate.Account = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";

//            var json = offerCreate.ToJson();
//            TxSigner signer = TxSigner.FromSecret("shqqJc2dqXzB6dEhLDBrVRBPkUQVd");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);
//        }

//        [TestMethod]
//        public async Task CanDeleteTrust()
//        {
//            AccountInfo accountInfo = await client.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//            ITrustSetTransaction trustSet = new TrustSetTransaction();
//            trustSet.Flags = TrustSetFlags.tfSetNoRipple;
//            trustSet.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//            trustSet.LimitAmount = new Currency { Value = "0", Issuer = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7", CurrencyCode = "XYZ" };
//            trustSet.QualityIn = 0;
//            trustSet.QualityOut = 0;
//            trustSet.Sequence = accountInfo.AccountData.Sequence;
//            trustSet.Fee = new Currency { Value = "12" };

//            var json = trustSet.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        //[TestMethod]
//        //public async Task CanReleaseEscrow()
//        //{
//        //    AccountInfo accountInfo = await client.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//        //    IEscrowFinishTransaction escrowFinishTransaction = new EscrowFinishTransaction();
//        //    escrowFinishTransaction.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//        //    escrowFinishTransaction.Owner = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//        //    escrowFinishTransaction.OfferSequence = 10;
//        //    escrowFinishTransaction.Fee = new Currency{Value = "15"};
//        //    escrowFinishTransaction.Sequence = accountInfo.AccountData.Sequence;

//        //    var json = escrowFinishTransaction.ToJson();
//        //    TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//        //    SubmitBlobRequest request = new SubmitBlobRequest();
//        //    request.TransactionBlob = signedTx.TxBlob;

//        //    Submit result = await client.SubmitTransactionBlob(request);
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
//        //    Assert.IsNotNull(result.Transaction.Hash);

//        //}

//        [TestMethod]
//        public async Task CanCreateEscrow()
//        {
//            IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
//            rippleClient.Connect();

//            AccountInfo accountInfo = await rippleClient.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//            IEscrowCreateTransaction createTransaction = new EscrowCreateTransaction();
//            createTransaction.Amount = new Currency { ValueAsXrp = 10 };
//            createTransaction.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//            createTransaction.FinishAfter = DateTime.UtcNow.AddMinutes(1);
//            createTransaction.Destination = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";
//            createTransaction.Fee = new Currency { Value = "11" };
//            createTransaction.Sequence = accountInfo.AccountData.Sequence;

//            var json = createTransaction.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await rippleClient.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);
//        }

//        //[TestMethod]
//        //public async Task CanFinishEscrow()
//        //{
//        //    IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
//        //    rippleClient.Connect();

//        //    AccountInfo accountInfo = await rippleClient.AccountInfo("r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs");

//        //    IEscrowFinishTransaction finishTransaction = new EscrowFinishTransaction();
//        //    finishTransaction.Account = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//        //    finishTransaction.Owner = "r9qDbw1k9JLxC6RigiN9faAcaQrJisyZhs";
//        //    finishTransaction.OfferSequence = 29;
//        //    finishTransaction.Fee = new Currency { Value = "11" };
//        //    finishTransaction.Sequence = accountInfo.AccountData.Sequence;

//        //    var json = finishTransaction.ToJson();
//        //    TxSigner signer = TxSigner.FromSecret("sEdTd6ponfrJs5FE9w1unefuYYjb7ue");
//        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//        //    SubmitBlobRequest request = new SubmitBlobRequest();
//        //    request.TransactionBlob = signedTx.TxBlob;

//        //    Submit result = await rippleClient.SubmitTransactionBlob(request);
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
//        //    Assert.IsNotNull(result.Transaction.Hash);
//        //}

//        [TestMethod]
//        public async Task NFTokenTests()
//        {
//            //await CanMintToken();
//            //await CanBurnToken();
//            //await CanSetNFTokenSellOfferOnToken();
//            //await CanSetBuyOfferOnToken();
//            //await CanCancelOfferOnToken();
//            //await CanAcceptOfferOnToken();
//        }

//        public string GetLastNFTokenID(Meta meta)
//        {
//            return meta.AffectedNodes.Last().ModifiedNode.FinalFields.NFTokens.Last().NFToken.NFTokenID;
//        }

//        //public async Task CanMintToken()
//        //{

//        //    AccountInfo accountInfo = await xls20client.AccountInfo("rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb");

//        //    INFTokenMintTransaction mintToken = new NFTokenMintTransaction();
//        //    mintToken.Sequence = accountInfo.AccountData.Sequence;
//        //    mintToken.NFTokenTaxon = 0;
//        //    mintToken.URI = "697066733a2f2f516d516a447644686648634d7955674441784b696734416f4d547453354a72736670694545704661334639515274";
//        //    mintToken.Account = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";

//        //    var json = mintToken.ToJson();
//        //    TxSigner signer = TxSigner.FromSecret("sEdV9rGxpFHK4zbpMYUVrZ86zv1Lmwn");
//        //    System.Diagnostics.Debug.WriteLine(signer.ToString());
//        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//        //    SubmitBlobRequest request = new SubmitBlobRequest();
//        //    request.TxBlob = signedTx.TxBlob;

//        //    Submit result = await xls20client.SubmitTransactionBlob(request);
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
//        //    Assert.IsNotNull(result.Transaction.Hash);

//        //    System.Threading.Thread.Sleep(5000);
//        //    ITransactionResponseCommon transaction = await xls20client.Tx(result.Transaction.Hash.ToString());
//        //    string something = JsonConvert.SerializeObject(transaction);
//        //    Debug.WriteLine(something);

//        //    Assert.IsNotNull(transaction.Meta);
//        //    //Assert.IsNotNull(GetLastNFTokenID(transaction.Meta));
//        //}

//        public async Task CanBurnToken()
//        {

//            AccountInfo accountInfo = await xls20client.AccountInfo("rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb");

//            INFTokenBurnTransaction mintBurn = new NFTokenBurnTransaction();
//            mintBurn.Sequence = accountInfo.AccountData.Sequence;
//            mintBurn.NFTokenID = "000000007A91B8433CA1328487EF1415E531D4EE20F5952C44B17C9E00000003";
//            mintBurn.Account = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";

//            var json = mintBurn.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdV9rGxpFHK4zbpMYUVrZ86zv1Lmwn");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await xls20client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        public async Task CanSetNFTokenSellOfferOnToken()
//        {

//            AccountInfo accountInfo = await xls20client.AccountInfo("rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb");

//            INFTokenCreateOfferTransaction offerCreate = new NFTokenCreateOfferTransaction();
//            offerCreate.Sequence = accountInfo.AccountData.Sequence;
//            offerCreate.NFTokenID = "000900007A91B8433CA1328487EF1415E531D4EE20F5952C0000099B00000000";
//            offerCreate.Amount = new Currency { ValueAsXrp = 20 };
//            offerCreate.Destination = "rJpMn8yEHeWSQ6w581dJsk8Z2jhCUEWbwD";
//            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
//            offerCreate.Account = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";
//            offerCreate.Flags = NFTokenCreateOfferFlags.tfSellToken;

//            var json = offerCreate.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdV9rGxpFHK4zbpMYUVrZ86zv1Lmwn");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await xls20client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        public async Task CanSetBuyOfferOnToken()
//        {

//            AccountInfo accountInfo = await xls20client.AccountInfo("rJpMn8yEHeWSQ6w581dJsk8Z2jhCUEWbwD");

//            INFTokenCreateOfferTransaction offerCreate = new NFTokenCreateOfferTransaction();
//            offerCreate.Sequence = accountInfo.AccountData.Sequence;
//            offerCreate.NFTokenID = "000900007A91B8433CA1328487EF1415E531D4EE20F5952C0000099B00000000";
//            offerCreate.Amount = new Currency { ValueAsXrp = 20 };
//            offerCreate.Owner = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";
//            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
//            offerCreate.Account = "rJpMn8yEHeWSQ6w581dJsk8Z2jhCUEWbwD";

//            var json = offerCreate.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEd7dzjj8JUZUpnroemkUeSS7gnLr4Z");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await xls20client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        public async Task CanCancelOfferOnToken()
//        {

//            AccountInfo accountInfo = await xls20client.AccountInfo("rJpMn8yEHeWSQ6w581dJsk8Z2jhCUEWbwD");

//            INFTokenCancelOfferTransaction offerCreate = new NFTokenCancelOfferTransaction();
//            offerCreate.Sequence = accountInfo.AccountData.Sequence;
//            string[] tokenOffers = { "000900007A91B8433CA1328487EF1415E531D4EE20F5952C0000099B00000000" };
//            offerCreate.NFTokenOffers = tokenOffers;
//            offerCreate.Account = "rJpMn8yEHeWSQ6w581dJsk8Z2jhCUEWbwD";

//            var json = offerCreate.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEd7dzjj8JUZUpnroemkUeSS7gnLr4Z");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await xls20client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }

//        public async Task CanAcceptOfferOnToken()
//        {

//            AccountInfo accountInfo = await xls20client.AccountInfo("rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb");

//            INFTokenAcceptOfferTransaction offerAccept = new NFTokenAcceptOfferTransaction();
//            offerAccept.Sequence = accountInfo.AccountData.Sequence;
//            offerAccept.NFTokenID = "000900007A91B8433CA1328487EF1415E531D4EE20F5952C0000099B00000000";
//            offerAccept.Account = "rBgHF5VQF74Gi2yFVppDbD9tL9radvkLmb";

//            var json = offerAccept.ToJson();
//            TxSigner signer = TxSigner.FromSecret("sEdV9rGxpFHK4zbpMYUVrZ86zv1Lmwn");
//            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

//            SubmitBlobRequest request = new SubmitBlobRequest();
//            request.TxBlob = signedTx.TxBlob;

//            Submit result = await xls20client.SubmitTransactionBlob(request);
//            Assert.IsNotNull(result);
//            Assert.AreEqual("tesSUCCESS", result.EngineResult);
//            Assert.IsNotNull(result.Transaction.Hash);

//        }
//    }
//}
