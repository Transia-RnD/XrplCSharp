using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using Xrpl.Models.Common;
using Xrpl.Models.Methods;
using Xrpl.Client;
using Xrpl.Models.Transactions;
using Xrpl.Utils.Hashes;
using Xrpl.Wallet;
using ICurrency = Xrpl.Models.Common.Currency;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Diagnostics;
using Xrpl.BinaryCodec.Types;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/utils.ts

namespace XrplTests.Xrpl.ClientLib.Integration
{
    public class Utils
    {

        private static string masterAccount = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
        private static string masterSecret = "snoPBrXtMeMyMHUVTgbuqAfg1SUTb";

        public static async Task LedgerAccept(IXrplClient client)
        {
            var request = new BaseRequest { Command = "ledger_accept" };
            await client.AnyRequest(request);
        }

        public static async Task FundNative(IXrplClient client, XrplWallet wallet)
        {
            Payment payment = new Payment
            {
                Account = masterAccount,
                Destination = wallet.ClassicAddress,
                Amount = new ICurrency { Value = "400000000", CurrencyCode = "XRP" }
            };
            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(payment.ToJson());
            Submit response = await client.Submit(values, XrplWallet.FromSeed(masterSecret));
            if (response.EngineResult != "tesSUCCESS")
            {
                throw new Exception("Response not successful, ${ response.result.engine_result}");
            }
            await LedgerAccept(client);
            response.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, response.TxJson);
        }

        /// <summary> currency with issuer </summary>
        public class IntegrationIC
        {
            public string Issuer { get; set; }
            public string IOAccount { get; set; }
            public string IOSeed { get; set; }
        }

        public static async Task<IntegrationIC> IssueIC(IXrplClient client)
        {
            XrplWallet cold = XrplWallet.Generate();
            XrplWallet hot = XrplWallet.Generate();
            await FundNative(client, cold);
            await FundNative(client, hot);
            // ACCOUNT SET
            AccountSet atx = new AccountSet
            {
                Account = cold.ClassicAddress,
                SetFlag = 8
            };
            Dictionary<string, dynamic> atxJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(atx.ToJson());
            Submit aresponse = await client.Submit(atxJson, cold);
            if (aresponse.EngineResult != "tesSUCCESS")
            {
                throw new Exception($"Response not successful, {aresponse.EngineResult}");
            }
            await LedgerAccept(client);
            aresponse.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, aresponse.TxJson);

            // TRUSTLINE
            TrustSet tltx = new TrustSet
            {
                Account = hot.ClassicAddress,
                LimitAmount = new ICurrency
                {
                    CurrencyCode = "USD",
                    Issuer = cold.ClassicAddress,
                    Value = "10000000000"
                }
            };
            Dictionary<string, dynamic> tltxJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tltx.ToJson());
            Submit tlresponse = await client.Submit(tltxJson, hot);
            if (tlresponse.EngineResult != "tesSUCCESS")
            {
                throw new Exception($"Response not successful, {tlresponse.EngineResult}");
            }
            await LedgerAccept(client);
            tlresponse.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, tlresponse.TxJson);

            // PAYMENT
            Payment ptx = new Payment
            {
                Account = cold.ClassicAddress,
                Destination = hot.ClassicAddress,
                Amount = new ICurrency
                {
                    Value = "400000",
                    CurrencyCode = "USD",
                    Issuer = cold.ClassicAddress
                }
            };
            Dictionary<string, dynamic> ptxJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ptx.ToJson());
            Submit presponse = await client.Submit(ptxJson, cold);
            if (presponse.EngineResult != "tesSUCCESS")
            {
                throw new Exception($"Response not successful, {presponse.EngineResult}");
            }
            await LedgerAccept(client);
            presponse.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, presponse.TxJson);
            return new IntegrationIC()
            {
                Issuer = cold.ClassicAddress,
                IOAccount = hot.ClassicAddress,
                IOSeed = hot.Seed,
            };
        }

        public static async Task FundIC(IXrplClient client, XrplWallet wallet, IntegrationIC ic)
        {
            // TRUSTLINE
            TrustSet tltx = new TrustSet
            {
                Account = wallet.ClassicAddress,
                LimitAmount = new ICurrency
                {
                    CurrencyCode = "USD",
                    Issuer = ic.Issuer,
                    Value = "10000000000"
                }
            };
            Dictionary<string, dynamic> tltxJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(tltx.ToJson());
            Submit tlresponse = await client.Submit(tltxJson, wallet);
            if (tlresponse.EngineResult != "tesSUCCESS")
            {
                throw new Exception($"Response not successful, {tlresponse.EngineResult}");
            }
            await LedgerAccept(client);
            tlresponse.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, tlresponse.TxJson);

            // PAYMENT
            Payment ptx = new Payment
            {
                Account = ic.IOAccount,
                Destination = wallet.ClassicAddress,
                Amount = new ICurrency {
                    Value = "1000",
                    CurrencyCode = "USD",
                    Issuer = ic.Issuer
                }
            };
            Dictionary<string, dynamic> ptxJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ptx.ToJson());
            Submit presponse = await client.Submit(ptxJson, XrplWallet.FromSeed(ic.IOSeed));
            if (presponse.EngineResult != "tesSUCCESS")
            {
                throw new Exception($"Response not successful, {presponse.EngineResult}");
            }
            await LedgerAccept(client);
            presponse.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, presponse.TxJson);
        }

        public static async Task<XrplWallet> GenerateFundedWallet(IXrplClient client)
        {
            XrplWallet wallet = XrplWallet.Generate();
            await FundNative(client, wallet);
            return wallet;
        }

        public static async Task VerifySubmittedTransaction(IXrplClient client, JToken tx, string? hashTx = null)
        {
            string hash = hashTx != null ? hashTx : HashLedger.HashSignedTx(tx);
            TxRequest request = new TxRequest(hash);
            TransactionResponseCommon data = await client.Tx(request);
              //assert(data.result)
              //assert.deepEqual(
              //  _.omit(data.result, [
              //    'date',
              //    'hash',
              //    'inLedger',
              //    'ledger_index',
              //    'meta',
              //    'validated',
              //  ]),
              //  typeof tx == 'string' ? decode(tx) : tx,
              //)
              //if (typeof data.result.meta === 'object')
              //          {
              //    assert.strictEqual(data.result.meta.TransactionResult, 'tesSUCCESS')
              //}
              //          else
              //          {
              //    assert.strictEqual(data.result.meta, 'tesSUCCESS')
              //}
        }

        public static async Task TestTransaction(IXrplClient client, Dictionary<string, dynamic> transaction, XrplWallet wallet)
        {
            await LedgerAccept(client);
            Submit response = await client.Submit(transaction, wallet);
            //Assert.IsNotNull(response.Type, "response");
            Assert.AreEqual(response.EngineResult, "tesSUCCESS");
            response.TxJson.Property("hash").Remove();
            await LedgerAccept(client);
            await VerifySubmittedTransaction(client, response.TxJson);
        }
    }
}