using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Xrpl.BinaryCodec;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Models;
using Xrpl.Models.Methods;
using Xrpl.Models.Transactions;
using Xrpl.Utils.Hashes;
using Xrpl.Wallet;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/submit.ts

namespace Xrpl.Sugar
{
    public static class SubmitSugar
    {
        private const int LEDGER_CLOSE_TIME = 1000;
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
        /// <returns>A promise that contains SubmitResponse</returns>
        public static async Task<Submit> Submit(this IXrplClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet wallet = null
        )
        {
            string signedTx = await client.GetSignedTx(transaction, autofill, false, wallet);
            return await SubmitRequest(client, signedTx, failHard);
        }
        /// <summary>
        /// Asynchronously submits a transaction and verifies that it has been included in a
        /// validated ledger(or has errored/will not be included for some reason).
        /// See[Reliable Transaction Submission] (https://xrpl.org/reliable-transaction-submission.html).
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="transaction">A transaction to autofill, sign and encode, and submit.</param>
        /// <param name="autofill">If true, autofill a transaction.</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <param name="wallet">A wallet to sign a transaction. It must be provided when submitting an unsigned transaction.</param>
        /// <returns>A promise that contains TxResponse, that will return when the transaction has been validated.</returns>
        public static async Task<TransactionResponseCommon> SubmitAndWait(this IXrplClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet wallet = null)
        {
            var signedTx = await client.GetSignedTx(transaction, autofill, failHard, wallet);
            var lastLedger = GetLastLedgerSequence(signedTx);
            if (lastLedger == null)
            {
                throw new ValidationException("Transaction must contain a LastLedgerSequence value for reliable submission.");
            }

            var response = await client.SubmitRequest(signedTx, failHard);
            var txHash = HashLedger.HashSignedTx(signedTx);
            return await WaitForFinalTransactionOutcome(
                client,
                txHash,
                lastLedger,
                response.EngineResult);
        }

