using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Ripple.Binary.Codec;
using Xrpl.Client;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/submit.ts

namespace Xrpl.Sugar
{
    public class SubmitSugar
    {
        /// <summary>
        /// Submits a signed/unsigned transaction.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="transaction"></param>
        /// <param name="autofill"></param>
        /// <param name="failHard"></param>
        /// <param name="wallet"></param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static async Task<Submit> Submit(
            IRippleClient client,
            JToken transaction,
            bool autofill = false,
            bool failHard = false,
            rWallet wallet = null
        )
        {
            string signedTx = await SubmitSugar.GetSignedTx(client, transaction, autofill, wallet);
            return await SubmitRequest(client, signedTx, failHard);
        }

        // Encodes and submits a signed transaction.
        public static Task<Submit> SubmitRequest(IRippleClient client, object signedTransaction, bool failHard)
        {
            //if (!isSigned(signedTransaction)) {
            //    throw new ValidationError('Transaction must be signed')
            //}

            //string signedTxEncoded = typeof signedTransaction === 'string' ? signedTransaction : encode(signedTransaction)
            string signedTxEncoded = BinaryCodec.Encode(signedTransaction);
            //SubmitBlobRequest request = new SubmitBlobRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = isAccountDelete(signedTransaction) || failHard };
            SubmitBlobRequest request = new SubmitBlobRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = false  };
            return client.SubmitTransactionBlob(request);

        }

        /// <summary>
        /// Initializes a transaction for a submit request
        /// </summary>
        /// <param name="client"></param>
        /// <param name="transaction"></param>
        /// <param name="autofill"></param>
        /// <param name="failHard"></param>
        /// <param name="wallet"></param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static async Task<string> GetSignedTx(
            IRippleClient client,
            JToken transaction,
            bool autofill,
            rWallet wallet
        )
        {
            //if (isSigned(transaction))
            //{
            //    return transaction
            //}

            //if (!wallet)
            //{
            //    throw new ValidationError(
            //      'Wallet must be provided when submitting an unsigned transaction',

            //    )
            //}
            JToken tx = transaction;
            //let tx =
            //  typeof transaction === 'string'
            //    ? // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- converts JsonObject to correct Transaction type
            //      (decode(transaction) as unknown as Transaction)
            //    : transaction
            if (autofill)
            {
                tx = await client.Autofill(tx);
            }

            return wallet.Sign(tx, false).TxBlob;
        }
    }
}

