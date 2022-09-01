using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xrpl.Client;
using Xrpl.Client.Models.Common;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;

namespace Xrpl.Sugar
{
    public class GetLedgerSugar
    {
        public async static Task<uint> GetLedgerIndex(IRippleClient client)
        {
            LedgerIndex index = new LedgerIndex(LedgerIndexType.Current);
            LedgerRequest request = new LedgerRequest() { LedgerIndex = index };
            LOLedger ledgerResponse = await client.Ledger(request);
            LedgerEntity ledgerEntity = (LedgerEntity)ledgerResponse.LedgerEntity;
            Debug.WriteLine(ledgerEntity.LedgerIndex);
            return Convert.ToUInt32(ledgerEntity.LedgerIndex, 16);
        }
    }
}