        /// <summary>
        /// Encodes and submits a signed transaction.
        /// </summary>
        /// <param name="client">A Client.</param>
        /// <param name="signedTransaction">signed Transaction</param>
        /// <param name="failHard">If true, and the transaction fails locally, do not retry or relay the transaction to other servers.</param>
        /// <returns></returns>
        public static async Task<Submit> SubmitRequest(this IXrplClient client, object signedTransaction, bool failHard)
        {
            if (!IsSigned(signedTransaction))
            {
                throw new ValidationException("Transaction must be signed");
            }

            string signedTxEncoded = signedTransaction is string transaction ? transaction : XrplBinaryCodec.Encode(signedTransaction);
            SubmitRequest request = new SubmitRequest { Command = "submit", TxBlob = signedTxEncoded, FailHard = failHard };
            var response = await client.GRequest<Submit, SubmitRequest>(request);
            return response;
        }
        /// <summary>
        /// The core logic of reliable submission.This polls the ledger until the result of the
        /// transaction can be considered final, meaning it has either been included in a
        /// validated ledger, or the transaction's lastLedgerSequence has been surpassed by the
        /// latest ledger sequence (meaning it will never be included in a validated ledger).
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="TxHash"></param>
        /// <param name="lastLedger"></param>
        /// <param name="submissionResult"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private static async Task<TransactionResponseCommon> WaitForFinalTransactionOutcome(this IXrplClient Client, string TxHash, uint? lastLedger, string submissionResult)
        {
            await Task.Delay(LEDGER_CLOSE_TIME);
            var latestLedger = await Client.GetLedgerIndex();
            if (lastLedger < latestLedger)
            {
                throw new ValidationException(
                    "The latest ledger sequence ${ latestLedger } is greater than the transaction's LastLedgerSequence (${lastLedger}).\n" +
                    $"Preliminary result: {submissionResult}");
            }

            TransactionResponseCommon txResponse = null;
            try
            {
                txResponse = await Client.Tx(new TxRequest(TxHash));

            }
            catch (Exception error)
            {
                // error is of an unknown type and hence we assert type to extract the value we need.
                var message = error?.Data["Error"] as string;
                if (message == "txnNotFound")
                {
                    return await WaitForFinalTransactionOutcome(Client, TxHash, lastLedger, submissionResult);
                }
                throw new ValidationException($"{message} \n Preliminary result: {submissionResult}.\nFull error details: {error.Message}");
            }
            if (txResponse.Validated == true)
            {
                return txResponse;
            }

            return await WaitForFinalTransactionOutcome(Client, TxHash, lastLedger, submissionResult);
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
        public static async Task<string> GetSignedTx(this IXrplClient client,
            Dictionary<string, dynamic> transaction,
            bool autofill = false,
            bool failHard = false,
            XrplWallet? wallet = null
        )
        {
            //if (IsSigned(transaction))
            //{
            //    return transaction
            //}

            if (wallet == null)
            {
                throw new ValidationException("Wallet must be provided when submitting an unsigned transaction");
            }
            Dictionary<string, dynamic> tx = transaction;
            //var tx = transaction is string 
            //    ? // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- converts JsonObject to correct Transaction type
            //      (decode(transaction) as unknown as TransactionCommon)
            //    : transaction
            if (autofill)
            {
                tx = await client.Autofill(tx);
            }
            return wallet.Sign(tx, false).TxBlob;
        }

        public static bool IsSigned(object transaction)
        {
            if (transaction is Dictionary<string, dynamic> { } tx)
            {
                return tx.TryGetValue("SigningPubKey", out var SigningPubKey) && SigningPubKey is not null ||
                       tx.TryGetValue("TxnSignature", out var TxnSignature) && TxnSignature is not null;
            }
            else
            {
                var ob = XrplBinaryCodec.Encode(transaction);
                var json = JObject.Parse($"{ob}");
                return json.TryGetValue("SigningPubKey", out var SigningPubKey) && !string.IsNullOrWhiteSpace(SigningPubKey.ToString()) ||
                       json.TryGetValue("TxnSignature", out var TxnSignature) && !string.IsNullOrWhiteSpace(TxnSignature.ToString());
            }
        }
        /// <summary>
        /// checks if there is a LastLedgerSequence as a part of the transaction
        /// </summary>
        /// <param name="transaction">tx</param>
        /// <returns></returns>
        public static uint? GetLastLedgerSequence(object transaction)
        {
            if (transaction is Dictionary<string, dynamic> { } tx)
            {
                return tx.TryGetValue("LastLedgerSequence", out var LastLedgerSequence) && LastLedgerSequence is uint
                    ? LastLedgerSequence
                    : null;
            }
            else if (transaction is TransactionCommon txc)
            {
                return txc.LastLedgerSequence;
            }

            else
            {
                var ob = XrplBinaryCodec.Encode(transaction);
                var json = JObject.Parse($"{ob}");

                return json.TryGetValue("LastLedgerSequence", out var LastLedgerSequence) && uint.TryParse(LastLedgerSequence.ToString(), out var seq)
                    ? seq
                    : null;
            }

        }

        /// <summary>
        /// checks if the transaction is an AccountDelete transaction
        /// </summary>
        /// <param name="transaction">tx</param>
        /// <returns></returns>
        public static bool IsAccountDelete(object transaction)
        {
            if (transaction is Dictionary<string, dynamic> { } tx)
            {
                return tx.TryGetValue("TransactionType", out var TransactionType) && $"{TransactionType}" == "AccountDelete";
            }
            else if (transaction is TransactionCommon txc)
            {
                return txc.TransactionType == TransactionType.AccountDelete;
            }
            else
            {
                var ob = XrplBinaryCodec.Encode(transaction);
                var json = JObject.Parse($"{ob}");

                return json.TryGetValue("TransactionType", out var TransactionType) && TransactionType.ToString() == "AccountDelete";
            }

        }
    }
}

