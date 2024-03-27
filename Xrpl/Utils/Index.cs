using Newtonsoft.Json.Linq;

using System;

using Xrpl.AddressCodec;
using Xrpl.BinaryCodec;
using Xrpl.BinaryCodec.Ledger;
using Xrpl.Keypairs;
using Xrpl.Models.Subscriptions;
using Xrpl.Models.Transactions;

//https://github.com/XRPLF/xrpl.js/blob/45963b70356f4609781a6396407e2211fd15bcf1/packages/xrpl/src/utils/index.ts

namespace Xrpl.Utils
{
    public static class Utilities
    {
        public static bool IsValidSecret(string secret)
        {
            try
            {
                XrplKeypairs.DeriveKeypair(secret);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    
        public static string Encode(this TransactionCommon transactionOrLedgerEntry)
        {
            return XrplBinaryCodec.Encode(transactionOrLedgerEntry);
        }
    
        public static string EncodeForSigning(this TransactionCommon transaction)
        {
            return XrplBinaryCodec.EncodeForSigning(transaction);
        }
    
        public static string EncodeForSigningClaim(this PaymentChannelClaim paymentChannelClaim)
        {
            return XrplBinaryCodec.EncodeForSigningClaim(paymentChannelClaim);
        }
    
        public static string EncodeForMultiSigning(this TransactionCommon transaction, string signer)
        {
            return XrplBinaryCodec.EncodeForMulitSigning(transaction, signer);
        }
    
        public static JToken Decode(string hex)
        {
            return XrplBinaryCodec.Decode(hex);
        }
    
        public static bool IsValidAddress(string address)
        {
            return XrplAddressCodec.IsValidXAddress(address) || XrplCodec.IsValidClassicAddress(address);
        }
    
        public static bool HasNextPage(this BaseResponse response)
        {
            return response.Result.ContainsKey("marker");
        }
    }
}
