using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/submit.ts

namespace Xrpl.Sugar
{
    public class SubmitSugar
    {
        /// <summary>
        /// Submits a signed/unsigned transaction.<br/>
        /// Steps performed on a transaction:<br/>
        /// 1.<br/>
        /// Autofill.<br/>
        /// 2.<br/>
        /// Sign and Encode.<br/>
        /// 3.<br/>
        /// Submit.
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="transaction">A transaction to autofill, sign and encode, and submit.</param>
        /// <param name="autofill">If true, autofill a transaction.</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <param name="wallet">A wallet to sign a transaction. It must be provided when submitting an unsigned transaction.</param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static async Task<Submit> Submit(
            IXrplClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet wallet = null
        )
        {
            string signedTx = await SubmitSugar.GetSignedTx(client, transaction, autofill, false, wallet);
            return await SubmitRequest(client, signedTx, failHard);
        }

        /// <summary>
        /// Encodes and submits a signed transaction.
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="signedTransaction">signed Transaction</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <returns></returns>
        public static async Task<Submit> SubmitRequest(IXrplClient client, string signedTransaction, bool failHard)
        {
            //if (!isSigned(signedTransaction)) {
            //    throw new ValidationException('Transaction must be signed')
            //}

            //string signedTxEncoded = typeof signedTransaction === 'string' ? signedTransaction : encode(signedTransaction)
            //string signedTxEncoded = BinaryCodec.Encode(signedTransaction);
            string signedTxEncoded = signedTransaction;
            //SubmitBlobRequest request = new SubmitBlobRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = isAccountDelete(signedTransaction) || failHard };
            SubmitRequest request = new SubmitRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = false  };
            var response = await client.GRequest<Submit, SubmitRequest>(request);
            return response;
        }

        /// <summary>
        /// Initializes a transaction for a submit request
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="transaction">A transaction to autofill, sign & encode, and submit.</param>
        /// <param name="autofill">If true, autofill a transaction.</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <param name="wallet">A wallet to sign a transaction. It must be provided when submitting an unsigned transaction.</param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static async Task<string> GetSignedTx(
            IXrplClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet? wallet = null
        )
        {
            //if (isSigned(transaction))
            //{
            //    return transaction
            //}

            if (wallet == null)
            {
                throw new ValidationException("Wallet must be provided when submitting an unsigned transaction");
            }
            Dictionary<string, dynamic> tx = transaction;
            //let tx =
            //  typeof transaction === 'string'
            //    ? // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- converts JsonObject to correct Transaction type
            //      (decode(transaction) as unknown as Transaction)
            //    : transaction
            if (autofill)
            {
                tx = await client.Autofill(tx);
            }
            Debug.WriteLine(JsonConvert.SerializeObject(tx));
            return wallet.Sign(tx, false).TxBlob;
        }
    }
}

