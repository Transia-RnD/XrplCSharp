//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/createCrossChainPayment.ts
using System.Collections.Generic;
using System.Linq;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transactions;

namespace Xrpl.Utils
{
    public static class CrossChainPayment
    {
        /// <summary>
        /// Creates a cross-chain payment transaction.
        /// </summary>
        /// <param name="payment">he initial payment transaction. If the transaction is
        /// signed, then it will need to be re-signed.There must be no more than 2
        /// memos, since one memo is used for the sidechain destination account.The
        /// destination must be the sidechain's door account.</param>
        /// <param name="destAccount"> the destination account on the sidechain.</param>
        /// <returns>A cross-chain payment transaction, where the mainchain door account
        ///is the `Destination` and the destination account on the sidechain is encoded
        ///in the memos.</returns>
        /// <exception cref="XrplException">if there are more than 2 memos.</exception>
        public static Payment CreateCrossChainPayment(this Payment payment, string destAccount) //todo check it
        {
            var destAccountHex = destAccount.ConvertStringToHex();
            var destAccountMemo = new Memo { MemoData = destAccountHex };

            var memos = payment.Memos?.Select(c => c.Memo).ToList() ?? new List<Memo>();
            if (memos.Count > 2)
            {
                throw new XrplException("Cannot have more than 2 memos in a cross-chain transaction.");
            }
            var newMemos = new List<Memo> { destAccountMemo };
            newMemos.AddRange(memos);

            var newPayment = new Payment 
            {
                TransactionType = payment.TransactionType,
                Account = payment.Account,
                AccountTxnID = payment.AccountTxnID,
                Amount = payment.Amount,
                DeliverMin = payment.DeliverMin,
                Destination = payment.Destination,
                DestinationTag = payment.DestinationTag,
                Fee = payment.Fee,
                Flags = payment.Flags,
                InvoiceID = payment.InvoiceID,
                LastLedgerSequence = payment.LastLedgerSequence,
                Meta = payment.Meta,
                Paths = payment.Paths,
                SendMax = payment.SendMax,
                Sequence = payment.Sequence,
                Signers = payment.Signers,
                SigningPublicKey = payment.SigningPublicKey,
                date = payment.date,
                inLedger = payment.inLedger,
                ledger_index = payment.ledger_index,

                Memos = newMemos.Select(c => new MemoWrapper() { Memo = c }).ToList(),
                TransactionSignature = null
            };

            return newPayment;
        }
    }
}
