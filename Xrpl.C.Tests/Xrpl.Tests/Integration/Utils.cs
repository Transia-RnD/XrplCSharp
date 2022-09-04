using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using Org.BouncyCastle.Asn1.Ocsp;
using Ripple.Binary.Codec.Types;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.XrplWallet;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Ripple.Binary.Codec.ShaMapTree;
using Newtonsoft.Json;
using System.Collections.Generic;
using Flurl.Http.Configuration;
using Xrpl.Utils.Hashes;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/utils.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    public class Utils
    {

        private static string masterAccount = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
        private static string masterSecret = "snoPBrXtMeMyMHUVTgbuqAfg1SUTb";

        public static async Task LedgerAccept(IRippleClient client)
        {
            var request = new RippleRequest { Command = "ledger_accept" };
            //await client.connection.request(request);
            await client.AnyRequest(request);
        }

        public static async Task FundAccount(IRippleClient client, Wallet wallet)
        {
            Payment payment = new Payment
            {
                Account = masterAccount,
                Destination = wallet.ClassicAddress,
                Amount = new Xrpl.Client.Models.Common.Currency { Value = "400000000", CurrencyCode = "XRP" }
            };
            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(payment.ToJson());
            Submit response = await client.Submit(values, Wallet.FromSeed(masterSecret));
            if (response.EngineResult != "tesSUCCESS")
            {
                throw new Exception("Response not successful, ${ response.result.engine_result}");
            }
            await LedgerAccept(client);
            response.TxJson.Property("hash").Remove();
            await VerifySubmittedTransaction(client, response.TxJson);
        }

        public static async Task<Wallet> GenerateFundedWallet(IRippleClient client)
        {
            Wallet wallet = Wallet.Generate();
            await FundAccount(client, wallet);
            return wallet;
        }

        public static async Task VerifySubmittedTransaction(IRippleClient client, JToken tx, string? hashTx = null)
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

        public static async Task TestTransaction(IRippleClient client, Dictionary<string, dynamic> transaction, Wallet wallet)
        {
            await LedgerAccept(client);
            Submit response = await client.Submit(transaction, wallet);
            //Assert.IsNotNull(response.Type, "response");
            Assert.IsNotNull(response.EngineResult, "tesSUCCESS");
            response.TxJson.Property("hash").Remove();
            await LedgerAccept(client);
            await VerifySubmittedTransaction(client, response.TxJson);
        }
    }
}