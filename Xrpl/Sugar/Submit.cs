using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Xrpl.BinaryCodec;
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
        public static int LEDGER_CLOSE_TIME = 1000;
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
            string transaction,
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
        public static async Task<Submit> SubmitRequest(IXrplClient client, Dictionary<string, dynamic> signedTransaction, bool failHard)
        {
            if (!IsSigned(signedTransaction))
            {
                throw new ValidationException("Transaction must be signed");
            }
            string signedTxEncoded = XrplBinaryCodec.Encode(signedTransaction);
            SubmitRequest request = new SubmitRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = IsAccountDelete(signedTransaction) || failHard };
            var response = await client.GRequest<Submit, SubmitRequest>(request);
            return response;
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
            if (!IsSigned(signedTransaction))
            {
                throw new ValidationException("Transaction must be signed");
            }
            SubmitRequest request = new SubmitRequest { Command = "submit", TxBlob = signedTransaction, FailHard = IsAccountDelete(signedTransaction) || failHard };
            var response = await client.GRequest<Submit, SubmitRequest>(request);
            return response;
        }

        /// <summary>
        /// The core logic of reliable submission.  This polls the ledger until the result of the
        /// transaction can be considered final, meaning it has either been included in a
        /// validated ledger, or the transaction's lastLedgerSequence has been surpassed by the
        /// latest ledger sequence (meaning it will never be included in a validated ledger).
        /// </summary>
        /// <param name="client">The client to use for the request.</param>
        /// <param name="txHash">The hash of the transaction to check.</param>
        /// <param name="lastLedger">The last ledger sequence of the transaction.</param>
        /// <param name="submissionResult">The preliminary result of the transaction.</param>
        /// <returns>The final result of the transaction.</returns>
        /// <exception cref="XrplError">Thrown if the transaction's lastLedgerSequence has been surpassed by the latest ledger sequence.</exception>
        /// <exception cref="Exception">Thrown if the transaction is not found.</exception>
        private async Task<TransactionResponseCommon> WaitForFinalTransactionOutcome(
            XrplClient client,
            string txHash,
            int lastLedger,
            string submissionResult)
        {
            await Task.Delay(LEDGER_CLOSE_TIME);

            var latestLedger = await client.GetLedgerIndex();

            if (lastLedger < latestLedger)
            {
                throw new XrplException(
                    $"The latest ledger sequence {latestLedger} is greater than the transaction's LastLedgerSequence ({lastLedger}).\n" +
                    $"Preliminary result: {submissionResult}");
            }

            TransactionResponseCommon txResponse = await client.Tx(new TxRequest(txHash));

            // error is of an unknown type and hence we assert type to extract the value we need.
            // eslint-disable-next-line @typescript-eslint/consistent-type-assertions,@typescript-eslint/no-unsafe-member-access -- ^
            //var message = error?.Data?.Error as string;
            //if (message == "txnNotFound")
            //{
            //    return await WaitForFinalTransactionOutcome(client, txHash, lastLedger, submissionResult);
            //}
            //throw new Exception($"{message} \n Preliminary result: {submissionResult}.\nFull error details: {error}");

            //if ((bool)txResponse.Validated)
            //{
            //    return txResponse;
            //}

            return await WaitForFinalTransactionOutcome(client, txHash, lastLedger, submissionResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool IsSigned(string transaction)
        {
            Dictionary<string, dynamic> tx = XrplBinaryCodec.Decode(transaction).ToObject<Dictionary<string, dynamic>>();
            return IsSigned(tx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool IsSigned(Dictionary<string, dynamic> transaction)
        {
            transaction.TryGetValue("SigningPubKey", out var SigningPubKey);
            transaction.TryGetValue("TxnSignature", out var TxnSignature);
            return SigningPubKey != null || TxnSignature != null;
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
            if (IsSigned(transaction))
            {
                return XrplBinaryCodec.Encode(transaction);
            }

            if (wallet == null)
            {
                throw new ValidationException("Wallet must be provided when submitting an unsigned transaction");
            }
            Dictionary<string, dynamic> tx = transaction;
            
            if (autofill)
            {
                tx = await client.Autofill(tx);
            }
            return wallet.Sign(tx, false).TxBlob;
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
            string transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet? wallet = null
        )
        {
            if (IsSigned(transaction))
            {
                return transaction;
            }

            if (wallet == null)
            {
                throw new ValidationException("Wallet must be provided when submitting an unsigned transaction");
            }
            Dictionary<string, dynamic> tx = XrplBinaryCodec.Decode(transaction).ToObject<Dictionary<string, dynamic>>();
            if (autofill)
            {
                tx = await client.Autofill(tx);
            }
            return wallet.Sign(tx, false).TxBlob;
        }

        /// <summary>
        /// Checks if the transaction is an AccountDelete transaction
        /// </summary>
        /// <param name="transaction">The transaction to check</param>
        /// <returns>True if the transaction is an AccountDelete transaction</returns>
        public static bool IsAccountDelete(Dictionary<string, dynamic> tx)
        {
            tx.TryGetValue("TransactionType", out var TransactionType);
            return TransactionType == "AccountDelete";
        }

        /// <summary>
        /// Checks if the transaction is an AccountDelete transaction
        /// </summary>
        /// <param name="transaction">The transaction to check</param>
        /// <returns>True if the transaction is an AccountDelete transaction</returns>
        public static bool IsAccountDelete(string transaction)
        {
            Dictionary<string, dynamic> tx = XrplBinaryCodec.Decode(transaction).ToObject<Dictionary<string, dynamic>>();
            tx.TryGetValue("TransactionType", out var TransactionType);
            return TransactionType == "AccountDelete";
        }
    }
}

