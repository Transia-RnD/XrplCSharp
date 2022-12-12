using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Models.Methods;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xrpl.Client;
using Xrpl.Models.Transactions;
using Xrpl.Utils.Hashes;
using Xrpl.Wallet;
using ICurrency = Xrpl.Models.Common.Currency;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

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
            //await client.connection.request(request);
            await client.AnyRequest(request);
        }

        public static async Task FundAccount(IXrplClient client, XrplWallet wallet)
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

        public static async Task<XrplWallet> GenerateFundedWallet(IXrplClient client)
        {
            XrplWallet wallet = XrplWallet.Generate();
            await FundAccount(client, wallet);
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