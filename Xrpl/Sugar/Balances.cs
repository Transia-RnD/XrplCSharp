using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Xrpl.Client;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/balances.ts

namespace Xrpl.Sugar
{
    public class Balance
    {
        public string Value { get; set; }
        public string Currency { get; set; }
        public string Issuer { get; set; }
    }

    public class GetBalancesOptions
    {
        public string? LedgerHash { get; set; }
        public LedgerIndex? LedgerIndex { get; set; }
        public string Peer { get; set; }
        public int? Limit { get; set; }
    }

    public static class BalancesSugar
    {

        public static IEnumerable<Balance> FormatBalances(this IEnumerable<TrustLine> trustlines) =>
            trustlines.Select(Map);

        public static Balance Map(this TrustLine trustline) =>
                    new Balance()
                    {
                        Value = trustline.Balance,
                        Currency = trustline.Currency,
                        Issuer = trustline.Account,
                    };

        /// <summary>
        /// Get the XRP balance for an account.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="address">Address of the account to retrieve XRP balance.</param>
        /// <param name="lederIndex">Retrieve the account balances at a given ledgerIndex.</param>
        /// <param name="ledgerHash">Retrieve the account balances at the ledger with a given ledger_hash.</param>
        /// <returns/> The XRP balance of the account (as a string).
        public static async Task<string> GetXrpBalance(this XrplClient client, string address, string? ledgerHash = null, LedgerIndex? lederIndex = null)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            AccountInfoRequest xrpRequest = new AccountInfoRequest(address)
            {
                LedgerHash = ledgerHash,
                LedgerIndex = lederIndex ?? index,
                Strict = true
            };
            AccountInfo accountInfo = await client.AccountInfo(xrpRequest);
            return accountInfo.AccountData.Balance.ValueAsXrp.ToString();
        }

        public static async Task<List<Balance>> GetBalances(this XrplClient client, string address, GetBalancesOptions options = null)
        {
            var linesRequest = new AccountLinesRequest(address)
            {
                Command = "account_lines",
                LedgerIndex = options?.LedgerIndex ?? new LedgerIndex(LedgerIndexType.Validated),
                LedgerHash = options?.LedgerHash,
                Peer = options?.Peer,
                Limit = options?.Limit
            };

            var response = await client.AccountLines(linesRequest);
            var lines = response.TrustLines;
            while (response.Marker is not null && lines.Count > 0)
            {
                linesRequest.Marker = response.Marker;
                response = await client.AccountLines(linesRequest);
                if (response.TrustLines.Count > 0)
                    lines.AddRange(response.TrustLines);
                if (options?.Limit is not null && lines.Count >= options.Limit)
                    break;
            }
            var balances = lines.FormatBalances().ToList();

            if (options?.Peer == null)
            {
                var xrp_balance = await GetXrpBalance(client, address, options?.LedgerHash, options?.LedgerIndex);
                if (!string.IsNullOrWhiteSpace(xrp_balance))
                {
                    balances.Insert(0, new Balance { Currency = "XRP", Value = xrp_balance });
                }

            }

            return balances;
        }
    }
}