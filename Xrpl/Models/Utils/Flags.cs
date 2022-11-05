using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Transaction;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/utils/flags.ts

namespace Xrpl.Models.Utils
{
    public class Flags
    {
        /// <summary>
        /// Sets a transaction's flags to its numeric representation.
        /// </summary>
        /// <param name="tx"> A transaction to set its flags to its numeric representation.</param>
        public static void SetTransactionFlagsToNumber(Dictionary<string, dynamic> tx)
        {
            if (!tx.TryGetValue("Flags", out var Flags))
            {
                tx.Add("Flags",0u);
                return;
            }
            if (Flags is null)
            {
                tx["Flags"] = 0u;
                return;
            }
            if (Flags is uint)
            {
                return;
            }
            //todo unattainable code

            tx["Flags"] = tx["TransactionType"] switch
            {
                TransactionType.AccountSet => ConvertAccountSetFlagsToNumber(Flags),
                //TransactionType.AccountDelete => expr,
                //TransactionType.CheckCancel => expr,
                //TransactionType.CheckCash => expr,
                //TransactionType.CheckCreate => expr,
                //TransactionType.DepositPreauth => expr,
                //TransactionType.EscrowCancel => expr,
                //TransactionType.EscrowCreate => expr,
                //TransactionType.EscrowFinish => expr,
                //TransactionType.NFTokenAcceptOffer => expr,
                //TransactionType.NFTokenBurn => expr,
                //TransactionType.NFTokenCancelOffer => expr,
                //TransactionType.NFTokenCreateOffer => expr,
                //TransactionType.NFTokenMint => expr,
                //TransactionType.OfferCancel => expr,
                TransactionType.OfferCreate => ConvertOfferCreateFlagsToNumber(Flags),
                TransactionType.Payment => ConvertPaymentTransactionFlagsToNumber(Flags),
                TransactionType.PaymentChannelClaim => ConvertPaymentChannelClaimFlagsToNumber(Flags),
                //TransactionType.PaymentChannelCreate => expr,
                //TransactionType.PaymentChannelFund => expr,
                //TransactionType.SetRegularKey => expr,
                //TransactionType.SignerListSet => expr,
                //TransactionType.TicketCreate => expr,
                TransactionType.TrustSet => ConvertTrustSetFlagsToNumber(Flags),
                _ => 0
            };
        }

        public static uint ConvertAccountSetFlagsToNumber(object flags)
        {
            return ReduceFlags((JToken)flags, AccountSetTfFlags.asfAccountTxnID);
        }

        public static uint ConvertOfferCreateFlagsToNumber(object flags)
        {
            return ReduceFlags((JToken)flags, AccountSetTfFlags.asfAccountTxnID);
        }

        public static uint ConvertPaymentChannelClaimFlagsToNumber(object flags)
        {
            return ReduceFlags((JToken)flags, AccountSetTfFlags.asfAccountTxnID);
        }

        public static uint ConvertPaymentTransactionFlagsToNumber(object flags)
        {
            return ReduceFlags((JToken)flags, AccountSetTfFlags.asfAccountTxnID);
        }

        public static uint ConvertTrustSetFlagsToNumber(object flags)
        {
            return ReduceFlags((JToken)flags, AccountSetTfFlags.asfAccountTxnID);
        }

        public static uint ReduceFlags(JToken flags, dynamic flagEnum)
        {
            JObject inner = flags.Value<JObject>();

            List<string> keys = inner.Properties().Select(p => p.Name).ToList();

            return (uint)inner.Properties().Select(p => p.Name).Aggregate(0, (resultFlags, flag) =>
            {
                if (flagEnum[flag] == null)
                {
                    throw new ValidationError("flag ${ flag } doesn't exist in flagEnum: ${JSON.stringify(flagEnum)}");
                }
                return flags[flag] != null ? resultFlags | flagEnum[flag] : resultFlags;
            });
        }
    }
}