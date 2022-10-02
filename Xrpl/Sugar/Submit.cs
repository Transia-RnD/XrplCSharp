using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.ClientLib;
using Xrpl.ClientLib.Exceptions;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.WalletLib;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/submit.ts

namespace Xrpl.Sugar
{
    public class SubmitSugar
    {
        /// <summary>
        /// Submits a signed/unsigned transaction.
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="transaction">A transaction to autofill, sign & encode, and submit.</param>
        /// <param name="autofill">If true, autofill a transaction.</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <param name="wallet">A wallet to sign a transaction. It must be provided when submitting an unsigned transaction.</param>
        /// <returns>A Wallet derived from a seed.</returns>
        public static async Task<Submit> Submit(
            IClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            Wallet wallet = null
        )
        {
            string signedTx = await SubmitSugar.GetSignedTx(client, transaction, autofill, false, wallet);
            return await SubmitRequest(client, signedTx, failHard);
        }

        // Encodes and submits a signed transaction.
        public static async Task<Submit> SubmitRequest(IClient client, string signedTransaction, bool failHard)
        {
            //if (!isSigned(signedTransaction)) {
            //    throw new ValidationError('Transaction must be signed')
            //}

            //string signedTxEncoded = typeof signedTransaction === 'string' ? signedTransaction : encode(signedTransaction)
            //string signedTxEncoded = BinaryCodec.Encode(signedTransaction);
            string signedTxEncoded = signedTransaction;
            //SubmitBlobRequest request = new SubmitBlobRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = isAccountDelete(signedTransaction) || failHard };
            SubmitRequest request = new SubmitRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = false  };
            return await client.Submit(request);
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
            IClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            Wallet? wallet = null
        )
        {
            //if (isSigned(transaction))
            //{
            //    return transaction
            //}

            if (wallet == null)
            {
                throw new ValidationError("Wallet must be provided when submitting an unsigned transaction");
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
            return wallet.Sign(tx, false).TxBlob;
        }
    }
}

