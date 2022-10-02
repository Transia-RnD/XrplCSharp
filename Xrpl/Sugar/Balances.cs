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
            AccountInfoRequest xrpRequest = new AccountInfoRequest(address) {
                LedgerHash = ledgerHash,
                LedgerIndex = lederIndex ?? index,
                Strict = true
            };
            AccountInfo accountInfo = await client.AccountInfo(xrpRequest);
            return accountInfo.AccountData.Balance.ValueAsXrp.ToString();
        }
    }
}