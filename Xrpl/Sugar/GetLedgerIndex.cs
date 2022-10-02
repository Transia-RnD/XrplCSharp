using System;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/sugar/getLedgerIndex.ts

namespace Xrpl.Sugar
{
    public class GetLedgerSugar
    {
        /// <summary>
        /// Returns the index of the most recently validated ledger.
        /// </summary>
        /// <param name="client">The Client used to connect to the ledger.</param>
        // <returns>The most recently validated ledger index.</returns>
        public static async Task<uint> GetLedgerIndex(IXrplClient client)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            LedgerRequest request = new LedgerRequest() { LedgerIndex = index };
            LOLedger ledgerResponse = await client.Ledger(request);
            LedgerEntity ledgerEntity = (LedgerEntity)ledgerResponse.LedgerEntity;
            return Convert.ToUInt32(ledgerEntity.LedgerIndex, 16);
        }
    }
}

