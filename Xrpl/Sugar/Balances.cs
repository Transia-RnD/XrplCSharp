﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/balances.ts

namespace Xrpl.Sugar
{
    public class BalancesSugar
    {
        public class Balance
        {
            public string value { get; set; }
            public string currency { get; set; }
            public string issuer { get; set; }
        }

        public class GetBalancesOptions
        {
            public string? LedgerHash { get; set; }
            public LedgerIndex? LedgerIndex { get; set; }
            public string Peer { get; set; }
            public int Limit { get; set; }
        }

        /// <summary>
        /// Get the XRP balance for an account.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="address">Address of the account to retrieve XRP balance.</param>
        /// <param name="lederIndex">Retrieve the account balances at a given ledgerIndex.</param>
        /// <param name="ledgerHash">Retrieve the account balances at the ledger with a given ledger_hash.</param>
        /// <returns/> The XRP balance of the account (as a string).
        public static async Task<string> GetXrpBalance(XrplClient client, string address, string? ledgerHash = null, LedgerIndex? lederIndex = null)
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

        //public async Task<List<Balance>> GetBalances(XrplClient client, string address, GetBalancesOptions options = null)
        //{
        //    var balances = new List<Balance>();
        //    var xrpPromise = Task.FromResult("");
        //    if (options?.Peer == null)
        //    {
        //        xrpPromise = GetXrpBalance(client, address, options?.LedgerHash, options?.LedgerIndex);
        //    }

        //    var linesRequest = new AccountLinesRequest
        //    {
        //        Command = "account_lines",
        //        Account = address,
        //        LedgerIndex = options?.LedgerIndex ?? new LedgerIndex(LedgerIndexType.Validated),
        //        LedgerHash = options?.LedgerHash,
        //        Peer = options?.Peer,
        //        Limit = options?.Limit
        //    };
        //    var linesPromise = RequestAll(linesRequest);

        //    await Task.WhenAll(xrpPromise, linesPromise).ContinueWith(async (t) =>
        //    {
        //        var xrpBalance = await xrpPromise;
        //        var linesResponses = await linesPromise;
        //        var accountLinesBalance = linesResponses.SelectMany(response => FormatBalances(response.Result.Lines));
        //        if (xrpBalance != "")
        //        {
        //            balances.Add(new Balance { Currency = "XRP", Value = xrpBalance });
        //        }
        //        balances.AddRange(accountLinesBalance);
        //    });
        //    return balances.Take(options?.Limit ?? balances.Count).ToList();
        //}
    }
}