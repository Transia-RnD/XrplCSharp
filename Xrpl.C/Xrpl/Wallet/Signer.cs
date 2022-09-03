using System;
using System.Collections.Generic;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Address.Codec;
using Ripple.Binary.Codec;
using Ripple.Binary.Codec.Enums;
using Ripple.Binary.Codec.Transactions;
using Ripple.Binary.Codec.Types;
using Ripple.Binary.Codec.Util;
using Ripple.Keypairs;
using Xrpl.Client.Exceptions;
using Xrpl.Client.Models.Transactions;
using Ripple.Keypairs.Extensions;
using Org.BouncyCastle.Math;
using System.Diagnostics;
using static Ripple.Address.Codec.B58;


// ReSharper disable RedundantArgumentNameForLiteralExpression

namespace Xrpl.XrplWallet
{
    public class Signer
    {
        public static string Multisign(Dictionary<string, dynamic>[] txs)
        {
            if (txs.Length == 0)
            {
                throw new ValidationError("There were 0 transactions to multisign");
            }
            foreach (Dictionary<string, dynamic> i in txs)
            {
                Dictionary<string, dynamic> tx = GetDecodedTransaction(i);
                //TxFormat.Validate(tx);
                if (tx["Signers"] == null || tx["Signers"].Length == 0)
                {
                    throw new ValidationError("For multisigning all transactions must include a Signers field containing an array of signatures. You may have forgotten to pass the 'forMultisign' parameter when signing.");
                }
                if (tx["SigningPubKey"] != "")
                {
                    throw new ValidationError("SigningPubKey must be an empty string for all transactions when multisigning.");
                }
            }

            Dictionary<string, dynamic>[] decodedTransactions = { };
            //Dictionary<string, dynamic>[] decodedTransactions = transactions.map(
            //    (txOrBlob: string | Transaction) => {
            //        return GetDecodedTransaction(txOrBlob)
            //    },
            //  )
            ValidateTransactionEquivalence(decodedTransactions);
            return BinaryCodec.Encode(GetTransactionWithAllSigners(decodedTransactions));
        }

        public static string AuthorizeChannel(Wallet wallet, string channelID, string amount)
        {
            Dictionary<string, dynamic> json = new Dictionary<string, dynamic>();
            json.Add("channel", channelID);
            json.Add("amount", amount);
            string signatureData = BinaryCodec.EncodeForSigningClaim(json);
            Debug.WriteLine(signatureData);
            Debug.WriteLine(wallet.PrivateKey);
            return Keypairs.Sign(signatureData.FromHex(), wallet.PrivateKey);
        }

        public static bool VerifySignature(Dictionary<string, dynamic> tx)
        {
            Dictionary<string, dynamic> decodedTx = GetDecodedTransaction(tx);
            return Keypairs.Verify(
              BinaryCodec.EncodeForSigning(decodedTx).FromHex(),
              decodedTx["TxnSignature"],
              decodedTx["SigningPubKey"]
            );
        }

        public static bool VerifySignature(string tx)
        {
            Dictionary<string, dynamic> decodedTx = GetDecodedTransaction(tx);
            return Keypairs.Verify(
              BinaryCodec.EncodeForSigning(decodedTx).FromHex(),
              decodedTx["TxnSignature"],
              decodedTx["SigningPubKey"]
            );
        }

        public static Dictionary<string, dynamic> GetDecodedTransaction(Dictionary<string, dynamic>  txOrBlob)
        {
            return BinaryCodec.Decode(BinaryCodec.Encode(txOrBlob)).ToObject<Dictionary<string, dynamic>>();
            
        }

        public static Dictionary<string, dynamic> GetDecodedTransaction(string txOrBlob)
        {
            return BinaryCodec.Decode(txOrBlob).ToObject<Dictionary<string, dynamic>>();
        }

        public static void ValidateTransactionEquivalence(Dictionary<string, dynamic>[] transactions)
        {
            Dictionary<string, dynamic>  exampleTx = transactions[0];
            exampleTx["Signers"] = null;
            string exampleTransaction = exampleTx.ToString();
            //if (transactions.Slice(1).Some(tx) != exampleTransaction)
            //{
            //    throw new ValidationError("txJSON is not the same for all signedTransactions");
            //}
        }

        public static Dictionary<string, dynamic> GetTransactionWithAllSigners(Dictionary<string, dynamic>[] transactions)
        {
            // Signers must be sorted in the combined transaction - See compareSigners' documentation for more details
            Dictionary<string, dynamic> finalTx = transactions[0];
            //transactions
            //finalTx["Signers"] = sortedSigners;
            return finalTx;
        }

        public static int CompareSigners(Dictionary<string, dynamic> left, Dictionary<string, dynamic> right)
        {
            return AddressToBigNumber(left["Signer"]["Account"]) == AddressToBigNumber(right["Signer"]["Account"]);
        }

        public static BigInteger AddressToBigNumber(string address)
        {
            string hex = XrplCodec.DecodeAccountID(address).ToHex();
            return new BigInteger(hex, 16);
        }
    }
}