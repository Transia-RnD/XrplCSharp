using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Xrpl.ClientLib.Exceptions;
using Xrpl.Models.Transactions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/utils/flags.ts

namespace Xrpl.Models.Utils
{
    public class Flags
    {
        public static void SetTransactionFlagsToNumber(ITransactionCommon tx)
        {
            if (tx.Flags == null)
            {
                tx.Flags = 0;
              return;
            }
            if (tx.Flags is uint)
            {
                return;
            }

            switch (tx.TransactionType.ToString())
            {
                case "AccountSet":
                    tx.Flags = ConvertAccountSetFlagsToNumber(tx.Flags);
                    break;
                case "OfferCreate":
                    tx.Flags = ConvertOfferCreateFlagsToNumber(tx.Flags);
                    break;
                case "PaymentChannelClaim":
                    tx.Flags = ConvertPaymentChannelClaimFlagsToNumber(tx.Flags);
                    break;
                case "Payment":
                    tx.Flags = ConvertPaymentTransactionFlagsToNumber(tx.Flags);
                    break;
                case "TrustSet":
                    tx.Flags = ConvertTrustSetFlagsToNumber(tx.Flags);
                    break;
                default:
                    tx.Flags = 0;
                    break;
            }
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