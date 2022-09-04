using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/balances.ts

namespace Xrpl.Sugar
{
    // <summary>
    /// Get the XRP balance for an account.
    /// </summary>
    /// <example>
    ///
    ///
    ///
    /// </example>
    /// <param name="client" Drops to convert to XRP. This can be a string, number, or BigNumber.
    /// <param name="address" Address of the account to retrieve XRP balance.
    /// <param name="ledgerHndex" Retrieve the account balances at a given ledger_index.
    /// <param name="ledgerHash" Retrieve the account balances at the ledger with a given ledger_hash.
    /// <returns/> The XRP balance of the account (as a string).
    public class BalancesSugar
    {
        public static async Task<string> GetXrpBalance(RippleClient client, string address, string? ledgerHash = null, LedgerIndex? lederIndex = null)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Validated);
            AccountInfoRequest xrpRequest = new AccountInfoRequest(address) {
                LedgerHash = ledgerHash,
                LedgerIndex = lederIndex ?? index,
                Strict = true
            };
            AccountInfo accountInfo = await client.AccountInfo(xrpRequest);
            Debug.WriteLine(XrpConversion.DropsToXrp(accountInfo.AccountData.Balance.ToString()));
            return accountInfo.AccountData.Balance.ValueAsXrp.ToString();
        }
    }
}