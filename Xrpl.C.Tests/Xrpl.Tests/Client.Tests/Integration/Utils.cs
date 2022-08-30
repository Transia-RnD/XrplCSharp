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
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/utils.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    public class Utils
    {

        private static string masterAccount = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";
        private static string masterSecret = "snoPBrXtMeMyMHUVTgbuqAfg1SUTb";

        public async Task LedgerAccept(IRippleClient client)
        {
            var request = new RippleRequest { Command = "ledger_accept" };
            //await client.connection.request(request);
            await client.AnyRequest(request);
        }

        public async Task FundWallet(IRippleClient client, rWallet wallet)
        {
            PaymentTransaction payment = new PaymentTransaction
            {
                Account = masterAccount,
                Destination = wallet.ClassicAddress,
                Amount = new Xrpl.Client.Models.Common.Currency { Value = "400000000", CurrencyCode = "XRP" }
            };
            //PaymentTransactionResponse response = await client.Submit(payment, rWallet.FromSeed(masterSecret, null, null));
            //if (response. !== 'tesSUCCESS')
            //{
            //    // eslint-disable-next-line no-console -- happens only when something goes wrong
            //    console.log(response)
            //  assert.fail(`Response not successful, ${ response.result.engine_result}`)
            //}
            //await ledgerAccept(client)
            //const signedTx = _.omit(response.result.tx_json, 'hash')
            //await verifySubmittedTransaction(client, signedTx as Transaction)
            //      }
        }
    }
}

