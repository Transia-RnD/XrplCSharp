

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/getBalanceChanges.ts

//todo DO
using static Xrpl.Models.Common.Common;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Xrpl.Models.Transactions;
using Xrpl.Sugar;
using Xrpl.Utils.Hashes.ShaMap;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Models.Common;

namespace Xrpl.Utils
{

    public class BalanceChange
    {
        public string Account { get; set; }
        public Balance Balance { get; set; }
    }

    public class Fields
    {
        public string Account { get; set; }
        public Currency Balance { get; set; }
        public IssuedCurrencyAmount LowLimit { get; set; }
        public IssuedCurrencyAmount HighLimit { get; set; }
        public Dictionary<string, object> FieldDict { get; set; }
    }

    public class NormalizedNode
    {
        public string NodeType { get; set; }
        public string LedgerEntryType { get; set; }
        public string LedgerIndex { get; set; }
        public Fields NewFields { get; set; }
        public Fields FinalFields { get; set; }
        public Fields PreviousFields { get; set; }
        public string PreviousTxnID { get; set; }
        public int PreviousTxnLgrSeq { get; set; }
    }

    public static class GetBalanceChanges
    {

        //todo need help with NormalizeNodes
        //public static NormalizedNode NormalizeNode(this INode affectedNode)
        //{
        //    var diffType = affectedNode[0];
        //    NormalizedNode node = affectedNode[diffType] as NormalizedNode;
        //    return new NormalizedNode
        //    {
        //        NodeType = diffType,
        //        LedgerEntryType = node.LedgerEntryType,
        //        LedgerIndex = node.LedgerIndex,
        //        NewFields = node.NewFields,
        //        FinalFields = node.FinalFields,
        //        PreviousFields = node.PreviousFields
        //    };
        //}

        //public static List<NormalizedNode> NormalizeNodes(this TransactionMetadata metadata)
        //{
        //    if (metadata.AffectedNodes.Count == 0)
        //    {
        //        return new List<NormalizedNode>();
        //    }

        //    return metadata.AffectedNodes.Select(NormalizeNodes).ToList();
        //}

        public static List<(string account, List<Balance> balances)> GroupByAccount(this List<BalanceChange> balanceChanges)
        {
            var grouped = balanceChanges.GroupBy(node => node.Account);
            return grouped.Select(item => (item.Key, item.Select(i => i.Balance).ToList())).ToList();
        }

        public static BigInteger GetValue(object balance) //todo need to check
        {
            if (balance is string val)
            {
                return BigInteger.Parse(val, NumberStyles.AllowLeadingSign
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                             | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                             | NumberStyles.AllowExponent
                                             | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            }

            var json = JObject.Parse(JsonConvert.SerializeObject(balance));
            return BigInteger.Parse(json["Value"].ToString(), NumberStyles.AllowLeadingSign
                                                   | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
                                                   | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                                                   | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                                   | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                                   | NumberStyles.AllowExponent
                                                   | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        public static BigInteger? ComputeBalanceChange(this NormalizedNode node)
        {
            BigInteger? value = null;
            if (node.NewFields?.Balance != null)
            {
                value = GetValue(node.NewFields.Balance);
            }
            else if (node.PreviousFields?.Balance != null && node.FinalFields?.Balance != null)
            {
                value = GetValue(node.FinalFields.Balance) - GetValue(node.PreviousFields.Balance);
            }

            if (value is null || value.Value.IsZero)
            {
                return null;
            }

            return value;
        }

        public static (string account, Balance balance) GetXRPQuantity(this NormalizedNode node)
        {
            var value = ComputeBalanceChange(node);

            if (value == null)
            {
                return (null, null);
            }

            return (node.FinalFields?.Account ?? node.NewFields?.Account,
                new Balance
                {
                    Currency = "XRP",
                    Value = XrpConversion.DropsToXrp(value.Value.ToString())
                });
        }

        public static BalanceChange FlipTrustlinePerspective(this BalanceChange balanceChange)
        {
            var negatedBalance = BigInteger.Parse(
                balanceChange.Balance.Value, NumberStyles.AllowLeadingSign
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowDecimalPoint)
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                                             | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                             | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                                             | NumberStyles.AllowExponent
                                             | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);//.Negate();
            return new BalanceChange
            {
                Account = balanceChange.Balance.Issuer,
                Balance = new Balance
                {
                    Issuer = balanceChange.Account,
                    Currency = balanceChange.Balance.Currency,
                    Value = negatedBalance.ToString()
                }
            };
        }

        public static List<BalanceChange> GetTrustlineQuantity(this NormalizedNode node)
        {
            var value = ComputeBalanceChange(node);

            if (value == null)
            {
                return null;
            }
            /*
             * A trustline can be created with a non-zero starting balance.
             * If an offer is placed to acquire an asset with no existing trustline,
             * the trustline can be created when the offer is taken.
             */
            var fields = node.NewFields ?? node.FinalFields;
            var result = new BalanceChange
            {
                Account = fields?.LowLimit?.Issuer,
                Balance = new Balance
                {
                    Issuer = fields?.HighLimit?.Issuer,
                    Currency = fields?.Balance.CurrencyCode,
                    Value = value.ToString()
                }
            };
            return new List<BalanceChange> { result, FlipTrustlinePerspective(result) };
        }
        ///// <summary> //todo need help with NormalizeNodes
        ///// Computes the complete list of every balance that changed in the ledger as a result of the given transaction.
        ///// </summary>
        ///// <param name="metadata">Transaction metadata.</param>
        ///// <returns>Parsed balance changes.</returns>
        //public static List<(string account, List<Balance> balances)> GetBalanceChanges(this TransactionMetadata metadata)
        //{
        //    var quantities = NormalizeNodes(metadata).Select(
        //        node =>
        //        {
        //            if (node.LedgerEntryType == "AccountRoot")
        //            {
        //                var xrpQuantity = GetXRPQuantity(node);
        //                if (xrpQuantity.account == null)
        //                {
        //                    return new List<BalanceChange>();
        //                }

        //                return new List<BalanceChange>() { new BalanceChange() { Account = xrpQuantity.account, Balance = xrpQuantity.balance } };
        //            }

        //            if (node.LedgerEntryType == "RippleState")
        //            {
        //                var trustlineQuantity = GetTrustlineQuantity(node);
        //                if (trustlineQuantity == null)
        //                {
        //                    return new List<BalanceChange>();
        //                }

        //                return trustlineQuantity;
        //            }

        //            return new List<BalanceChange>();
        //        }).ToList();
        //    return GroupByAccount(quantities.SelectMany(q => q).ToList());
        //}
    }
}

