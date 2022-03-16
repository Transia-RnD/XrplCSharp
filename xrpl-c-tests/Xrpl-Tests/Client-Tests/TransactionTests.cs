using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Types;
using Ripple.TxSigning;
using RippleDotNet.Extensions;
using RippleDotNet.Json.Converters;
using RippleDotNet.Model;
using RippleDotNet.Model.Account;
using RippleDotNet.Model.Transaction;
using RippleDotNet.Model.Transaction.Interfaces;
using RippleDotNet.Model.Transaction.TransactionTypes;
using RippleDotNet.Requests.Ledger;
using RippleDotNet.Requests.Transaction;
using RippleDotNet.Responses.Transaction.Interfaces;
using Currency = RippleDotNet.Model.Currency;

namespace RippleDotNet.Tests
{
    [TestClass]
    public class TransactionTests
    {
        private static IRippleClient client;
        private static IRippleClient xls20client;
        private static JsonSerializerSettings serializerSettings;

        private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
        private static string xls20Url = "wss://xls20-sandbox.rippletest.net:51233";


        //private static string serverUrl = "wss://s1.ripple.com:443";
        //private static string serverUrl = "wss://s2.ripple.com:443";


        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            client = new RippleClient(serverUrl);
            client.Connect();

            xls20client = new RippleClient(xls20Url);
            xls20client.Connect();

            serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.Converters.Add(new TransactionConverter());
        }


        [TestMethod]
        public async Task CanGetTransaction()
        {
            //transaction on mainnet
            RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
            rippleClient.Connect();
            ITransactionResponseCommon transaction = await rippleClient.Transaction("F1CFA020DB5DF2AF3E06D9E84B50EFAA2854D7269238C1F188BE007C9D2B5FB8");
            Assert.IsNotNull(transaction);           
        }

        [TestMethod]
        public async Task CanGetTransactionAsBinary()
        {
            //transaction on mainnet
            RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
            rippleClient.Connect();
            IBaseTransactionResponse transaction = await rippleClient.TransactionAsBinary("5FF261E0E463EF3CA9E2BD4F0754E398A3DBAADF71A3911190C5F9A1241ED403");            
            Assert.IsNotNull(transaction);
        }

        [TestMethod]
        public async Task CanGetTransactions()
        {
            RippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
            rippleClient.Connect();
            var transactions = await rippleClient.AccountTransactions("rPGKpTsgSaQiwLpEekVj1t5sgYJiqf2HDC");
            Console.WriteLine(transactions.Transactions.Count);
        }

        [TestMethod]
        public void CanSerializeAndDeserializeHex()
        {
            //https://ripple.com/build/transactions/#domain
            var domain = "example.com";
            var hex = domain.ToHex();
            Assert.AreEqual(0, string.Compare("6578616d706c652e636f6d", hex, StringComparison.OrdinalIgnoreCase));
            var result = hex.FromHexString();
            Assert.AreEqual(domain, result);
        }

        [TestMethod]
        public void CanCreatePaymentTransaction()
        {
            IPaymentTransaction paymentTransaction = new PaymentTransaction();
            paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            paymentTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
            paymentTransaction.Amount = new Currency{ ValueAsXrp = 1 };

            const string expectedResult = "{\"Amount\":\"1000000\",\"Destination\":\"rEqtEHKbinqm18wQSQGstmqg9SFpUELasT\",\"Flags\":2147483648,\"Account\":\"rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V\",\"TransactionType\":\"Payment\"}";
            
            Assert.AreEqual(expectedResult, paymentTransaction.ToJson());            
        }

        [TestMethod]
        public async Task CanSignAndSubmitPaymentTransaction()
        {
            IPaymentTransaction paymentTransaction = new PaymentTransaction();
            paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            paymentTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
            paymentTransaction.Amount = new Currency { ValueAsXrp = 1 };

            SubmitRequest request = new SubmitRequest();
            request.Transaction = paymentTransaction;
            request.Offline = false;
            request.Secret = "xxxxxxx";

            Submit result = await client.SubmitTransaction(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);            
            Assert.IsNotNull(result.Transaction.Hash);           
        }

        [TestMethod]
        public async Task CanSubmitPaymentTransaction()
        {
            IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
            rippleClient.Connect();

            AccountInfo accountInfo = await rippleClient.AccountInfo("rEqtEHKbinqm18wQSQGstmqg9SFpUELasT");

            IPaymentTransaction paymentTransaction = new PaymentTransaction();
            paymentTransaction.Account = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
            paymentTransaction.Destination = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            paymentTransaction.Amount = new Currency { ValueAsXrp = 20};
            paymentTransaction.Sequence = accountInfo.AccountData.Sequence;
            paymentTransaction.Fee = new Currency{Value = "15"};

            var json = paymentTransaction.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await rippleClient.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);
        }

        [TestMethod]
        public async Task CanEstablishTrust()
        {
            AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

            ITrustSetTransaction trustSet = new TrustSetTransaction();
            trustSet.LimitAmount = new Currency{CurrencyCode = "XYZ", Issuer = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT", Value = "1000000"};
            trustSet.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            trustSet.Sequence = accountInfo.AccountData.Sequence;

            var json = trustSet.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await client.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);
        }

        [TestMethod]
        public async Task CanGetBookOffers()
        {
            IRippleClient rippleClient = new RippleClient("wss://s1.ripple.com:443");
            rippleClient.Connect();
            BookOffersRequest request = new BookOffersRequest();

            request.TakerGets = new Currency { CurrencyCode = "EUR", Issuer = "rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq" };
            request.TakerPays = new Currency();

            //request.TakerGets = new Currency();
            //request.TakerPays = new Currency { CurrencyCode = "EUR", Issuer = "rhub8VRN55s94qWKDv6jmDy1pUykJzF3wq" };

            request.Limit = 10;

            var offers = await rippleClient.BookOffers(request);

            foreach (var bookOffer in offers.Offers)
            {
                Debug.WriteLine(bookOffer.Account);
            }

            Assert.IsNotNull(offers);
        }

        [TestMethod]
        public async Task CanSetOffer()
        {

            AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

            IOfferCreateTransaction offerCreate = new OfferCreateTransaction();
            offerCreate.Sequence = accountInfo.AccountData.Sequence;
            offerCreate.TakerGets = new Currency {ValueAsXrp = 10};
            offerCreate.TakerPays = new Currency{CurrencyCode = "XYZ", Issuer = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT", Value = "10"};
            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
            offerCreate.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";

            var json = offerCreate.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await client.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);

        }

        [TestMethod]
        public async Task CanGetTestNetBookOffers()
        {
            BookOffersRequest request = new BookOffersRequest();
           
            request.TakerGets = new Currency();
            request.TakerPays = new Currency { CurrencyCode = "XYZ", Issuer = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT" };

            request.Limit = 10;

            var offers = await client.BookOffers(request);

            Assert.IsNotNull(offers);            
        }

        [TestMethod]
        public async Task CanFillOrder()
        {
            AccountInfo accountInfo = await client.AccountInfo("rEqtEHKbinqm18wQSQGstmqg9SFpUELasT");

            IOfferCreateTransaction offerCreate = new OfferCreateTransaction();
            offerCreate.Sequence = accountInfo.AccountData.Sequence;
            offerCreate.TakerGets = new Currency { CurrencyCode = "XYZ", Issuer = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT", Value = "10" };
            offerCreate.TakerPays = new Currency { ValueAsXrp = 10 };
            offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
            offerCreate.Account = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";

            var json = offerCreate.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await client.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);
        }

        [TestMethod]
        public async Task CanDeleteTrust()
        {
            AccountInfo accountInfo = await client.AccountInfo("rho3u4kXc5q3chQFKfn9S1ZqUCya1xT3t4");

            ITrustSetTransaction trustSet = new TrustSetTransaction();
            trustSet.Flags = TrustSetFlags.tfSetNoRipple;
            trustSet.Account = "rho3u4kXc5q3chQFKfn9S1ZqUCya1xT3t4";
            trustSet.LimitAmount = new Currency {ValueAsNumber = 0, Issuer = "rDLXQ8KEBn3Aw313bGzhEemx8cCPpGha3d", CurrencyCode = "PHP"};
            trustSet.QualityIn = 0;
            trustSet.QualityOut = 0;
            trustSet.Sequence = accountInfo.AccountData.Sequence;
            trustSet.Fee = new Currency {Value = "12"};

            var json = trustSet.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await client.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);

        }

        [TestMethod]
        public async Task CanReleaseEscrow()
        {
            IEscrowFinishTransaction escrowFinishTransaction = new EscrowFinishTransaction();
            escrowFinishTransaction.Account = "rho3u4kXc5q3chQFKfn9S1ZqUCya1xT3t4";
            escrowFinishTransaction.Owner = "r9NpyVfLfUG8hatuCCHKzosyDtKnBdsEN3";
            escrowFinishTransaction.OfferSequence = 10;
            escrowFinishTransaction.Fee = new Currency{Value = "15"};
            //escrowFinishTransaction.Flags = TransactionFlags.tfFullyCanonicalSig;

            var json = escrowFinishTransaction.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await client.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);

        }

        [TestMethod]
        public async Task CanCreateEscrow()
        {
            IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
            rippleClient.Connect();

            AccountInfo accountInfo = await rippleClient.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

            IEscrowCreateTransaction createTransaction = new EscrowCreateTransaction();
            createTransaction.Amount = new Currency{ValueAsXrp = 10};
            createTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            createTransaction.FinishAfter = DateTime.UtcNow.AddMinutes(1);
            createTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
            createTransaction.Fee = new Currency{Value = "11"};
            createTransaction.Sequence = accountInfo.AccountData.Sequence;

            var json = createTransaction.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await rippleClient.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);
        }

        [TestMethod]
        public async Task CanFinishEscrow()
        {
            IRippleClient rippleClient = new RippleClient("wss://s.altnet.rippletest.net:51233");
            rippleClient.Connect();

            AccountInfo accountInfo = await rippleClient.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

            IEscrowFinishTransaction finishTransaction = new EscrowFinishTransaction();
            finishTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            finishTransaction.Owner = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
            finishTransaction.OfferSequence = 29;
            finishTransaction.Fee = new Currency { Value = "11" };
            //finishTransaction.Flags = TransactionFlags.tfFullyCanonicalSig;
            finishTransaction.Sequence = accountInfo.AccountData.Sequence;

            var json = finishTransaction.ToJson();
            TxSigner signer = TxSigner.FromSecret("xxxxxxx");
            SignedTx signedTx = signer.SignJson(JObject.Parse(json));

            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await rippleClient.SubmitTransactionBlob(request);
            Assert.IsNotNull(result);
            Assert.AreEqual("tesSUCCESS", result.EngineResult);
            Assert.IsNotNull(result.Transaction.Hash);
        }

        // NFT Functions

        // Address: rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7
        // Seed: shqqJc2dqXzB6dEhLDBrVRBPkUQVd

        // Address: rK6x1tM8nsrLzFRk3tAJ4TZMYKectAgEmr
        // Seed: sh2eEdA9XK2QzsyXwX7hNGzqoY6zs

        //[TestMethod]
        //public async Task CanMintToken()
        //{

        //    AccountInfo accountInfo = await xls20client.AccountInfo("rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7");

        //    INFTokenMintTransaction mintToken = new NFTokenMintTransaction();
        //    mintToken.Sequence = accountInfo.AccountData.Sequence;
        //    mintToken.TokenTaxon = 0;
        //    //mintToken.Issuer = "";
        //    mintToken.TransferFee = 0;
        //    mintToken.URI = "ipfs://";
        //    mintToken.Account = "rv2pHEbfVtU4UA5ES8CKD2RckEqhWwfL7";

        //    var json = mintToken.ToJson();
        //    TxSigner signer = TxSigner.FromSecret("shqqJc2dqXzB6dEhLDBrVRBPkUQVd");
        //    System.Diagnostics.Debug.WriteLine(signer.ToString());
        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

        //    SubmitBlobRequest request = new SubmitBlobRequest();
        //    request.TransactionBlob = signedTx.TxBlob;

        //    Submit result = await xls20client.SubmitTransactionBlob(request);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
        //    Assert.IsNotNull(result.Transaction.Hash);

        //}

        //[TestMethod]
        //public async Task CanBurnToken()
        //{

        //    AccountInfo accountInfo = await xls20client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

        //    INFTokenBurnTransaction mintBurn = new NFTokenBurnTransaction();
        //    mintBurn.Sequence = accountInfo.AccountData.Sequence;
        //    mintBurn.TokenID = "1";
        //    mintBurn.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";

        //    var json = mintBurn.ToJson();
        //    TxSigner signer = TxSigner.FromSecret("xxxxxxx");
        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

        //    SubmitBlobRequest request = new SubmitBlobRequest();
        //    request.TransactionBlob = signedTx.TxBlob;

        //    Submit result = await xls20client.SubmitTransactionBlob(request);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
        //    Assert.IsNotNull(result.Transaction.Hash);

        //}

        //[TestMethod]
        //public async Task CanSetOfferOnToken()
        //{

        //    AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

        //    INFTokenCreateOfferTransaction offerCreate = new NFTokenCreateOfferTransaction();
        //    offerCreate.Sequence = accountInfo.AccountData.Sequence;
        //    offerCreate.TokenID = "1";
        //    offerCreate.Amount = "100";
        //    offerCreate.Owner = "";
        //    offerCreate.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
        //    offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
        //    offerCreate.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";

        //    var json = offerCreate.ToJson();
        //    TxSigner signer = TxSigner.FromSecret("xxxxxxx");
        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

        //    SubmitBlobRequest request = new SubmitBlobRequest();
        //    request.TransactionBlob = signedTx.TxBlob;

        //    Submit result = await xls20client.SubmitTransactionBlob(request);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
        //    Assert.IsNotNull(result.Transaction.Hash);

        //}

        //[TestMethod]
        //public async Task CanCancelOfferOnToken()
        //{

        //    AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

        //    IOfferCreateTransaction offerCreate = new OfferCreateTransaction();
        //    offerCreate.Sequence = accountInfo.AccountData.Sequence;
        //    offerCreate.TakerGets = new Currency { ValueAsXrp = 10 };
        //    offerCreate.TakerPays = new Currency { CurrencyCode = "XYZ", Issuer = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT", Value = "10" };
        //    offerCreate.Expiration = DateTime.UtcNow.AddHours(1);
        //    offerCreate.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";

        //    var json = offerCreate.ToJson();
        //    TxSigner signer = TxSigner.FromSecret("xxxxxxx");
        //    SignedTx signedTx = signer.SignJson(JObject.Parse(json));

        //    SubmitBlobRequest request = new SubmitBlobRequest();
        //    request.TransactionBlob = signedTx.TxBlob;

        //    Submit result = await xls20client.SubmitTransactionBlob(request);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("tesSUCCESS", result.EngineResult);
        //    Assert.IsNotNull(result.Transaction.Hash);

        //}

    }
}
